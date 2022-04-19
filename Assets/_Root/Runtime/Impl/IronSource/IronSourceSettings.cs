using System;
using UnityEngine;

namespace Pancake.Monetization
{
    [Serializable]
    public class IronSourceSettings
    {
        [SerializeField] private bool enable;
        [SerializeField] private AppUnit appKey;
        [SerializeField] private bool useAdaptiveBanner;
        [SerializeField] private IronSourceBannerUnit bannerAdUnit;


#if UNITY_EDITOR
        private System.Collections.Generic.List<AdapterMediationIronSource> _mediationNetworks = new System.Collections.Generic.List<AdapterMediationIronSource>();

        /// <summary>
        /// editor only
        /// </summary>
        public Network importingSdk;

        /// <summary>
        /// editor only
        /// </summary>
        public AdapterMediationIronSource importingMediationNetwork;
#endif

        public bool Enable => enable;

        public IronSourceBannerUnit BannerAdUnit => bannerAdUnit;

        public AppUnit AppKey => appKey;

        public bool UseAdaptiveBanner => useAdaptiveBanner;

#if UNITY_EDITOR
        /// <summary>
        /// editor only
        /// </summary>
        public System.Collections.Generic.List<AdapterMediationIronSource> MediationNetworks { get => _mediationNetworks; set => _mediationNetworks = value; }
#endif
    }
}