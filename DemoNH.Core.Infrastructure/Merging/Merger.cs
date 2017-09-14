using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoNH.Core.Infrastructure.Merging
{
	public class Merger<T>
	{
		private Func<T, T, bool> IdentityQuestion;
		private Func<T, T, bool> EqualsQuestion;

		public Merger(Func<T, T, bool> identityQuestion, Func<T, T, bool> equalsQuestion)
		{
			this.IdentityQuestion = identityQuestion;
			this.EqualsQuestion = equalsQuestion;
		}

		public static MergeResult<T> Merge(Func<T, T, bool> identityQuestion, Func<T, T, bool> equalsQuestion, List<T> originalList, List<T> otherList)
		{
			Merger<T> instance = new Merger<T>(identityQuestion, equalsQuestion);
			MergeResult<T> returnValue = instance.Merge(originalList, otherList);
			return returnValue;
		}

		public MergeResult<T> Merge(IList<T> originalList, IList<T> otherList)
		{
			if (originalList != null)
				originalList = originalList.Where(it => it != null).ToList();

			if (otherList != null)
				otherList = otherList.Where(it => it != null).ToList();

			MergeResult<T> result = new MergeResult<T>();
			result.ItemsToDelete = new List<T>();
			result.ItemsToInsert = null;
			result.ItemsToUpdate = new List<ItemToUpdate<T>>();
			if (originalList != null)
			{
				foreach (T originalItem in originalList)
				{
					T otherItem = otherList.Where(it => IdentityQuestion(it, originalItem)).FirstOrDefault();
					if (otherItem == null)
						result.ItemsToDelete.Add(originalItem);
					else if (EqualsQuestion(originalItem, otherItem) == false)
						result.ItemsToUpdate.Add(new ItemToUpdate<T>() { Original = originalItem, Modified = otherItem });
				}
			}
			if (otherList != null)
				result.ItemsToInsert = otherList.Where(it => !originalList.Any(it2 => this.IdentityQuestion(it, it2))).ToList();
			else
				result.ItemsToInsert = new List<T>();
			return result;
		}
	}
}