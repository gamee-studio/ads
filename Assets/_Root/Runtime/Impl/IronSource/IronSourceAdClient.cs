using System;

namespace Pancake.Monetization
{
    public class IronSourceAdClient : AdClient
    {
        private const string NO_SDK_MESSAGE = "SDK missing. Please import IronSource plugin.";
        private IronSourceBannerLoader _banner;
        private IronSourceInterstitialLoader _interstitial;
        private IronSourceRewardedLoader _rewarded;
        private ApplovinAppOpenLoader _appOpen;
        private static IronSourceAdClient client;
        private bool _isBannerLoaded;
        private bool _isRewardedCompleted;

        public static IronSourceAdClient Instance => client ?? (client = new IronSourceAdClient());

        public override EAdNetwork Network => EAdNetwork.IronSource;
        public override bool IsBannerAdSupported => true;
        public override bool IsInsterstitialAdSupport => true;
        public override bool IsRewardedAdSupport => true;
        public override bool IsRewardedInterstitialAdSupport => false;
        public override bool IsAppOpenAdSupport => false;

        public override bool IsSdkAvaiable
        {
            get
            {
#if PANCAKE_IRONSOURCE_ENABLE
                return true;
#else
                return false;
#endif
            }
        }

#if PANCAKE_IRONSOURCE_ENABLE
        public event Action OnBannerAdLoadedEvent;
        public event Action<IronSourceError> OnBannerAdLoadFailedEvent;
        public event Action OnBannerAdClickedEvent;
        public event Action OnBannerAdScreenPresentedEvent;
        public event Action OnBannerAdScreenDismissedEvent;
        public event Action OnBannerAdLeftApplicationEvent;

        public event Action OnInterstitialAdClickedEvent;
        public event Action OnInterstitialAdClosedEvent;
        public event Action<IronSourceError> OnInterstitialAdLoadFailedEvent;
        public event Action OnInterstitialAdOpenedEvent;
        public event Action OnInterstitialAdReadyEvent;
        public event Action OnInterstitialAdShowSucceededEvent;
        public event Action<IronSourceError> OnInterstitialAdShowFailedEvent;

        public event Action OnRewardedVideoAdOpenedEvent;
        public event Action<IronSourcePlacement> OnRewardedVideoAdClickedEvent;
        public event Action OnRewardedVideoAdClosedEvent;
        public event Action<bool> OnRewardedVideoAvailabilityChangedEvent;
        public event Action OnRewardedVideoAdStartedEvent;
        public event Action OnRewardedVideoAdEndedEvent;
        public event Action<IronSourcePlacement> OnRewardedVideoAdRewardedEvent;
        public event Action<IronSourceError> OnRewardedVideoAdShowFailedEvent;

        public event Action<string> OnSegmentReceivedEvent;
        
#if PANCAKE_ADMOB_ENABLE
        public event Action<GoogleMobileAds.Api.AdValueEventArgs> OnAppOpenAdPaid;
#endif

        public event Action OnAppOpenAdOpening;
        public event Action OnAppOpenAdLoaded;
        public event Action OnAppOpenAdFailedToShow;
        public event Action OnAppOpenAdFailedToLoad;
        public event Action OnAppOpenAdClosed;
        public event Action OnAppOpenAdDidRecordImpression;
        
#endif

#if PANCAKE_IRONSOURCE_ENABLE
        internal void InvokeBannerAdLoaded()
        {
            OnBannerAdLoadedEvent?.Invoke();
            _isBannerLoaded = true;
        }

        internal void InvokeBannerAdLoadFailed(IronSourceError error)
        {
            OnBannerAdLoadFailedEvent?.Invoke(error);
            _isBannerLoaded = false;
        }

        internal void InvokeBannerAdClicked() { OnBannerAdClickedEvent?.Invoke(); }
        internal void InvokeBannerAdScreenPresented() { OnBannerAdScreenPresentedEvent?.Invoke(); }
        internal void InvokeBannerAdScreenDismissed() { OnBannerAdScreenDismissedEvent?.Invoke(); }
        internal void InvokeBannerAdLeftApplication() { OnBannerAdLeftApplicationEvent?.Invoke(); }

        internal void InvokeInterstitialAdClicked() { OnInterstitialAdClickedEvent?.Invoke(); }

        internal void InvokeInterstitialAdClosed()
        {
            OnInterstitialAdClosedEvent?.Invoke();
            InvokeInterstitialAdCompleted();
        }

        internal void InvokeInterstitialAdLoadFailed(IronSourceError error) { OnInterstitialAdLoadFailedEvent?.Invoke(error); }
        internal void InvokeInterstitialAdOpened() { OnInterstitialAdOpenedEvent?.Invoke(); }
        internal void InvokeInterstitialAdReady() { OnInterstitialAdReadyEvent?.Invoke(); }
        internal void InvokeInterstitialAdShowSucceeded() { OnInterstitialAdShowSucceededEvent?.Invoke(); }
        internal void InvokeInterstitialAdShowFailed(IronSourceError error) { OnInterstitialAdShowFailedEvent?.Invoke(error); }

        internal void InvokeRewardedVideoAdOpened()
        {
            OnRewardedVideoAdOpenedEvent?.Invoke();
            _isRewardedCompleted = false;
        }

        internal void InvokeRewardedVideoAdClicked(IronSourcePlacement placement) { OnRewardedVideoAdClickedEvent?.Invoke(placement); }

        internal void InvokeRewardedVideoAdClosed()
        {
            OnRewardedVideoAdClosedEvent?.Invoke();

            if (_isRewardedCompleted) InvokeRewardedAdCompleted();
            else InvokeRewardedAdSkipped();

            _isRewardedCompleted = false;
        }

