using System;
using GoogleMobileAds.Api;

namespace Snorlax.Ads
{
    public class AdmobAdClient : AdClient
    {
        private const string NO_SDK_MESSAGE = "SDK missing. Please import the AdMob (Google Mobile Ads) plugin.";
        private AdmobBannerLoader _banner = new AdmobBannerLoader();
        private AdmobInterstitialLoader _interstitial = new AdmobInterstitialLoader();
        private AdmobRewardedLoader _rewarded = new AdmobRewardedLoader();
        private AdmobRewardedInterstitialLoader _rewardedInterstitial = new AdmobRewardedInterstitialLoader();
        private static AdmobAdClient client;
        public static AdmobAdClient Instance => client ??= new AdmobAdClient();
        public override EAdNetwork Network => EAdNetwork.Admob;
        public override bool IsBannerAdSupported => true;
        public override bool IsInsterstitialAdSupport => true;
        public override bool IsRewardedAdSupport => true;
        public override bool IsRewardedInterstitialAdSupport => true;
        public override bool IsAppOpenAdSupport => true;

        public override bool IsSdkAvaiable
        {
            get
            {
#if USE_ADMOB
                return true;
#else
                return false;
#endif
            }
        }

        protected override string NoSdkMessage => NO_SDK_MESSAGE;

        public AdmobBannerLoader Banner => _banner;

        public AdmobInterstitialLoader Interstitial => _interstitial;

        public AdmobRewardedLoader Rewarded => _rewarded;

        public AdmobRewardedInterstitialLoader RewardedInterstitial => _rewardedInterstitial;

        protected override void InternalInit()
        {
            MobileAds.Initialize(status =>
            {
                RuntimeHelper.RunOnMainThread(() =>
                {
                    if (Settings.AdmobSettings.EnableTestMode) Admob.SetupDeviceTest();

                    isInitialized = true;
                });
            });

            _banner.OnClosedEvent += InvokeBannerAdClosed;
            _banner.OnFailToLoadEvent += InvokeBannerAdFailedToLoad;
            _banner.OnLoadedEvent += InvokeBannerAdLoaded;
            _banner.OnOpeningEvent += InvokeBannerAdOpening;
            _banner.OnPaidEvent += InvokeBannerAdPaid;

            _interstitial.OnClosedEvent += InvokeInterstitialAdClosed;
            _interstitial.OnFailToLoadEvent += InvokeInterstitialAdFailedToLoad;
            _interstitial.OnFailToShowEvent += InvokeInterstitialAdFailedToShow;
            _interstitial.OnLoadedEvent += InvokeInterstitialAdLoaded;
            _interstitial.OnOpeningEvent += InvokeInterstitialAdOpening;
            _interstitial.OnPaidEvent += InvokeInterstitialAdPaid;
            _interstitial.OnCompleted += HandleInterstitialCompleted;

            _rewarded.OnClosedEvent += InvokeRewardedAdClosed;
            _rewarded.OnFailToLoadEvent += InvokeRewardedAdFailedToLoad;
            _rewarded.OnFailToShowEvent += InvokeRewardedAdFailedToShow;
            _rewarded.OnLoadedEvent += InvokeRewardedAdLoaded;
            _rewarded.OnOpeningEvent += InvokeRewardedAdOpening;
            _rewarded.OnPaidEvent += InvokeRewardedAdPaid;
            _rewarded.OnRewardEvent += InvokeRewardedAdRewared;
            _rewarded.OnCompleted += HandleRewaredCompleted;
            _rewarded.OnSkipped += HandleRewardedSkipped;

            _rewardedInterstitial.OnClosedEvent += InvokeRewardedInterstitialAdClosed;
            _rewardedInterstitial.OnFailToLoadEvent += InvokeRewardedInterstitialAdFailedToLoad;
            _rewardedInterstitial.OnFailToShowEvent += InvokeRewardedInterstitialAdFailedToShow;
            _rewardedInterstitial.OnLoadedEvent += InvokeRewardedInterstitialAdLoaded;
            _rewardedInterstitial.OnOpeningEvent += InvokeRewardedInterstitialAdOpening;
            _rewardedInterstitial.OnPaidEvent += InvokeRewardedInterstitialAdPaid;
            _rewardedInterstitial.OnRewardEvent += InvokeRewardedInterstitialAdRewared;
            _rewardedInterstitial.OnCompleted += HandleRewaredInterstitialCompleted;
            _rewardedInterstitial.OnSkipped += HandleRewardedInterstitialSkipped;
        }

        public event EventHandler<EventArgs> OnBannerAdClosed;
        public event EventHandler<AdFailedToLoadEventArgs> OnBannerAdFailedToLoad;
        public event EventHandler<EventArgs> OnBannerAdLoaded;
        public event EventHandler<EventArgs> OnBannerAdOpening;
        public event EventHandler<AdValueEventArgs> OnBannerAdPaid;

