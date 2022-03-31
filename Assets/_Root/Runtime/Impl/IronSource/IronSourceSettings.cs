using System;
using UnityEngine;

namespace Snorlax.Ads
{
    [Serializable]
    public class IronSourceSettings
    {
        [SerializeField] private bool enable;
        [SerializeField] private AppUnit appKey;
        [SerializeField] private bool useAdaptiveBanner;
        [SerializeField] private IronSourceBannerUnit bannerAdUnit;


#if UNITY_EDITOR
        private System.Collections.Generic.List<Network> _mediationNetworks = new System.Collections.Generic.List<Network>();

        /// <summary>
        /// editor only
        /// </summary>
        public Network importingSdk;

        /// <summary>
        /// editor only
        /// </summary>
        public Network importingMediationNetwork;
#endif

        public bool Enable => enable;

        public IronSourceBannerUnit BannerAdUnit => bannerAdUnit;

        public AppUnit AppKey => appKey;

        public bool UseAdaptiveBanner => useAdaptiveBanner;

#if UNITY_EDITOR
        /// <summary>
        /// editor only
        /// </summary>
        public System.Collections.Generic.List<Network> MediationNetworks { get => _mediationNetworks; set => _mediationNetworks = value; }
#endif
    }
}