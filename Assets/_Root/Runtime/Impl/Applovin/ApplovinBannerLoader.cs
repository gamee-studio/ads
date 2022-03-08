// ReSharper disable AccessToStaticMemberViaDerivedType

namespace Snorlax.Ads
{
    public class ApplovinBannerLoader
    {
        private readonly ApplovinAdClient _client;

        public ApplovinBannerLoader(ApplovinAdClient client)
        {
            _client = client;
            Initialized();
        }

        private void Initialized()
        {
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnAdLoaded;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnAdClicked;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnAdExpanded;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnAdLoadFailed;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnAdCollapsed;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaid;
            MaxSdk.CreateBanner(Settings.ApplovinSettings.BannerAdUnit.Id, Settings.ApplovinSettings.BannerAdUnit.ConvertPosition());
            // if (Settings.ApplovinSettings.BannerAdUnit.useAdaptiveBanner)
            // {
            //     MaxSdk.SetBannerExtraParameter(Settings.ApplovinSettings.BannerAdUnit.Id, "adaptive_banner", true);
            // }
        }

        private void OnAdRevenuePaid(string unit, MaxSdkBase.AdInfo info) { _client.InvokeBannerAdRevenuePaid(info); }

        private void OnAdCollapsed(string unit, MaxSdkBase.AdInfo info) { _client.InvokeBannerAdCollapsed(); }

        private void OnAdLoadFailed(string unit, MaxSdkBase.ErrorInfo error) { _client.InvokeBannerAdFaildToLoad(); }

        private void OnAdExpanded(string unit, MaxSdkBase.AdInfo info) { _client.InvokeBannerAdExpanded(); }

        private void OnAdClicked(string unit, MaxSdkBase.AdInfo info) { _client.InvokeBannerAdClicked(); }

        private void OnAdLoaded(string unit, MaxSdkBase.AdInfo info) { _client.InvokeBannerAdLoaded(); }
    }
}