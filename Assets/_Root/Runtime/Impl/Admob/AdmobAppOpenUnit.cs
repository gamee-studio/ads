using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class AdmobAppOpenUnit : AppOpenAdUnit
    {
        public AdmobAppOpenUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}