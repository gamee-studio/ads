#if ADS_FIREBASE_TRACKING
using Firebase.Analytics;

namespace Pancake.Monetization
{
    internal static class AppTracking
    {
#if PANCAKE_MAX_ENABLE
        internal static void TrackingRevenue(MaxSdkBase.AdInfo adInfo)
        {
            // Log an event with ad value parameters
            Parameter[] LTVParameters =
            {
                // Log ad value in micros.
                new Parameter("valuemicros", adInfo.Revenue * 1000000),
                // These values below won’t be used in ROASrecipe.
                // But log for purposes of debugging and futurereference.
                new Parameter("currency", "USD"), new Parameter("precision", 1), new Parameter("adunitid", adInfo.AdUnitIdentifier),
                new Parameter("network", adInfo.NetworkName)
            };

            FirebaseAnalytics.LogEvent("paid_ad_impression", LTVParameters);
        }
#endif

#if PANCAKE_ADMOB_ENABLE
        internal static void TrackingRevenue(GoogleMobileAds.Api.AdValueEventArgs e, string unitId)
        {
            // Log an event with ad value parameters
            Parameter[] LTVParameters =
            {
                // Log ad value in micros.
                new Parameter("valuemicros", e.AdValue.Value),
                // These values below won’t be used in ROASrecipe.
                // But log for purposes of debugging and futurereference.
                new Parameter("currency", e.AdValue.CurrencyCode), new Parameter("precision", (int) e.AdValue.Precision), new Parameter("adunitid", unitId),
                new Parameter("network", "admob")
            };

            FirebaseAnalytics.LogEvent("paid_ad_impression", LTVParameters);
        }
#endif
    }
}

#endif