using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class InterstitialAdUnit : AdUnit
    {
        public InterstitialAdUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}