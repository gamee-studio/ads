using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class ApplovinRewardedUnit : RewardedAdUnit
    {
        public ApplovinRewardedUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}