using System;

namespace DemoNH.Core.Infrastructure.Cache.Interfaces
{
	public interface ICacheProvider : IDisposable
	{
		T GetValue<T>(string key);

		void SetEntry<T>(string key, T value);

		void SetEntry<T>(string key, T value, TimeSpan expireIn);

		bool ExpireEntryIn(string key, TimeSpan expireIn);

		void Increment(string key, uint value = 1);

		void Decrement(string key, uint value = 1);

		bool ContainsKey(string key);

		bool DeleteByPrefix(string prefix);
	}
}