        public event EventHandler<EventArgs> OnInterstitialAdClosed;
        public event EventHandler<AdFailedToLoadEventArgs> OnInterstitialAdFailedToLoad;
        public event EventHandler<AdErrorEventArgs> OnInterstitialAdFailedToShow;
        public event EventHandler<EventArgs> OnInterstitialAdLoaded;
        public event EventHandler<EventArgs> OnInterstititalAdOpening;
        public event EventHandler<AdValueEventArgs> OnInterstititalAdPaid;

        public event EventHandler<AdValueEventArgs> OnRewardedAdPaid;
        public event EventHandler<EventArgs> OnRewardedAdOpening;
        public event EventHandler<EventArgs> OnRewardedAdLoaded;
        public event EventHandler<AdErrorEventArgs> OnRewardedAdFailedToShow;
        public event EventHandler<AdFailedToLoadEventArgs> OnRewardedAdFailedToLoad;
        public event EventHandler<EventArgs> OnRewardedAdClosed;
        public event EventHandler<Reward> OnRewardedAdRewarded;


        public event EventHandler<AdValueEventArgs> OnRewardedInterstitialAdPaid;
        public event EventHandler<EventArgs> OnRewardedInterstitialAdOpening;
        public event EventHandler<EventArgs> OnRewardedInterstitialAdLoaded;
        public event EventHandler<AdErrorEventArgs> OnRewardedInterstitialAdFailedToShow;
        public event EventHandler<AdFailedToLoadEventArgs> OnRewardedInterstitialAdFailedToLoad;
        public event EventHandler<EventArgs> OnRewardedInterstitialAdClosed;
        public event EventHandler<Reward> OnRewardedInterstitialAdRewarded;


        private void InvokeBannerAdLoaded(AdLoader<AdUnit> instance, object sender, EventArgs args) { OnBannerAdLoaded?.Invoke(sender, args); }
        private void InvokeBannerAdClosed(AdLoader<AdUnit> instance, object sender, EventArgs args) { OnBannerAdClosed?.Invoke(sender, args); }
        private void InvokeBannerAdFailedToLoad(AdLoader<AdUnit> instance, object sender, AdFailedToLoadEventArgs args) { OnBannerAdFailedToLoad?.Invoke(sender, args); }
        private void InvokeBannerAdOpening(AdLoader<AdUnit> instance, object sender, EventArgs args) { OnBannerAdOpening?.Invoke(sender, args); }
        private void InvokeBannerAdPaid(AdLoader<AdUnit> instance, object sender, AdValueEventArgs args) { OnBannerAdPaid?.Invoke(sender, args); }
        private void InvokeInterstitialAdClosed(AdLoader<AdUnit> instance, object sender, EventArgs args) { OnInterstitialAdClosed?.Invoke(sender, args); }

        private void InvokeInterstitialAdFailedToLoad(AdLoader<AdUnit> instance, object sender, AdFailedToLoadEventArgs args)
        {
            OnInterstitialAdFailedToLoad?.Invoke(sender, args);
        }

        private void InvokeInterstitialAdFailedToShow(AdLoader<AdUnit> instance, object sender, AdErrorEventArgs args)
        {
            OnInterstitialAdFailedToShow?.Invoke(sender, args);
        }

        private void InvokeInterstitialAdLoaded(AdLoader<AdUnit> instance, object sender, EventArgs args) { OnInterstitialAdLoaded?.Invoke(sender, args); }
        private void InvokeInterstitialAdOpening(AdLoader<AdUnit> instance, object sender, EventArgs args) { OnInterstititalAdOpening?.Invoke(sender, args); }
        private void InvokeInterstitialAdPaid(AdLoader<AdUnit> instance, object sender, AdValueEventArgs args) { OnInterstititalAdPaid?.Invoke(sender, args); }
        private void InvokeRewardedAdPaid(AdLoader<AdUnit> instance, object sender, AdValueEventArgs args) { OnRewardedAdPaid?.Invoke(sender, args); }
        private void InvokeRewardedAdOpening(AdLoader<AdUnit> instance, object sender, EventArgs args) { OnRewardedAdOpening?.Invoke(sender, args); }
        private void InvokeRewardedAdLoaded(AdLoader<AdUnit> instance, object sender, EventArgs args) { OnRewardedAdLoaded?.Invoke(sender, args); }
        private void InvokeRewardedAdFailedToShow(AdLoader<AdUnit> instance, object sender, AdErrorEventArgs args) { OnRewardedAdFailedToShow?.Invoke(sender, args); }

