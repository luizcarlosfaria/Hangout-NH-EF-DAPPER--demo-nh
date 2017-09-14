using System.Collections.Generic;
using DemoNH.Core.Infrastructure.Cache.Interfaces;
using NHibernate.Mapping;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;

namespace DemoNH.Core.Infrastructure.Cache
{
	public class CacheProvider : ICacheProvider
	{
		private readonly IRedisClient _redisClient;

		public CacheProvider(IRedisClient redisClient)
		{
			_redisClient = redisClient;
		}

		public T GetValue<T>(string key)
		{
			IRedisTypedClient<T> typedRedisClient = _redisClient.As<T>();
			return typedRedisClient.GetValue(key);
		}

		public void SetEntry<T>(string key, T value)
		{
			IRedisTypedClient<T> typedRedisClient = _redisClient.As<T>();
			typedRedisClient.SetEntry(key, value);
		}

		public void SetEntry<T>(string key, T value, TimeSpan expireIn)
		{
			IRedisTypedClient<T> typedRedisClient = _redisClient.As<T>();
			typedRedisClient.SetEntry(key, value, expireIn);
		}

		public bool ExpireEntryIn(string key, TimeSpan expireIn)
		{
			return _redisClient.ExpireEntryIn(key, expireIn);
		}

		public void Increment(string key, uint value = 1)
		{
			_redisClient.Increment(key, value);
		}

		public void Decrement(string key, uint value = 1)
		{
			_redisClient.Decrement(key, value);
		}

		public bool ContainsKey(string key)
		{
			return _redisClient.ContainsKey(key);
		}

		public bool DeleteByPrefix(string prefix)
		{
			List<string> matchingKeys = _redisClient.SearchKeys(prefix + "*");

			return _redisClient.RemoveEntry(matchingKeys.ToArray());
		}

		public void Dispose()
		{
			_redisClient.Dispose();
		}
	}
}