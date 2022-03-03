using Snorlax.Ads;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject fetchInterstitial;
    public GameObject fetchRewarded;
    public GameObject fetchRewardedInterstitial;


    private void Update()
    {
        fetchInterstitial.SetActive(!Advertising.IsInterstitialAdReady());
        fetchRewarded.SetActive(!Advertising.IsRewardedAdReady());
        fetchRewardedInterstitial.SetActive(!Advertising.IsRewardedInterstitialAdReady());
    }
    
    public void HideBanner()
    {
        Advertising.HideBannerAd();
    }
    
    public void DestroyBanner()
    {
        Advertising.DestroyBannerAd();
    }

    public void ShowBanner()
    {
        Advertising.ShowBannerAd();
    }
    public void ShowInterstitial()
    {
        Advertising.ShowInterstitialAd();
    }
    
    public void ShowRewarded()
    {
        Advertising.ShowRewardedAd();
    }  
    
    public void ShowRewardedIntersttitial()
    {
        Advertising.ShowRewardedInterstitialAd();
    }
}
