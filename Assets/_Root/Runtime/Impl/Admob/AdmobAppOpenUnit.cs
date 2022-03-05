using System;
using UnityEngine;

namespace Snorlax.Ads
{
    [Serializable]
    public class AdmobAppOpenUnit : AppOpenAdUnit
    {
        public ScreenOrientation orientation;

        public AdmobAppOpenUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }
    }
}