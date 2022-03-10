using System;

#pragma warning disable CS0414
// ReSharper disable AccessToStaticMemberViaDerivedType
// ReSharper disable NotAccessedField.Local
namespace Snorlax.Ads
{
    public class ApplovinAdClient : AdClient
    {
        private const string NO_SDK_MESSAGE = "SDK missing. Please import applovin max plugin.";
        private ApplovinBannerLoader _banner;
        private ApplovinInterstitialLoader _interstitial;
        private ApplovinRewardedLoader _rewarded;
        private ApplovinRewardedInterstitialLoader _rewardedInterstitial;
        private static ApplovinAdClient client;
        private bool _isBannerDestroyed;
        public static ApplovinAdClient Instance => client ??= new ApplovinAdClient();

#if PANCAKE_MAX_ENABLE
        public event Action OnBannerAdLoaded;
        public event Action OnBannerAdFaildToLoad;
        public event Action OnBannerAdClicked;
        public event Action OnBannerAdExpanded;
        public event Action OnBannerAdCollapsed;
        public event Action<MaxSdkBase.AdInfo> OnBannerAdRevenuePaid;
        public event Action OnInterstitialAdClicked;
        public event Action OnInterstitialAdLoaded;
        public event Action OnInterstitialAdFaildToLoad;
        public event Action OnInterstitialAdFaildToDisplay;
        public event Action OnInterstitialAdDisplay;
        public event Action OnInterstitialAdHidden;
        public event Action<MaxSdkBase.AdInfo> OnInterstitialAdRevenuePaid;
        public event Action OnRewardedAdClicked;
        public event Action OnRewardedAdLoaded;
        public event Action OnRewardedAdFaildToLoad;
        public event Action OnRewardedAdFaildToDisplay;
        public event Action OnRewardedAdDisplay;
        public event Action OnRewardedAdHidden;
        public event Action<MaxSdkBase.AdInfo> OnRewardedAdRevenuePaid;
        public event Action<MaxSdkBase.Reward> OnRewardedAdReceivedReward;
        public event Action OnRewardedInterstitialAdClicked;
        public event Action OnRewardedInterstitialAdLoaded;
        public event Action OnRewardedInterstitialAdFaildToLoad;
        public event Action OnRewardedInterstitialAdFaildToDisplay;
        public event Action OnRewardedInterstitialAdDisplay;
        public event Action OnRewardedInterstitialAdHidden;
        public event Action<MaxSdkBase.AdInfo> OnRewardedInterstitialAdRevenuePaid;
        public event Action<MaxSdkBase.Reward> OnRewardedInterstitialAdReceivedReward;
#endif

        public override EAdNetwork Network => EAdNetwork.Applovin;
        public override bool IsBannerAdSupported => true;
        public override bool IsInsterstitialAdSupport => true;
        public override bool IsRewardedAdSupport => true;
        public override bool IsRewardedInterstitialAdSupport => true;
        public override bool IsAppOpenAdSupport => false;

        public override bool IsSdkAvaiable
        {
            get
            {
#if PANCAKE_MAX_ENABLE
                return true;
#else
                return false;
#endif
            }
        }

#if PANCAKE_MAX_ENABLE
        public override float GetAdaptiveBannerHeight => _banner.GetAdaptiveBannerHeight();
#endif

        protected override string NoSdkMessage => NO_SDK_MESSAGE;

