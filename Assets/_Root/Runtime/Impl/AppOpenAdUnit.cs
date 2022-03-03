using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class AppOpenAdUnit : AdUnit
    {
        public AppOpenAdUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}