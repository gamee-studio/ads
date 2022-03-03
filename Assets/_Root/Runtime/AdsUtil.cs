using System;

namespace Snorlax.Ads
{
    public class AdsUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionA"></param>
        /// <param name="versionB"></param>
        /// <returns></returns>
        public static EVersionComparisonResult CompareVersion(string versionA, string versionB)
        {
            var vA = new Version(versionA);
            var vB = new Version(versionB);
            int result = vA.CompareTo(vB);
            if (result >0)
            {
                return EVersionComparisonResult.Greater;
            }
            return result < 0 ? EVersionComparisonResult.Lesser : EVersionComparisonResult.Equal;
        }
    }
}