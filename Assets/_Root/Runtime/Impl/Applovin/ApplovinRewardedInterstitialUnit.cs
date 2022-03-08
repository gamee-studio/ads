using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class ApplovinRewardedInterstitialUnit : RewardedInterstitialAdUnit
    {
        public ApplovinRewardedInterstitialUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}