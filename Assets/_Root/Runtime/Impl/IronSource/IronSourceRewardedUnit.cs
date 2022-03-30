using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class IronSourceRewardedUnit : AdUnit
    {
        public IronSourceRewardedUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}