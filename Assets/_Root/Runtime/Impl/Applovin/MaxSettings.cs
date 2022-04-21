using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pancake.Monetization
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

#if UNITY_EDITOR
        public List<MaxNetwork> editorListNetwork = new List<MaxNetwork>();
        public MaxNetwork editorImportingNetwork;
        public List<MaxNetwork> editorImportingListNetwork = new List<MaxNetwork>();
#endif

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