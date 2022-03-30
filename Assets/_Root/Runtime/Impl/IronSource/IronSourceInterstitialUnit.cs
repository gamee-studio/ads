using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class IronSourceInterstitialUnit : InterstitialAdUnit
    {
        public IronSourceInterstitialUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}