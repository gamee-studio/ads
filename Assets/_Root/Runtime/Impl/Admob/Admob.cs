#if PANCAKE_ADMOB_ENABLE
using GoogleMobileAds.Api;
using UnityEngine;

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
            if (Settings.AdSettings.EnableGDPR) builder.AddExtra("npa", GDPRHelper.GetValueGDPR().ToString());

            return builder.Build();
        }
    }
}
#endif