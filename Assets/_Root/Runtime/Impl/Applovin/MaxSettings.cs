using System;
using UnityEngine;

namespace Snorlax.Ads
{
    [Serializable]
    // avoid config name with AppLovinSettings
    public class MaxSettings
    {
        [SerializeField] private bool enable;
        [SerializeField] private string sdkKey;
        [SerializeField] private ApplovinBannerUnit bannerAdUnit;
        [SerializeField] private ApplovinInterstitialUnit interstitialAdUnit;
        [SerializeField] private ApplovinRewardedUnit rewardedAdUnit;
        [SerializeField] private ApplovinRewardedInterstitialUnit rewardedInterstitialAdUnit;
        [SerializeField] private bool enableAgeRestrictedUser;
        [SerializeField] private bool enableRequestAdAfterHidden = true;
        [SerializeField] private bool enableMaxAdReview;

        public bool Enable => enable;
        public string SdkKey => sdkKey;
        public ApplovinBannerUnit BannerAdUnit => bannerAdUnit;
        public ApplovinInterstitialUnit InterstitialAdUnit => interstitialAdUnit;
        public ApplovinRewardedUnit RewardedAdUnit => rewardedAdUnit;

        public ApplovinRewardedInterstitialUnit RewardedInterstitialAdUnit => rewardedInterstitialAdUnit;

        public bool EnableAgeRestrictedUser => enableAgeRestrictedUser;

        public bool EnableRequestAdAfterHidden => enableRequestAdAfterHidden;

        public bool EnableMaxAdReview => enableMaxAdReview;
    }
}