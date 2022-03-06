using System;
using UnityEngine;

namespace Snorlax.Ads
{
    [Serializable]
    public class ApplovinSettings
    {
        [SerializeField] private bool enable;

        public bool Enable => enable;
    }
}