        internal void InvokeRewardedVideoAvailabilityChanged(bool status) { OnRewardedVideoAvailabilityChangedEvent?.Invoke(status); }

        internal void InvokeRewardedVideoAdStarted()
        {
            OnRewardedVideoAdStartedEvent?.Invoke();
            _isRewardedCompleted = false;
        }

        internal void InvokeRewardedVideoAdEnded() { OnRewardedVideoAdEndedEvent?.Invoke(); }

        internal void InvokeRewardedVideoAdRewarded(IronSourcePlacement placement)
        {
            if (placement == null) return;
            OnRewardedVideoAdRewardedEvent?.Invoke(placement);
            _isRewardedCompleted = true;
        }

        internal void InvokeRewardedVideoAdShowFailed(IronSourceError error)
        {
            OnRewardedVideoAdShowFailedEvent?.Invoke(error);
            _isRewardedCompleted = false;
        }

        internal void InvokeSegmentReceived(string segment) { OnSegmentReceivedEvent?.Invoke(segment); }
#if PANCAKE_ADMOB_ENABLE
        internal void InvokeAppOpenAdRevenuePaid(GoogleMobileAds.Api.AdValueEventArgs value) { OnAppOpenAdPaid?.Invoke(value); }
        internal void InvokeAppOpenAdOpening() { OnAppOpenAdOpening?.Invoke(); }
        internal void InvokeAppOpenAdLoaded() { OnAppOpenAdLoaded?.Invoke(); }
        internal void InvokeAppOpenAdFailedToShow() { OnAppOpenAdFailedToShow?.Invoke(); }
        internal void InvokeAppOpenAdFailedToLoad() { OnAppOpenAdFailedToLoad?.Invoke(); }
        internal void InvokeAppOpenAdClosed() { OnAppOpenAdClosed?.Invoke(); }
        internal void InvokeAppOpenAdDidRecordImpression() { OnAppOpenAdDidRecordImpression?.Invoke(); }
        internal virtual void InternalAppOpenAdCompleted(IronSourceAppOpenLoader instance) { InvokeAppOpenAdCompleted(); }
#endif
#endif

        protected override string NoSdkMessage => NO_SDK_MESSAGE;
        //public IronSourceBannerLoader Banner => _banner;
        //public IronSourceInterstitialLoader Interstitial => _interstitial;
        //public IronSourceRewardedLoader Rewarded => _rewarded;

        protected override void InternalInit()
        {
#if PANCAKE_IRONSOURCE_ENABLE
            IronSource.Agent.init(Settings.IronSourceSettings.AppKey.Id,
                IronSourceAdUnits.REWARDED_VIDEO,
                IronSourceAdUnits.INTERSTITIAL,
                IronSourceAdUnits.OFFERWALL,
                IronSourceAdUnits.BANNER);
            IronSource.Agent.validateIntegration();
            IronSourceEvents.onSegmentReceivedEvent += InvokeSegmentReceived;
#endif
            
            _banner = new IronSourceBannerLoader(this);
            _interstitial = new IronSourceInterstitialLoader(this);
            _rewarded = new IronSourceRewardedLoader(this);
            isInitialized = true;
        }

        protected override void InternalShowBannerAd()
        {
#if PANCAKE_IRONSOURCE_ENABLE
            if (!_isBannerLoaded)
            {
                var bannerUnit = Settings.IronSourceSettings.BannerAdUnit;
                var size = bannerUnit.ConvertSize();
                size.SetAdaptive(Settings.IronSourceSettings.UseAdaptiveBanner);
                IronSource.Agent.loadBanner(size, bannerUnit.ConvertPosition());
            }

            IronSource.Agent.displayBanner();
#endif
        }

        protected override void InternalHideBannerAd()
        {
#if PANCAKE_IRONSOURCE_ENABLE
            IronSource.Agent.hideBanner();
#endif
        }

        protected override void InternalDestroyBannerAd()
        {
#if PANCAKE_IRONSOURCE_ENABLE
            IronSource.Agent.destroyBanner();
            _isBannerLoaded = false;
#endif
        }

        protected override void InternalLoadInterstitialAd()
        {
#if PANCAKE_IRONSOURCE_ENABLE
            IronSource.Agent.loadInterstitial();
#endif
        }

        protected override void InternalShowInterstitialAd()
        {
#if PANCAKE_IRONSOURCE_ENABLE
            R.isShowingAd = true;
            IronSource.Agent.showInterstitial();
#endif
        }

        protected override bool InternalIsInterstitialAdReady()
        {
#if PANCAKE_IRONSOURCE_ENABLE
            return IronSource.Agent.isInterstitialReady();
#else
            return false;
#endif
        }

        protected override void InternalLoadRewardedAd()
        {
#if PANCAKE_IRONSOURCE_ENABLE
            //IronSource.Agent.loadRewardedVideo(); // IronSource loads rewarded video ads in the background automatically,
#endif
        }

        protected override void InternalShowRewardedAd()
        {
#if PANCAKE_IRONSOURCE_ENABLE
             R.isShowingAd = true;
            IronSource.Agent.showRewardedVideo();
#endif
        }

        protected override bool InternalIsRewardedAdReady()
        {
#if PANCAKE_IRONSOURCE_ENABLE
            return IronSource.Agent.isRewardedVideoAvailable();
#else
            return false;
#endif
        }
        
#if PANCAKE_ADMOB_ENABLE
        protected override void InternalLoadAppOpenAd() { _appOpen.Load();}
        protected override void InternalShowAppOpenAd() { _appOpen.Show(); }
        protected override bool InternalIsAppOpenAdReady() { return _appOpen.IsReady(); }
#endif
    }
}