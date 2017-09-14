using DemoNH.Core.Infrastructure.Extensions;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoNH.Core.Infrastructure.Cache.Redis
{
	public class RedisClientForSpring : RedisProviderBase, Spring.Caching.ICache
	{
		public RedisClientForSpring(IRedisClient redisClient, string isolationKey)
			: base(redisClient, isolationKey)
		{
		}

		public void Clear()
		{
			string formattedKey = base.GetKey();
			List<string> keys = this.NativeClient.SearchKeys(formattedKey + "*");
			this.RemoveAll(keys);
		}

		public int Count
		{
			get
			{
				string formattedKey = base.GetKey();
				return this.NativeClient.SearchKeys(formattedKey + "*").Count;
			}
		}

		public object Get(object key)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			string jsonSerialized = this.NativeClient.Get<string>(formattedKey);
			object deserializeObject = null;
			if (jsonSerialized.IsNotNullOrWhiteSpace())
				deserializeObject = JsonConvert.DeserializeObject(jsonSerialized, this.SerializerSettings);
			return deserializeObject;
		}

		public void Insert(object key, object value, TimeSpan timeToLive)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			string jsonSerialized = JsonConvert.SerializeObject(value, this.SerializerSettings);
			this.NativeClient.Set(formattedKey, jsonSerialized, timeToLive);
		}

		public void Insert(object key, object value)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			string jsonSerialized = JsonConvert.SerializeObject(value, this.SerializerSettings);
			this.NativeClient.Set(formattedKey, jsonSerialized);
		}

		public System.Collections.ICollection Keys
		{
			get
			{
				string formattedKey = base.GetKey();
				List<string> keys = this.NativeClient.SearchKeys(formattedKey + "*");
				return keys;
			}
		}

		public void Remove(object key)
		{
			string stringKey = (string)key;
			string formattedKey = base.GetKey(stringKey);
			this.NativeClient.Remove(formattedKey);
		}

		public void RemoveAll(System.Collections.ICollection keys)
		{
			this.NativeClient.RemoveAll(keys.Cast<string>());
		}
	}
}