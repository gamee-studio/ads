namespace Snorlax.Ads
{
    public enum EAdNetwork
    {
        None = 0,
        Admob = 1
    }

    public enum EBannerAdNetwork
    {
        None = EAdNetwork.None,
        Admob = EAdNetwork.Admob
    }

    public enum EInterstitialAdNetwork
    {
        None = EAdNetwork.None,
        Admob = EAdNetwork.Admob
    }

    public enum ERewardedAdNetwork
    {
        None = EAdNetwork.None,
        Admob = EAdNetwork.Admob
    }  
    
    public enum ERewardedInterstitialAdNetwork
    {
        None = EAdNetwork.None,
        Admob = EAdNetwork.Admob
    }
}