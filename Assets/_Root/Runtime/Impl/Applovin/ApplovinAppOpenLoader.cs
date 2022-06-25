#if PANCAKE_MAX_ENABLE && PANCAKE_ADMOB_ENABLE
using System;
using GoogleMobileAds.Api;
#endif

namespace Pancake.Monetization
{
    public class ApplovinAppOpenLoader : AdLoader<AdUnit>
    {
        private readonly ApplovinAdClient _client;
#if PANCAKE_MAX_ENABLE && PANCAKE_ADMOB_ENABLE
        private AppOpenAd _appOpenAd;

        internal override bool IsReady() { return _appOpenAd != null; }
#endif
        public ApplovinAppOpenLoader(ApplovinAdClient client)
        {
            _client = client;
#if PANCAKE_MAX_ENABLE && PANCAKE_ADMOB_ENABLE
            unit = Settings.MaxSettings.AppOpenAdUnit;
#endif
        }
#if PANCAKE_MAX_ENABLE && PANCAKE_ADMOB_ENABLE
        internal override void Load() { AppOpenAd.LoadAd(unit.Id, ((ApplovinAppOpenUnit) unit).orientation, Admob.CreateRequest(), OnAdLoadCallback); }

        private void OnAdLoadCallback(AppOpenAd appOpenAd, AdFailedToLoadEventArgs error)
        {
            if (error != null)
            {
                OnAdFaildToLoad(error);
                return;
            }

            _appOpenAd = appOpenAd;
            OnAdLoaded();
            _appOpenAd.OnAdDidDismissFullScreenContent += OnAdClosed;
            _appOpenAd.OnAdDidRecordImpression += OnAdDidRecordImpression;
            _appOpenAd.OnAdDidPresentFullScreenContent += OnAdOpening;
            _appOpenAd.OnAdFailedToPresentFullScreenContent += OnAdFailedToShow;
            _appOpenAd.OnPaidEvent += OnPaidHandleEvent;
        }

        private void OnAdDidRecordImpression(object sender, EventArgs e) { _client.InvokeAppOpenAdDidRecordImpression(); }

        private void OnPaidHandleEvent(object sender, AdValueEventArgs e) { _client.InvokeAppOpenAdRevenuePaid(e); }

        private void OnAdFailedToShow(object sender, AdErrorEventArgs e) { _client.InvokeAppOpenAdFailedToShow(); }

        private void OnAdOpening(object sender, EventArgs e) { _client.InvokeAppOpenAdOpening(); }

        private void OnAdClosed(object sender, EventArgs e)
        {
            _client.InvokeAppOpenAdClosed();
            _client.InternalAppOpenAdCompleted(this);
            Destroy();
        }

        private void OnAdLoaded() { _client.InvokeAppOpenAdLoaded(); }

        private void OnAdFaildToLoad(AdFailedToLoadEventArgs e)
        {
            _client.InvokeAppOpenAdFailedToLoad();
            Destroy();
        }

        internal override void Show() { _appOpenAd?.Show(); }

        internal override void Destroy()
        {
            _appOpenAd?.Destroy();
            _appOpenAd = null;
        }
#endif
    }
}