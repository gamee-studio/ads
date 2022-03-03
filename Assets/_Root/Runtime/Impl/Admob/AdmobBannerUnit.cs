using System;
using GoogleMobileAds.Api;

namespace Snorlax.Ads
{
    [Serializable]
    public class AdmobBannerUnit : BannerAdUnit
    {
        public EBannerSize size = EBannerSize.Banner;

        public AdmobBannerUnit(string iOSId, string androidId)
            : base(iOSId, androidId)
        {
        }

        public AdSize ConvertSize()
        {
            switch (size)
            {
                case EBannerSize.Banner: return AdSize.Banner;
                case EBannerSize.LargeBanner:
                case EBannerSize.MediumRectangle:
                    return AdSize.MediumRectangle;
                case EBannerSize.FullBanner: return AdSize.IABBanner;
                case EBannerSize.Leaderboard: return AdSize.Leaderboard;
                case EBannerSize.SmartBanner: return AdSize.SmartBanner;
                default:
                    return AdSize.SmartBanner;
            }
        }

        public AdPosition ConvertPosition()
        {
            switch (position)
            {
                case EBannerPosition.Top: return AdPosition.Top;
                case EBannerPosition.Bottom: return AdPosition.Bottom;
                case EBannerPosition.TopLeft: return AdPosition.TopLeft;
                case EBannerPosition.TopRight: return AdPosition.TopRight;
                case EBannerPosition.BottomLeft: return AdPosition.BottomLeft;
                case EBannerPosition.BottomRight: return AdPosition.BottomRight;
                default:
                    return AdPosition.Bottom;
            }
        }
    }
}