        private void InvokeRewardedAdFailedToLoad(AdLoader<AdUnit> instance, object sender, AdFailedToLoadEventArgs args)
        {
            OnRewardedAdFailedToLoad?.Invoke(sender, args);
        }

        private void InvokeRewardedAdClosed(AdLoader<AdUnit> instance, object sender, EventArgs args) { OnRewardedAdClosed?.Invoke(sender, args); }
        private void InvokeRewardedAdRewared(AdLoader<AdUnit> instance, object sender, Reward args) { OnRewardedAdRewarded?.Invoke(sender, args); }

        private void HandleInterstitialCompleted(AdmobInterstitialLoader instance) { InternalInterstitialCompleted(instance); }

        protected virtual void InternalInterstitialCompleted(AdmobInterstitialLoader instance) { RuntimeHelper.RunOnMainThread(InvokeInterstitialAdCompleted); }
        private void HandleRewaredCompleted(AdmobRewardedLoader instance) { InternalRewaredCompleted(instance); }

        protected virtual void InternalRewaredCompleted(AdmobRewardedLoader instance) { RuntimeHelper.RunOnMainThread(InvokeRewardedAdCompleted); }

        private void HandleRewardedSkipped(AdmobRewardedLoader instance) { InternalRewardSkipped(instance); }

        protected virtual void InternalRewardSkipped(AdmobRewardedLoader instance) { RuntimeHelper.RunOnMainThread(InvokeRewardedAdSkipped); }

        private void HandleRewardedInterstitialSkipped(AdmobRewardedInterstitialLoader instance) { InternalRewardedInterstitialSkipped(instance); }

        private void HandleRewaredInterstitialCompleted(AdmobRewardedInterstitialLoader instance) { InternalRewaredInterstitialCompleted(instance); }

        protected virtual void InternalRewardedInterstitialSkipped(AdmobRewardedInterstitialLoader loader)
        {
            RuntimeHelper.RunOnMainThread(InvokeRewardedInterstitialAdSkipped);
        }

        protected virtual void InternalRewaredInterstitialCompleted(AdmobRewardedInterstitialLoader loader)
        {
            RuntimeHelper.RunOnMainThread(InvokeRewardedInterstitialAdCompleted);
        }

        private void InvokeRewardedInterstitialAdRewared(AdmobRewardedInterstitialLoader instance, Reward args) { OnRewardedInterstitialAdRewarded?.Invoke(null, args); }

        private void InvokeRewardedInterstitialAdPaid(AdmobRewardedInterstitialLoader instance, object sender, AdValueEventArgs args)
        {
            OnRewardedInterstitialAdPaid?.Invoke(sender, args);
        }

        private void InvokeRewardedInterstitialAdOpening(AdmobRewardedInterstitialLoader instance, object sender, EventArgs args)
        {
            OnRewardedInterstitialAdOpening?.Invoke(sender, args);
        }

        private void InvokeRewardedInterstitialAdLoaded(AdmobRewardedInterstitialLoader instance) { OnRewardedInterstitialAdLoaded?.Invoke(null, null); }

        private void InvokeRewardedInterstitialAdFailedToShow(AdmobRewardedInterstitialLoader instance, object sender, AdErrorEventArgs args)
        {
            OnRewardedInterstitialAdFailedToShow?.Invoke(sender, args);
        }

        private void InvokeRewardedInterstitialAdFailedToLoad(AdmobRewardedInterstitialLoader instance, AdFailedToLoadEventArgs args)
        {
            OnRewardedInterstitialAdFailedToLoad?.Invoke(null, args);
        }

        private void InvokeRewardedInterstitialAdClosed(AdmobRewardedInterstitialLoader instance, object sender, EventArgs args)
        {
            OnRewardedInterstitialAdClosed?.Invoke(sender, args);
        }

        protected override void InternalShowBannerAd()
        {
            _banner.Load();
            _banner.Show();
        }

        protected override void InternalHideBannerAd() { _banner.Hide(); }

        protected override void InternalDestroyBannerAd() { _banner.Destroy(); }

        protected override void InternalLoadInterstitialAd() { _interstitial?.Load(); }

        protected override bool InternalIsInterstitialAdReady() { return _interstitial.IsReady(); }

        protected override void InternalShowInterstitialAd() { _interstitial?.Show(); }

        protected override void InternalLoadRewardedAd() { _rewarded?.Load(); }

        protected override bool InternalIsRewardedAdReady() { return _rewarded.IsReady(); }

        protected override void InternalShowRewardedAd() { _rewarded?.Show(); }

        protected override void InternalLoadRewardedInterstitialAd() { _rewardedInterstitial.Load(); }

        protected override bool InternalIsRewardedInterstitialAdReady() { return _rewardedInterstitial.IsReady(); }

        protected override void InternalShowRewardedInterstitialAd() { _rewardedInterstitial.Show(); }
    }
}