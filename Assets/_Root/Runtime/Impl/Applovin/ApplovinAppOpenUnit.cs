using System;
using UnityEngine;

namespace Pancake.Monetization
{
    [Serializable]
    public class ApplovinAppOpenUnit : AppOpenAdUnit
    {
        public ScreenOrientation orientation = ScreenOrientation.Portrait;
        public bool appOpen = true;
        public bool applicationFocus = true;

        public ApplovinAppOpenUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }

        public ApplovinAppOpenUnit()
            : base("", "")
        {
            orientation = ScreenOrientation.Portrait;
        }
    }
}