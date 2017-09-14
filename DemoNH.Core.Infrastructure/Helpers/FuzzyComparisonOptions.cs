using FuzzyString;
using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.Helpers
{
	public static class FuzzyComparisonOptions
	{
		public static List<FuzzyStringComparisonOptions> GetOptions()
		{
			return new List<FuzzyStringComparisonOptions>
						{
							FuzzyStringComparisonOptions.UseJaccardDistance,
							FuzzyStringComparisonOptions.UseLongestCommonSubsequence,
							FuzzyStringComparisonOptions.UseOverlapCoefficient,
							FuzzyStringComparisonOptions.UseRatcliffObershelpSimilarity,
							FuzzyStringComparisonOptions.UseSorensenDiceDistance
						};
		}
	}
}