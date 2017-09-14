﻿using DemoNH.Core.Infrastructure.Log;
using DemoNH.Core.Infrastructure.Serialization.Interfaces;
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DemoNH.Core.Infrastructure.MessageQueuing
{
	public class RabbitMQConsumer : IQueueConsumer
	{
		public RabbitMQConnectionPool ConnectionPool { get; private set; }

		public string QueueName { get; private set; }

		public ISerializer Serializer { get; private set; }

		public ILogger Logger { private get; set; }

		public Type ExpectedType { get; private set; }

		public IMessageProcessingWorker MessageProcessingWorker { get; set; }

		public IMessageRejectionHandler MessageRejectionHandler { get; set; }

		public ConsumerCountManager ConsumerCountManager { get { return this._consumerCountManager; } }

		private readonly CancellationTokenSource _cancellationTokenSource;

		private volatile int _scalingAmount;
		private volatile bool _isStopped;
		private volatile int _consumerWorkersCount;
		private volatile ConsumerCountManager _consumerCountManager;

		public RabbitMQConsumer(RabbitMQConnectionPool connectionPool, string queueName, ISerializer serializer, ILogger logger, Type expectedType, IMessageProcessingWorker messageProcessingWorker, ConsumerCountManager consumerCountManager, IMessageRejectionHandler messageRejectionHandler)
		{
			//Set using constructor parameters
			this.ConnectionPool = connectionPool;
			this.QueueName = queueName;
			this.Serializer = serializer;
			this.Logger = logger;
			this.ExpectedType = expectedType;
			this.MessageProcessingWorker = messageProcessingWorker;
			this._consumerCountManager = consumerCountManager;
			this.MessageRejectionHandler = messageRejectionHandler;

			//Set using default values
			this._consumerWorkersCount = 0;
			this._cancellationTokenSource = new CancellationTokenSource();
			this._isStopped = true;
		}

		public void Start()
		{
			this._isStopped = false;
			CancellationToken token = this._cancellationTokenSource.Token;
			Task.Factory.StartNew(() => this.ManageConsumersLoop(token), token);
		}

		public void Stop()
		{
			this._isStopped = true;
			//lock (this._scalingAmountSyncLock)
			{
				this._scalingAmount = this._consumerWorkersCount * -1;
			}
			this._cancellationTokenSource.Cancel();
		}

		public void SetQueueName(string queueName)
		{
			if (queueName != null)
			{
				this.QueueName = queueName;
			}
		}

		public uint GetMessageCount()
		{
			using (IModel model = this.ConnectionPool.GetConnection().CreateModel())
			{
				return GetMessageCount(model);
			}
		}

		public uint GetConsumerCount()
		{
			using (IModel model = this.ConnectionPool.GetConnection().CreateModel())
			{
				return GetConsumerCount(model);
			}
		}

		protected virtual void ManageConsumersLoop(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				if (!this._isStopped)
				{
					QueueInfo queueInfo = this.CreateQueueInfo();
					this._scalingAmount = this._consumerCountManager.GetScalingAmount(queueInfo, this._consumerWorkersCount);
					int scalingAmount = this._scalingAmount;
					for (var i = 1; i <= scalingAmount; i++)
					{
						this._scalingAmount--;
						this._consumerWorkersCount++;

						Task.Factory.StartNew(() =>
						{
							try
							{
								using (IQueueConsumerWorker consumerWorker = this.CreateNewConsumerWorker(token))
								{
									consumerWorker.DoConsume();
								}
							}
							catch (Exception exception)
							{
								this.Logger.Error("DemoNH.Core.Infrastructure.MessageQueuing.RabbitMQConsumer", exception.ToString(),
									"QueueName", this.QueueName);
							}
							finally
							{
								this._consumerWorkersCount--;
								this._scalingAmount++;
							}
						}
						, token);
					}
				}

				Thread.Sleep(this._consumerCountManager.AutoscaleFrequency);
			}
		}

		#region Consumer CallBack Methods

		private RabbitMQConsumerWorker CreateNewConsumerWorker(CancellationToken parentToken)
		{
			var newConsumerWorker = new RabbitMQConsumerWorker(
				connection: this.ConnectionPool.GetConnection(),
				queueName: this.QueueName,
				messageProcessingWorker: this.MessageProcessingWorker,
				messageRejectionHandler: this.MessageRejectionHandler,
				serializer: this.Serializer,
				scaleCallbackFunc: this.GetScalingAmount,
				expectedType: this.ExpectedType,
				parentToken: parentToken
			);

			return newConsumerWorker;
		}

		private int GetScalingAmount()
		{
			return this._scalingAmount;
		}

		#endregion Consumer CallBack Methods

		public void Dispose()
		{
			this.Stop();
		}

		#region Private Methods

		private QueueInfo CreateQueueInfo()
		{
			QueueInfo queueInfo = null;
			using (IModel model = this.ConnectionPool.GetConnection().CreateModel())
			{
				queueInfo = new QueueInfo()
				{
					QueueName = this.QueueName,
					ConsumerCount = this.GetConsumerCount(model),
					MessageCount = this.GetMessageCount(model)
				};
			}
			return queueInfo;
		}

		private uint GetMessageCount(IModel model)
		{
			QueueDeclareOk queueDeclareOk = model.QueueDeclarePassive(this.QueueName);
			return queueDeclareOk.MessageCount;
		}

		private uint GetConsumerCount(IModel model)
		{
			QueueDeclareOk queueDeclareOk = model.QueueDeclarePassive(this.QueueName);
			return queueDeclareOk.ConsumerCount;
		}

		#endregion Private Methods
	}
}