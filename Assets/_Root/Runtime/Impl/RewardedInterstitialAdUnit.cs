using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class RewardedInterstitialAdUnit : AdUnit
    {
        public RewardedInterstitialAdUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}