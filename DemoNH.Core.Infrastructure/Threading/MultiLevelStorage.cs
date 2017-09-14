using Spring.Threading;
using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.Threading
{
	public class MultiLevelStorage : IThreadStorage
	{
		private List<IThreadStorage> CacheLevels { get; set; }

		private IKeyParser KeyParser { get; set; }

		private string ResolveName(string name)
		{
			string newName = this.KeyParser.GetName(name);
			return newName;
		}

		public void FreeNamedDataSlot(string name)
		{
			if (string.IsNullOrWhiteSpace(name) == false)
			{
				string newName = this.ResolveName(name);
				this.CacheLevels.ForEach(it => it.FreeNamedDataSlot(newName));
			}
		}

		public object GetData(string name)
		{
			object returnValue = null;
			if (string.IsNullOrWhiteSpace(name) == false)
			{
				string newName = this.ResolveName(name);
				for (int cacheIndex = 0; cacheIndex < this.CacheLevels.Count; cacheIndex++)
				{
					IThreadStorage currentCache = this.CacheLevels[cacheIndex];
					returnValue = currentCache.GetData(newName);
					if (returnValue != null)
					{
						if (cacheIndex > 0)
						{
							cacheIndex--;
							for (; cacheIndex >= 0; cacheIndex--)
							{
								this.CacheLevels[cacheIndex].SetData(newName, returnValue);
							}
						}
						break;
					}
				}
			}
			return returnValue;
		}

		public void SetData(string name, object value)
		{
			if (string.IsNullOrWhiteSpace(name) == false)
			{
				string newName = this.ResolveName(name);
				for (int cacheIndex = 0; cacheIndex < this.CacheLevels.Count; cacheIndex++)
				{
					IThreadStorage currentCache = this.CacheLevels[cacheIndex];
					currentCache.SetData(newName, value);
				}
			}
		}
	}
}