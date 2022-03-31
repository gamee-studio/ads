using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class AppUnit : AdUnit
    {
        public AppUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }

        public AppUnit()
            : base("", "")
        {
        }
    }
}