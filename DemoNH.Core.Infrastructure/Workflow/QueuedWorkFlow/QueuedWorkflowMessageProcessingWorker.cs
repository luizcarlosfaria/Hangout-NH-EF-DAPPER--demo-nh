using DemoNH.Core.Infrastructure.Extensions;
using DemoNH.Core.Infrastructure.MessageQueuing;
using DemoNH.Core.Infrastructure.Workflow.QueuedWorkFlow.Exceptions;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DemoNH.Core.Infrastructure.Workflow.QueuedWorkFlow
{
	/// <summary>
	/// QueuedWorkflowMessageProcessingWorker<RequestType, ResponseType> handle queue OnMessage event to delegate to your the behavior for your business implementation.
	/// </summary>
	/// <typeparam name="RequestType"></typeparam>
	/// <typeparam name="ResponseType"></typeparam>
	public class QueuedWorkflowMessageProcessingWorker : IMessageProcessingWorker
	{
		public static volatile Logger Logger = NLog.LogManager.GetLogger("QueuedWorkflowMessageProcessingWorker");

		/// <summary>
		/// Property to define an IQueueClient
		/// </summary>
		private IQueueClient _queueClient;

		/// <summary>
		/// Property to define your business service or process. This is a object to reduce the coupling between business and infrastructure classes
		/// </summary>
		private object _listenerObject;

		/// <summary>
		/// Property to define the name of method that contains your business implementation inside a ListenerObject
		/// </summary>
		private MethodInfo _listenerMethod { get; set; }

		/// <summary>
		/// If object definned in ListenerObject implements IFlowNotifier<RequestType, ResponseType> it can handle events changind de default workflow
		/// </summary>
		private volatile IFlowNotifier _flowNotifierImpl;

		/// <summary>
		/// Define a full configured route, used to ensure queue, exchange and binding to receive request messages
		/// </summary>
		public AmqpBasedRoute ReceiveRoute { get; private set; }

		/// <summary>
		/// Define a full configured route, used to ensure queue, exchange and binding to send response messages
		/// </summary>		
		private AmqpBasedRoute _successResponseRoute;

		/// <summary>
		/// Define a full configured route, used to ensure queue, exchange and binding to send failure response messages
		/// </summary>
		private AmqpBasedRoute _failureResponseRoute;

		private ExceptionStrategy _errorFlowStrategy;

		private bool _canUseRequestAsResponse;

		public QueuedWorkflowMessageProcessingWorker(IQueueClient queueClient, object listenerObject, MethodInfo listenerMethod, QueuedTransition queuedTransition, QueuedTransition nextQueuedTransition, bool createZombieQueues)
		{
			Contract.Requires(queueClient.IsNotNull(), "No queueClient specified: Either specify a non-null value for the 'queueClient' constructor argument.");
			Contract.Requires(listenerObject.IsNotNull(), "No listenerObject specified: Either specify a non-null value for the 'listenerObject' constructor argument.");
			Contract.Requires(listenerMethod.IsNotNull(), "No listenerMethod specified: Either specify a non-null value for the 'listenerMethod' constructor argument.");
			this._queueClient = queueClient;
			this._listenerObject = listenerObject;
			this._listenerMethod = listenerMethod;

			if (this._listenerObject is IFlowNotifier)
			{
				this._flowNotifierImpl = (IFlowNotifier)this._listenerObject;
			}

			Type inType = this._listenerMethod.GetUniqueAndExpectedInputParameterType();
			Type outType = this._listenerMethod.GetReturnValueType();
			if (outType == typeof(void))
				this._canUseRequestAsResponse = true;
			else
				this._canUseRequestAsResponse = outType.IsAssignableFrom(inType);

			//Receive Route é usada para garantir a criação da rota no RabbitMQ
			this.ReceiveRoute = new AmqpBasedRoute()
			{
				ExchangeName = queuedTransition.ExchangeName,					//Criando ExchangeName Default
				QueueName = queuedTransition.LogicalQueueName + ".Process",		//Queue de recebimento (Fila Principal)
				RoutingKey = queuedTransition.BuildRoutingKey()					//Binding Entre a ExchangeName e a Fila
			};
			this.ReceiveRoute.EnsureAll(this._queueClient);

			if (createZombieQueues)
			{
				//Filas Zombie não precisam ser armazenadas em variáveis, só precisam ser configuradas
				//corretamente no RabbitMQ
				AmqpBasedRoute zombieConfiguration = new AmqpBasedRoute()
				{
					ExchangeName = this.ReceiveRoute.ExchangeName,
					QueueName = queuedTransition.LogicalQueueName + ".Zombie",
					RoutingKey = this.ReceiveRoute.RoutingKey
				};
				zombieConfiguration.EnsureAll(this._queueClient);
			}

			//Caso não seja um passo final, é necessário armazenar e tratar as respostas para o próximo step
			if (nextQueuedTransition != null)
			{
				this._successResponseRoute = new AmqpBasedRoute()
				{
					QueueName = nextQueuedTransition.LogicalQueueName + ".Process",
					ExchangeName = nextQueuedTransition.ExchangeName,
					RoutingKey = nextQueuedTransition.BuildRoutingKey(),
				};
				this._successResponseRoute.EnsureAll(this._queueClient);
			}

			this._errorFlowStrategy = queuedTransition.ErrorFlowStrategy;
			//Nesse passo as estratégias são usadas para configurar o comportamento adequado para falhas
			if (this._errorFlowStrategy == ExceptionStrategy.SendToErrorQueue)
			{
				//Se ao encontrar um erro, é necessário enviar para a fila de erro, todo o pipe de erro é criado
				//isso conta com a criação da fila, e dos bindings no RabbitMQ
				this._failureResponseRoute = new AmqpBasedRoute()
				{
					QueueName = queuedTransition.LogicalQueueName + ".Failure",
					ExchangeName = this.ReceiveRoute.ExchangeName,
					RoutingKey = queuedTransition.BuildFailureRoutingKey()
				};
				this._failureResponseRoute.EnsureAll(this._queueClient);
			}
			else if (this._errorFlowStrategy == ExceptionStrategy.Requeue)
			{
				//Nesse caso, ao gerar um erro na fila a mensagem volta para a fila, portanto
				//a rota usada é escolhida como a mesma rota de recebimento, forçando um requeue
				this._failureResponseRoute = this.ReceiveRoute;
			}
			else if (this._errorFlowStrategy == ExceptionStrategy.SendToNextStepQueue)
			{
				//Nesse caso temos a possibilidade de continuar o fluxo, mesmo que uma
				//exceção seja lançada.
				if (this._successResponseRoute == null)
					throw new InvalidOperationException("The ErrorFlowStrategy property cannot be setted with SendToNextStepQueue value when current step is a final step.");
				this._failureResponseRoute = this._successResponseRoute;
			}
		}

		public void OnMessage(object request, IMessageFeedbackSender feedbackSender)
		{
			if (this.HandleStartAndCancel(request).Cancel == false)
			{
			RetryEntryPoint:

				InvokeResult invokeResult = this.InvokeListenerMethodOnListenerObject(request);

				if (invokeResult.Success)
				{
					List<object> responses = new List<object>();
					if (invokeResult.HasValue)
					{
						if (invokeResult.ReturnedValue is IEnumerable)
						{
							foreach (var itemOfReturnedValue in invokeResult.ReturnedValue.To<IEnumerable>())
							{
								responses.Add(itemOfReturnedValue);
							}
						}
						else
						{
							responses.Add(invokeResult.ReturnedValue);
						}
					}
					else if (this._canUseRequestAsResponse)
					{
						responses.Add(request);
					}
					else
					{
						throw new InvalidCastException("Unsupported queued method result");
					}
					this.HandlePreSucessResponse(request, responses);
					this.HandleResult(this._successResponseRoute, responses);
					this.HandlePostSuccessResponse(request, responses);
				}
				else if (invokeResult.Exception != null)
				{
					bool exemptionClaimedRequeue = this.ExemptionClaimedRequeue(invokeResult.Exception);
					bool exemptionClaimedRetry = this.ExemptionClaimedRetry(invokeResult.Exception);
					if (exemptionClaimedRequeue)
					{
						this.HandleResult(this.ReceiveRoute, new List<object>() { request });
						this.HandlePostFailureResponse(request, invokeResult.Exception, exemptionClaimedRequeue ? ExceptionStrategy.Requeue : this._errorFlowStrategy);
					}
					else if (exemptionClaimedRetry)
					{
						goto RetryEntryPoint;
					}
					else
					{
						this.HandleResult(this._failureResponseRoute, new List<object>() { request });
						this.HandlePostFailureResponse(request, invokeResult.Exception, exemptionClaimedRequeue ? ExceptionStrategy.Requeue : this._errorFlowStrategy);
					}
				}
			}
		}

		private bool ExistsAnyExceptionOfType<TException>(Exception exception)
			where TException : Exception
		{
			while (exception != null)
			{
				if (exception is TException)
					return true;
				exception = exception.InnerException;
			}
			return false;
		}

		private bool ExemptionClaimedRequeue(Exception exception)
		{
			return this.ExistsAnyExceptionOfType<FlowRejectAndRequeueException>(exception);
		}

		private bool ExemptionClaimedRetry(Exception exception)
		{
			return this.ExistsAnyExceptionOfType<FlowRetryException>(exception);
		}

		private CancelEventArgs HandleStartAndCancel(object request)
		{
			CancelEventArgs cancellation = new CancelEventArgs(cancel: false);
			if (this._flowNotifierImpl != null)
			{
				this._flowNotifierImpl.OnStart(request, cancellation);
				if (cancellation.Cancel)
				{
					this._flowNotifierImpl.OnCancel(request);
				}
			}
			return cancellation;
		}

		private void HandlePreSucessResponse(object request, IEnumerable responses)
		{
			if (this._flowNotifierImpl != null)
				this._flowNotifierImpl.OnProcessingSuccess(request, responses);
		}

		private void HandlePostSuccessResponse(object request, IEnumerable responses)
		{
			if (this._flowNotifierImpl != null)
				this._flowNotifierImpl.OnSuccess(request, responses);
		}

		private void HandlePostFailureResponse(object request, Exception exception, ExceptionStrategy usedExceptionStrategy)
		{
			if (this._flowNotifierImpl != null)
			{
				this._flowNotifierImpl.OnFailure(request, exception, this._errorFlowStrategy, usedExceptionStrategy);
			}
		}

		protected InvokeResult InvokeListenerMethodOnListenerObject(object requestObject)
		{
			InvokeResult returnValue = new InvokeResult();
			try
			{
				var safeMethod = new Spring.Reflection.Dynamic.SafeMethod(this._listenerMethod);
				returnValue.ReturnedValue = safeMethod.Invoke(this._listenerObject, requestObject);
			}
			catch (System.Exception ex)
			{
				returnValue.Exception = ex;
			}
			return returnValue;
		}

		//private string BuildInvocationFailureMessage(string methodName, object[] arguments)
		//{
		//	return string.Concat(new string[]
		//	{
		//		"Failed to invoke target method '",
		//		methodName,
		//		"' with argument types = [",
		//		StringUtils.CollectionToCommaDelimitedString<string>(this.GetArgumentTypes(arguments)),
		//		"], values = [",
		//		StringUtils.CollectionToCommaDelimitedString<object>(arguments),
		//		"]"
		//	});
		//}
		//private System.Collections.Generic.List<string> GetArgumentTypes(object[] arguments)
		//{
		//	System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
		//	if (arguments != null)
		//	{
		//		for (int i = 0; i < arguments.Length; i++)
		//		{
		//			list.Add(arguments[i].GetType().ToString());
		//		}
		//	}
		//	return list;
		//}

		protected virtual void HandleResult(AmqpBasedRoute replyRoute, List<object> responses)
		{
			if (replyRoute != null)
			{
				this._queueClient.BatchPublish(exchangeName: replyRoute.ExchangeName, routingKey: replyRoute.RoutingKey, contentList: responses);
			}
		}
	}
}