        #region internal

#if PANCAKE_MAX_ENABLE
        internal void InvokeBannerAdLoaded() { OnBannerAdLoaded?.Invoke(); }
        internal void InvokeBannerAdFaildToLoad() { OnBannerAdFaildToLoad?.Invoke(); }
        internal void InvokeBannerAdClicked() { OnBannerAdClicked?.Invoke(); }
        internal void InvokeBannerAdExpanded() { OnBannerAdExpanded?.Invoke(); }
        internal void InvokeBannerAdCollapsed() { OnBannerAdCollapsed?.Invoke(); }
        internal void InvokeBannerAdRevenuePaid(MaxSdkBase.AdInfo info) { OnBannerAdRevenuePaid?.Invoke(info); }
        internal void InvokeInterstitialAdLoaded() { OnInterstitialAdLoaded?.Invoke(); }
        internal void InvokeInterstitialAdFaildToLoad() { OnInterstitialAdFaildToLoad?.Invoke(); }
        internal void InvokeInterstitialAdFaildToDisplay() { OnInterstitialAdFaildToDisplay?.Invoke(); }
        internal void InvokeInterstitialAdClicked() { OnInterstitialAdClicked?.Invoke(); }
        internal void InvokeInterstitialAdDisplay() { OnInterstitialAdDisplay?.Invoke(); }
        internal void InvokeInterstitialAdHidden() { OnInterstitialAdHidden?.Invoke(); }
        internal void InvokeInterstitialAdRevenuePaid(MaxSdkBase.AdInfo info) { OnInterstitialAdRevenuePaid?.Invoke(info); }
        internal void InvokeRewardedAdLoaded() { OnRewardedAdLoaded?.Invoke(); }
        internal void InvokeRewardedAdFaildToLoad() { OnRewardedAdFaildToLoad?.Invoke(); }
        internal void InvokeRewardedAdFaildToDisplay() { OnRewardedAdFaildToDisplay?.Invoke(); }
        internal void InvokeRewardedAdClicked() { OnRewardedAdClicked?.Invoke(); }
        internal void InvokeRewardedAdDisplay() { OnRewardedAdDisplay?.Invoke(); }
        internal void InvokeRewardedAdHidden() { OnRewardedAdHidden?.Invoke(); }
        internal void InvokeRewardedAdRevenuePaid(MaxSdkBase.AdInfo info) { OnRewardedAdRevenuePaid?.Invoke(info); }
        internal void InvokeRewardedAdReceivedReward(MaxSdkBase.Reward reward) { OnRewardedAdReceivedReward?.Invoke(reward); }
        internal void InvokeRewardedInterstitialAdLoaded() { OnRewardedInterstitialAdLoaded?.Invoke(); }
        internal void InvokeRewardedInterstitialAdFaildToLoad() { OnRewardedInterstitialAdFaildToLoad?.Invoke(); }
        internal void InvokeRewardedInterstitialAdFaildToDisplay() { OnRewardedInterstitialAdFaildToDisplay?.Invoke(); }
        internal void InvokeRewardedInterstitialAdClicked() { OnRewardedInterstitialAdClicked?.Invoke(); }
        internal void InvokeRewardedInterstitialAdDisplay() { OnRewardedInterstitialAdDisplay?.Invoke(); }
        internal void InvokeRewardedInterstitialAdHidden() { OnRewardedInterstitialAdHidden?.Invoke(); }
        internal void InvokeRewardedInterstitialAdRevenuePaid(MaxSdkBase.AdInfo info) { OnRewardedInterstitialAdRevenuePaid?.Invoke(info); }
        internal void InvokeRewardedInterstitialAdReceivedReward(MaxSdkBase.Reward reward) { OnRewardedInterstitialAdReceivedReward?.Invoke(reward); }
#endif

        #endregion

        protected override void InternalInit()
        {
#if PANCAKE_MAX_ENABLE
            MaxSdk.SetSdkKey(Settings.MaxSettings.SdkKey);
            if (Settings.AdSettings.EnableGDPR) MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitializedEvent;

            MaxSdk.InitializeSdk();
            MaxSdk.SetIsAgeRestrictedUser(Settings.MaxSettings.EnableAgeRestrictedUser);
#endif

            _banner = new ApplovinBannerLoader(this);
            _interstitial = new ApplovinInterstitialLoader(this);
            _rewarded = new ApplovinRewardedLoader(this);
            _rewardedInterstitial = new ApplovinRewardedInterstitialLoader(this);

            LoadInterstitialAd();
            LoadRewardedAd();
            LoadRewardedInterstitialAd();
            isInitialized = true;
            _isBannerDestroyed = false;
        }

#if PANCAKE_MAX_ENABLE
        private void OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration configuration)
        {
            if (configuration.ConsentDialogState == MaxSdkBase.ConsentDialogState.Applies)
            {
                ShowConsentForm();
            }
            else if (configuration.ConsentDialogState == MaxSdkBase.ConsentDialogState.DoesNotApply)
            {
                // No need to show consent dialog, proceed with initialization
            }
            else
            {
                // Consent dialog state is unknown. Proceed with initialization, but check if the consent
                // dialog should be shown on the next application initialization
            }
        }
#endif

        protected override void InternalShowBannerAd()
        {
#if PANCAKE_MAX_ENABLE
            if (string.IsNullOrEmpty(Settings.MaxSettings.BannerAdUnit.Id)) return;
            if (_isBannerDestroyed)
            {
                MaxSdk.CreateBanner(Settings.MaxSettings.BannerAdUnit.Id, Settings.MaxSettings.BannerAdUnit.ConvertPosition());
                _isBannerDestroyed = false;
            }

            MaxSdk.ShowBanner(Settings.MaxSettings.BannerAdUnit.Id);
#endif
        }

