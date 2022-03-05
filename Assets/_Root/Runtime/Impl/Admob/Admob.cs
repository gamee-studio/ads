#if PANCAKE_ADMOB_ENABLE
using GoogleMobileAds.Api;

namespace Snorlax.Ads
{
    public static class Admob
    {
        internal static void SetupDeviceTest()
        {
            var configuration = new RequestConfiguration.Builder().SetTestDeviceIds(Settings.AdmobSettings.DevicesTest).build();
            MobileAds.SetRequestConfiguration(configuration);
        }

        internal static AdRequest CreateRequest()
        {
            var builder = new AdRequest.Builder();
            // targetting setting
            // extra options
            // consent
            return builder.Build();
        }
    }
}
#endif