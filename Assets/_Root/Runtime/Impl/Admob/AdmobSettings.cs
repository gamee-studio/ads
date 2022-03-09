using System;
using System.Collections.Generic;
using UnityEngine;

namespace Snorlax.Ads
{
    [Serializable]
    public class AdmobSettings
    {
        [SerializeField] private bool enable;
        [SerializeField] private List<string> devicesTest;
        [SerializeField] private AdmobBannerUnit bannerAdUnit;
        [SerializeField] private AdmobIntestitialUnit interstitialAdUnit;
        [SerializeField] private AdmobRewardedUnit rewardedAdUnit;
        [SerializeField] private AdmobRewardedInterstitialUnit rewardedInterstitialAdUnit;
        [SerializeField] private AdmobAppOpenUnit appOpenAdUnit;
        [SerializeField] private bool enableTestMode;
        [SerializeField] private bool useAdaptiveBanner;

#if UNITY_EDITOR
        private List<Network> _mediationNetworks = new List<Network>();
        public Network importingNetwork;
#endif

        public bool Enable => enable;
        public List<string> DevicesTest => devicesTest;
        public bool EnableTestMode => enableTestMode;
        public bool UseAdaptiveBanner => useAdaptiveBanner;
        public AdmobBannerUnit BannerAdUnit => bannerAdUnit;
        public AdmobIntestitialUnit InterstitialAdUnit => interstitialAdUnit;
        public AdmobRewardedUnit RewardedAdUnit => rewardedAdUnit;
        public AdmobRewardedInterstitialUnit RewardedInterstitialAdUnit => rewardedInterstitialAdUnit;
        public AdmobAppOpenUnit AppOpenAdUnit => appOpenAdUnit;

#if UNITY_EDITOR
        public List<Network> MediationNetworks { get => _mediationNetworks; set => _mediationNetworks = value; }
#endif
    }
}