        protected override void InternalHideBannerAd()
        {
#if PANCAKE_MAX_ENABLE
            if (string.IsNullOrEmpty(Settings.MaxSettings.BannerAdUnit.Id)) return;
            MaxSdk.HideBanner(Settings.MaxSettings.BannerAdUnit.Id);
#endif
        }

        protected override void InternalDestroyBannerAd()
        {
#if PANCAKE_MAX_ENABLE
            if (string.IsNullOrEmpty(Settings.MaxSettings.BannerAdUnit.Id)) return;
            _isBannerDestroyed = true;
            MaxSdk.DestroyBanner(Settings.MaxSettings.BannerAdUnit.Id);
#endif
        }

        protected override void InternalLoadInterstitialAd()
        {
#if PANCAKE_MAX_ENABLE
            if (string.IsNullOrEmpty(Settings.MaxSettings.InterstitialAdUnit.Id)) return;
            MaxSdk.LoadInterstitial(Settings.MaxSettings.InterstitialAdUnit.Id);
#endif
        }

        protected override void InternalShowInterstitialAd()
        {
#if PANCAKE_MAX_ENABLE
            if (string.IsNullOrEmpty(Settings.MaxSettings.InterstitialAdUnit.Id)) return;
            MaxSdk.ShowInterstitial(Settings.MaxSettings.InterstitialAdUnit.Id);
#endif
        }

        protected override bool InternalIsInterstitialAdReady()
        {
#if PANCAKE_MAX_ENABLE
            if (string.IsNullOrEmpty(Settings.MaxSettings.InterstitialAdUnit.Id)) return false;
            return MaxSdk.IsInterstitialReady(Settings.MaxSettings.InterstitialAdUnit.Id);
#else
            return false;
#endif
        }

        protected override void InternalLoadRewardedAd()
        {
#if PANCAKE_MAX_ENABLE
            if (string.IsNullOrEmpty(Settings.MaxSettings.RewardedAdUnit.Id)) return;
            MaxSdk.LoadRewardedAd(Settings.MaxSettings.RewardedAdUnit.Id);
#endif
        }

        protected override void InternalShowRewardedAd()
        {
#if PANCAKE_MAX_ENABLE
            if (string.IsNullOrEmpty(Settings.MaxSettings.RewardedAdUnit.Id)) return;
            MaxSdk.ShowRewardedAd(Settings.MaxSettings.RewardedAdUnit.Id);
#endif
        }

        protected override bool InternalIsRewardedAdReady()
        {
#if PANCAKE_MAX_ENABLE
            if (string.IsNullOrEmpty(Settings.MaxSettings.RewardedAdUnit.Id)) return false;
            return MaxSdk.IsRewardedAdReady(Settings.MaxSettings.RewardedAdUnit.Id);
#else
            return false;
#endif
        }

        protected override void InternalLoadRewardedInterstitialAd()
        {
#if PANCAKE_MAX_ENABLE
            if (string.IsNullOrEmpty(Settings.MaxSettings.RewardedInterstitialAdUnit.Id)) return;
            MaxSdk.LoadRewardedInterstitialAd(Settings.MaxSettings.RewardedInterstitialAdUnit.Id);
#endif
        }

        protected override void InternalShowRewardedInterstitialAd()
        {
#if PANCAKE_MAX_ENABLE
            if (string.IsNullOrEmpty(Settings.MaxSettings.RewardedInterstitialAdUnit.Id)) return;
            MaxSdk.ShowRewardedInterstitialAd(Settings.MaxSettings.RewardedInterstitialAdUnit.Id);
#endif
        }

        protected override bool InternalIsRewardedInterstitialAdReady()
        {
#if PANCAKE_MAX_ENABLE
            if (string.IsNullOrEmpty(Settings.MaxSettings.RewardedInterstitialAdUnit.Id)) return false;
            return MaxSdk.IsRewardedInterstitialAdReady(Settings.MaxSettings.RewardedInterstitialAdUnit.Id);
#else
            return false;
#endif
        }

        public override void ShowConsentForm()
        {
#if UNITY_ANDROID
#if PANCAKE_MAX_ENABLE
            if (AdsUtil.IsInEEA())
            {
                MaxSdk.UserService.ShowConsentDialog();
            }
#endif
#elif UNITY_IOS
            if (Unity.Advertisement.IosSupport.ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == Unity.Advertisement.IosSupport.ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                Unity.Advertisement.IosSupport.ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
#endif
        }
    }
}