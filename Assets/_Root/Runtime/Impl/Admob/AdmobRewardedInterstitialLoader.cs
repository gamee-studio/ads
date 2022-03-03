using System;
using GoogleMobileAds.Api;

namespace Snorlax.Ads
{
    public class AdmobRewardedInterstitialLoader : AdLoader<AdUnit>
    {
        private RewardedInterstitialAd _rewardedInterstitialAd;
        public bool IsEarnRewardedInterstitial { get; private set; }
        public event Action<AdmobRewardedInterstitialLoader> OnCompleted = delegate { };
        public event Action<AdmobRewardedInterstitialLoader> OnSkipped = delegate { };
        public event Action<AdmobRewardedInterstitialLoader, object, AdValueEventArgs> OnPaidEvent = delegate { };
        public event Action<AdmobRewardedInterstitialLoader, object, EventArgs> OnOpeningEvent = delegate { };
        public event Action<AdmobRewardedInterstitialLoader> OnLoadedEvent = delegate { };
        public event Action<AdmobRewardedInterstitialLoader, object, AdErrorEventArgs> OnFailToShowEvent = delegate { };
        public event Action<AdmobRewardedInterstitialLoader, AdFailedToLoadEventArgs> OnFailToLoadEvent = delegate { };
        public event Action<AdmobRewardedInterstitialLoader, object, EventArgs> OnRecordImpressionEvent = delegate { };
        public event Action<AdmobRewardedInterstitialLoader, object, EventArgs> OnClosedEvent = delegate { };
        public event Action<AdmobRewardedInterstitialLoader, Reward> OnRewardEvent = delegate { };

        public AdmobRewardedInterstitialLoader() { unit = Settings.AdmobSettings.RewardedInterstitialAdUnit; }

        internal override bool IsReady() { return _rewardedInterstitialAd != null; }

        internal override void Load()
        {
            RewardedInterstitialAd.LoadAd(unit.Id, Admob.CreateRequest(), OnAdLoadCallback);
        }

        private void OnAdLoadCallback(RewardedInterstitialAd arg1, AdFailedToLoadEventArgs arg2)
        {
            if (arg2 != null)
            {
                OnAdFailedToLoad(arg2);
                return;
            }

            _rewardedInterstitialAd = arg1;
            OnAdLoaded();

            _rewardedInterstitialAd.OnAdDidDismissFullScreenContent += OnAdClosed;
            _rewardedInterstitialAd.OnAdDidRecordImpression += OnAdDidRecordImpression;
            _rewardedInterstitialAd.OnAdDidPresentFullScreenContent += OnAdOpening;
            _rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += OnAdFailedToShow;
            _rewardedInterstitialAd.OnPaidEvent += OnPaidHandleEvent;
        }

        private void OnRewardHandleEvent(Reward e)
        {
            IsEarnRewardedInterstitial = true;
            OnRewardEvent.Invoke(this, e);
        }

        private void OnPaidHandleEvent(object sender, AdValueEventArgs e) { OnPaidEvent.Invoke(this, sender, e); }

        private void OnAdDidRecordImpression(object sender, EventArgs e) { OnRecordImpressionEvent.Invoke(this, sender, e); }

        private void OnAdOpening(object sender, EventArgs e) { OnOpeningEvent.Invoke(this, sender, e); }

        private void OnAdLoaded() { OnLoadedEvent.Invoke(this); }

        private void OnAdFailedToShow(object sender, AdErrorEventArgs e) { OnFailToShowEvent.Invoke(this, sender, e); }

        private void OnAdFailedToLoad(AdFailedToLoadEventArgs e)
        {
            OnFailToLoadEvent.Invoke(this, e);
            Destroy();
        }

        private void OnAdClosed(object sender, EventArgs e)
        {
            OnClosedEvent.Invoke(this, sender, e);
            if (IsEarnRewardedInterstitial)
            {
                OnCompleted.Invoke(this);
            }
            else
            {
                OnSkipped.Invoke(this);
            }

            Destroy();
        }

        internal override void Show()
        {
            _rewardedInterstitialAd?.Show(OnRewardHandleEvent);
        }

        internal override void Destroy()
        {
            _rewardedInterstitialAd?.Destroy();
            _rewardedInterstitialAd = null;
        }
    }
}