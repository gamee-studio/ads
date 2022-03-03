using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class AdmobIntestitialUnit : InterstitialAdUnit
    {
        public AdmobIntestitialUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}