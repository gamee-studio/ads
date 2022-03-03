using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class AdmobRewardedInterstitialUnit : RewardedInterstitialAdUnit
    {
        public AdmobRewardedInterstitialUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}