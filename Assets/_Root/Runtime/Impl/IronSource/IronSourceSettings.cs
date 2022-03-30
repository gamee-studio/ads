using System;
using UnityEngine;

namespace Snorlax.Ads
{
    [Serializable]
    public class IronSourceSettings
    {
        [SerializeField] private bool enable;
        [SerializeField] private AdUnit sdkKey;
        [SerializeField] private bool useAdaptiveBanner;
        [SerializeField] private IronSourceBannerUnit bannerAdUnit;
        [SerializeField] private IronSourceInterstitialUnit interstitialAdUnit;
        [SerializeField] private IronSourceRewardedUnit rewardedAdUnit;


        public bool Enable => enable;

        public IronSourceBannerUnit BannerAdUnit => bannerAdUnit;

        public IronSourceInterstitialUnit InterstitialAdUnit => interstitialAdUnit;

        public IronSourceRewardedUnit RewardedAdUnit => rewardedAdUnit;

        public AdUnit SDKKey => sdkKey;

        public bool UseAdaptiveBanner => useAdaptiveBanner;
    }
}