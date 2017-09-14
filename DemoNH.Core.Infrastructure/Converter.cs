using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DemoNH.Core.Infrastructure
{
	/// <summary>
	/// Define um objeto de conversão
	/// </summary>
	/// <typeparam name="T">Tipo do objeto a ser convertido</typeparam>
	public class Converter<T> : IConverter
	 where T : class
	{
		/// <summary>
		/// Define um Fetcher
		/// </summary>
		public class Fetcher
		{
			/// <summary>
			/// Expressão Lambda que define como obter o valor do método
			/// </summary>
			public LambdaExpression PathExpression { get; set; }

			private System.Reflection.PropertyInfo propertyInfo;

			internal System.Reflection.PropertyInfo GetPropertyInfo()
			{
				if (this.propertyInfo == null)
				{
					this.propertyInfo = (System.Reflection.PropertyInfo)this.GetMemberExpression().Member;
				}
				return this.propertyInfo;
			}

			private MemberExpression memberExpression;

			internal MemberExpression GetMemberExpression()
			{
				if (this.memberExpression == null)
				{
					if (this.PathExpression.Body is UnaryExpression)
					{
						UnaryExpression unaryExpression = ((UnaryExpression)this.PathExpression.Body);
						this.memberExpression = (MemberExpression)unaryExpression.Operand;
					}
					else if (this.PathExpression.Body is MemberExpression)
						this.memberExpression = (MemberExpression)this.PathExpression.Body;
				}
				return this.memberExpression;
			}

			private Delegate compiledExpression;

			internal Delegate GetCompiledExpression()
			{
				if (compiledExpression == null)
					compiledExpression = this.PathExpression.Compile();
				return compiledExpression;
			}

			internal object GetValue(params object[] args)
			{
				return GetCompiledExpression().DynamicInvoke(args);
			}

			/// <summary>
			/// Func que define como converter
			/// </summary>
			public Func<Object, Object> ConvertExpression { get; set; }
		}

		/// <summary>
		/// Lista de Fetchers usada para determinar quais valores extrair das classes
		/// </summary>
		public List<Fetcher> FetchPathList { get; private set; }

		#region Static Build

		public static Converter<T> Create()
		{
			Converter<T> converterInstance = new Converter<T>();
			return converterInstance;
		}

		#endregion Static Build

		#region Constructors

		private Converter()
		{
			this.FetchPathList = new List<Fetcher>();
		}

		#endregion Constructors

		#region Fetch

		public Converter<T> Fetch(Expression<Func<T, object>> path)
		{
			Fetcher newFetcher = new Fetcher();
			newFetcher.PathExpression = path;
			newFetcher.ConvertExpression = delegate(object arg) { return arg; };

			this.FetchPathList.Add(newFetcher);
			return this;
		}

		public Converter<T> Fetch(Expression<Func<T, object>> path, IConverter converter)
		{
			Fetcher newFetcher = new Fetcher();
			newFetcher.PathExpression = path;
			newFetcher.ConvertExpression = delegate(object arg) { return converter.Convert(arg); };
			this.FetchPathList.Add(newFetcher);
			return this;
		}

		#endregion Fetch

		#region Convert

		public List<T> Convert(IList<T> listOfInstancesToConvert)
		{
			List<T> returnValue = new List<T>();
			foreach (T instanceToConvert in listOfInstancesToConvert)
				returnValue.Add(this.Convert(instanceToConvert));
			return returnValue;
		}

		public T Convert(T instanceToConvert)
		{
			T newInstance = default(T);
			if (instanceToConvert != default(T))
			{
				newInstance = Activator.CreateInstance<T>();
				foreach (Fetcher newFetcher in this.FetchPathList)
				{
					object currentPropertyValue = newFetcher.GetValue(instanceToConvert);
					if (currentPropertyValue != null)
					{
						System.Reflection.PropertyInfo propertyInfo = newFetcher.GetPropertyInfo();
						System.Collections.IList currentPropertyList = currentPropertyValue as System.Collections.IList;
						if (currentPropertyList != null)
						{
							Type genericType = currentPropertyValue.GetType().GetGenericArguments()[0];
							Type GenericListType = typeof(List<>).MakeGenericType(genericType);
							System.Collections.IList newGenericList = Activator.CreateInstance(GenericListType) as System.Collections.IList;

							//Executando listas sem Converter
							foreach (object itemOfList in currentPropertyList)
							{
								object convertedItemOfList = newFetcher.ConvertExpression(itemOfList);
								newGenericList.Add(convertedItemOfList);
							}
							propertyInfo.SetValue(newInstance, newGenericList, null);
						}
						//else if ((currentPropertyValue.GetType().IsValueType) || (currentPropertyValue is string))
						else
						{
							object convertedPropertValue = newFetcher.ConvertExpression(currentPropertyValue);
							propertyInfo.SetValue(newInstance, convertedPropertValue, null);
						}
					}
				}
			}
			return newInstance;
		}

		#endregion Convert

		object IConverter.Convert(object instanceToConvert)
		{
			return this.Convert((T)instanceToConvert);
		}
	}

	/// <summary>
	/// Define um objeto conversor
	/// </summary>
	public interface IConverter
	{
		object Convert(object instanceToConvert);
	}
}