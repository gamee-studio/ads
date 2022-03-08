using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class ApplovinInterstitialUnit : InterstitialAdUnit
    {
        public ApplovinInterstitialUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}