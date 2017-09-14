//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace DemoNH.Core.Infrastructure.AOP.Cache
//{
//	[AttributeUsage(AttributeTargets.Method)]
//	public class CacheableAttribute : Attribute
//	{
//		public CacheableAttribute()
//		{
//			this.NameStrategy = CacheNameStrategy.Generated;
//		}

//		public string CacheName { get; set; }

//		public CacheNameStrategy NameStrategy { get; set; }

//	}

//	public enum CacheNameStrategy
//	{
//		Generated,
//		UserDefined
//	}
//}