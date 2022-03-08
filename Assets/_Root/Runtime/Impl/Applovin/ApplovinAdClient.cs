using System;

// ReSharper disable AccessToStaticMemberViaDerivedType
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
        public static ApplovinAdClient Instance => client ??= new ApplovinAdClient();

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

        protected override string NoSdkMessage => NO_SDK_MESSAGE;

        #region internal

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

        #endregion

        protected override void InternalInit()
        {
            MaxSdk.SetSdkKey(Settings.ApplovinSettings.SdkKey);
            MaxSdk.InitializeSdk();
            MaxSdk.SetIsAgeRestrictedUser(Settings.ApplovinSettings.EnableAgeRestrictedUser);

            _banner = new ApplovinBannerLoader(this);
            _interstitial = new ApplovinInterstitialLoader(this);
            _rewarded = new ApplovinRewardedLoader(this);
            _rewardedInterstitial = new ApplovinRewardedInterstitialLoader(this);

            LoadInterstitialAd();
            LoadRewardedAd();
            LoadRewardedInterstitialAd();
            isInitialized = true;
        }

        protected override void InternalShowBannerAd()
        {
            if (string.IsNullOrEmpty(Settings.ApplovinSettings.BannerAdUnit.Id)) return;
            MaxSdk.ShowBanner(Settings.ApplovinSettings.BannerAdUnit.Id);
        }

        protected override void InternalHideBannerAd()
        {
            if (string.IsNullOrEmpty(Settings.ApplovinSettings.BannerAdUnit.Id)) return;
            MaxSdk.HideBanner(Settings.ApplovinSettings.BannerAdUnit.Id);
        }

        protected override void InternalDestroyBannerAd()
        {
            if (string.IsNullOrEmpty(Settings.ApplovinSettings.BannerAdUnit.Id)) return;
            MaxSdk.DestroyBanner(Settings.ApplovinSettings.BannerAdUnit.Id);
        }

        protected override void InternalLoadInterstitialAd()
        {
            if (string.IsNullOrEmpty(Settings.ApplovinSettings.InterstitialAdUnit.Id)) return;
            MaxSdk.LoadInterstitial(Settings.ApplovinSettings.InterstitialAdUnit.Id);
        }

        protected override void InternalShowInterstitialAd()
        {
            if (string.IsNullOrEmpty(Settings.ApplovinSettings.InterstitialAdUnit.Id)) return;
            MaxSdk.ShowInterstitial(Settings.ApplovinSettings.InterstitialAdUnit.Id);
        }

        protected override bool InternalIsInterstitialAdReady()
        {
            if (string.IsNullOrEmpty(Settings.ApplovinSettings.InterstitialAdUnit.Id)) return false;
            return MaxSdk.IsInterstitialReady(Settings.ApplovinSettings.InterstitialAdUnit.Id);
        }

        protected override void InternalLoadRewardedAd()
        {
            if (string.IsNullOrEmpty(Settings.ApplovinSettings.RewardedAdUnit.Id)) return;
            MaxSdk.LoadRewardedAd(Settings.ApplovinSettings.RewardedAdUnit.Id);
        }

        protected override void InternalShowRewardedAd()
        {
            if (string.IsNullOrEmpty(Settings.ApplovinSettings.RewardedAdUnit.Id)) return;
            MaxSdk.ShowRewardedAd(Settings.ApplovinSettings.RewardedAdUnit.Id);
        }

        protected override bool InternalIsRewardedAdReady()
        {
            if (string.IsNullOrEmpty(Settings.ApplovinSettings.RewardedAdUnit.Id)) return false;
            return MaxSdk.IsRewardedAdReady(Settings.ApplovinSettings.RewardedAdUnit.Id);
        }

        protected override void InternalLoadRewardedInterstitialAd()
        {
            if (string.IsNullOrEmpty(Settings.ApplovinSettings.RewardedInterstitialAdUnit.Id)) return;
            MaxSdk.LoadRewardedInterstitialAd(Settings.ApplovinSettings.RewardedInterstitialAdUnit.Id);
        }

        protected override void InternalShowRewardedInterstitialAd()
        {
            if (string.IsNullOrEmpty(Settings.ApplovinSettings.RewardedInterstitialAdUnit.Id)) return;
            MaxSdk.ShowRewardedInterstitialAd(Settings.ApplovinSettings.RewardedInterstitialAdUnit.Id);
        }

        protected override bool InternalIsRewardedInterstitialAdReady()
        {
            if (string.IsNullOrEmpty(Settings.ApplovinSettings.RewardedInterstitialAdUnit.Id)) return false;
            return MaxSdk.IsRewardedInterstitialAdReady(Settings.ApplovinSettings.RewardedInterstitialAdUnit.Id);
        }
    }
}