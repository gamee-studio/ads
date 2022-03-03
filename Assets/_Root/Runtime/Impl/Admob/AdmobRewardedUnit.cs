using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class AdmobRewardedUnit : RewardedAdUnit
    {
        public AdmobRewardedUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}