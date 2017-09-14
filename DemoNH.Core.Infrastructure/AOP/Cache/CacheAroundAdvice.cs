//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Cache = SharedCache.WinServiceCommon.Provider.Cache.IndexusDistributionCache;
//using AopAlliance.Intercept;
//using System.Reflection;
//using Spring.Threading;

//namespace DemoNH.Core.Infrastructure.AOP.Cache
//{
//	public class CacheAroundAdvice : IMethodInterceptor
//	{
//		IThreadStorage CacheStorage { get; set; }

//		#region Métodos de Suporte

//		private string ClearKey(string text)
//		{
//			return text.Replace("[", string.Empty)
//			.Replace("]", string.Empty)
//			.Replace("\"", string.Empty)
//			.Replace("\'", string.Empty)
//			;
//		}

//		private T Serialize<T>(object objectToSerialize, CacheSerializationType serializationType)
//		{
//			object returnValue = null;
//			switch (serializationType)
//			{
//				case CacheSerializationType.Binary:
//					{
//						System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
//						using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
//						{
//							binaryFormatter.Serialize(stream, objectToSerialize);
//							System.IO.StreamReader streamReader = new System.IO.StreamReader(stream);
//							returnValue = streamReader.ReadToEnd();
//							streamReader.Close();
//							stream.Close();
//						}
//						break;
//					}
//				case CacheSerializationType.Json:
//					{
//						returnValue = JsonHelper.Serialize(objectToSerialize);
//						break;
//					}
//				case CacheSerializationType.Soap:
//					{
//						System.Runtime.Serialization.Formatters.Soap.SoapFormatter soapFormatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
//						using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
//						{
//							soapFormatter.Serialize(stream, objectToSerialize);
//							System.IO.StreamReader streamReader = new System.IO.StreamReader(stream);
//							returnValue = streamReader.ReadToEnd();
//							streamReader.Close();
//							//stream.Close();
//						}
//						break;
//					}
//			}
//			return (T)returnValue;
//		}

//		protected virtual string GetCacheKey(IMethodInvocation invocation, bool includeParameterValues)
//		{
//			string returnValue = null;
//			using (System.IO.StringWriter cacheKeyWriter = new System.IO.StringWriter())
//			{
//				Type targetType = invocation.Target.GetType();
//				string methodName = invocation.Method.ToString();
//				string assemblyName = targetType.Assembly.ManifestModule.ScopeName;
//				string typeName = targetType.Name;
//				string typeNamespace = targetType.Namespace;

//				cacheKeyWriter.Write("{0} | {1}.{2} | {3}", assemblyName, typeNamespace, typeName, methodName);
//				cacheKeyWriter.Write("(");
//				if (includeParameterValues)
//				{
//					ParameterInfo[] parameters = invocation.Method.GetParameters();
//					for (int i = 0; i < parameters.Length; i++)
//					{
//						string parameterName = parameters[i].Name;
//						object parameterValue = invocation.Arguments[i];

//						object parameterInfo = new { Name = parameterName, Value = parameterValue };

//						string itemValue = this.Serialize<string>(parameterValue, CacheSerializationType.Json);
//						cacheKeyWriter.Write(parameterName + ":" + itemValue);
//						if (i + 1 < parameters.Length)
//							cacheKeyWriter.Write(", ");
//					}
//				}
//				cacheKeyWriter.Write(")");
//				returnValue = this.ClearKey(cacheKeyWriter.ToString());
//			}
//			return returnValue;
//		}

//		/// <summary>
//		/// Obtém a chave de cache para o método/argumentos informados
//		/// </summary>
//		/// <param name="input"></param>
//		/// <param name="cacheableAttribute"></param>
//		/// <returns></returns>
//		private string GetCacheKey(IMethodInvocation input, CacheableAttribute cacheableAttribute)
//		{
//			string returnValue = null;
//			if ((cacheableAttribute != null) && (cacheableAttribute.NameStrategy == CacheNameStrategy.UserDefined))
//			{
//				returnValue = cacheableAttribute.CacheName;
//			}
//			else
//			{
//				returnValue = this.GetCacheKey(input, true);
//			}
//			return returnValue;
//		}

//		/// <summary>
//		/// Obtém as definições do cache para o método em questão
//		/// </summary>
//		/// <param name="methodInfo">Método</param>
//		/// <returns>Retorna um CacheableAttribute contento as definições de cache para o método. Caso não haja definição no método, retorna nulo.</returns>
//		private CacheableAttribute GetCacheableAttribute(System.Reflection.MethodInfo methodInfo)
//		{
//			CacheableAttribute returnValue = null;
//			object value = methodInfo.GetCustomAttributes(typeof(CacheableAttribute), true).FirstOrDefault();
//			if (value != null)
//				returnValue = (CacheableAttribute)value;
//			return returnValue;
//		}

//		#endregion

//		public virtual object Invoke(AopAlliance.Intercept.IMethodInvocation invocation)
//		{
//			object returnValue = null;
//			CacheableAttribute cacheableAttribute = this.GetCacheableAttribute(invocation.Method);
//			if (cacheableAttribute != null)
//			{
//				//this.TraceLogger.Trace("Método " + invocation.Method.ToString() + " Eleito para utilização de log");
//				string cacheKey = this.GetCacheKey(invocation, cacheableAttribute);
//				//this.TraceLogger.Trace("CacheKey:" + cacheKey);
//				object cachedValue = this.CacheStorage.GetData(cacheKey);
//				if (cachedValue != null)
//				{
//					//this.TraceLogger.Trace("Método " + invocation.Method.ToString() + " Retornando resultado do cache");
//					returnValue = cachedValue;
//				}
//				else
//				{
//					//this.TraceLogger.Trace("Método " + invocation.Method.ToString() + " Cache não possui regristro");

//					Exception currentException = null;
//					try
//					{
//						returnValue = invocation.Proceed();
//					}
//					catch (Exception ex)
//					{
//						currentException = ex;
//						throw;
//					}
//					if (currentException == null)
//					{
//						if (returnValue != null)
//						{
//							//this.TraceLogger.Trace("Método " + invocation.Method.ToString() + " Não há dado no cache para este objeto");
//							this.CacheStorage.SetData(cacheKey, returnValue);
//							//this.TraceLogger.Trace("Método " + invocation.Method.ToString() + " Adicionando no cache");
//						}
//					}
//				}
//			}
//			else //Neste caso o método não é cacheado
//			{
//				returnValue = invocation.Proceed();
//			}
//			return returnValue;
//		}

//	}
//}