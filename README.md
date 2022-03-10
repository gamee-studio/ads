# What

Integration module for implementing in-game advertising for Unity 3d

## Support network

1. Admob mediation
2. Applovin mediation (MAX)

## How To Install

Add the following lines

- for newest update

```csharp
"com.gamee.ads": "https://github.com/gamee-studio/ads.git?path=Assets/_Root",
"com.snorlax.locale": "https://github.com/snorluxe/locale.git?path=Assets/_Root#1.0.2",
"com.google.external-dependency-manager": "https://github.com/snorluxe/external-dependency-manager.git?path=Assets/_Root#1.2.169",
```

- for excactly version

```csharp
"com.gamee.ads": "https://github.com/gamee-studio/ads.git?path=Assets/_Root#1.0.4",
"com.snorlax.locale": "https://github.com/snorluxe/locale.git?path=Assets/_Root#1.0.2",
"com.google.external-dependency-manager": "https://github.com/snorluxe/external-dependency-manager.git?path=Assets/_Root#1.2.169",
```

To `Packages/manifest.json`

## Usage

![1](https://user-images.githubusercontent.com/44673303/157580975-6b5cd124-4196-49f5-9a48-b620bb203f63.png)

#### _BASIC_

1. Auto Init :
    1. `true` if you want Adverstising to automatically initialize setting at `Start()`
    2. `false` you need to call `Advertising.Initialize()` manually where you want
    3. `Advertising.Initialize()` is required to use other Adverstising APIs

2. [GDPR](https://developers.google.com/admob/unity/eu-consent) : General Data Protection Regulation
    - Under the Google EU User Consent Policy, you must make certain disclosures to your users in the European Economic Area (EEA) and obtain their consent to use cookies or other
      local storage, where legally required, and to use personal data (such as AdID) to serve ads. This policy reflects the requirements of the EU ePrivacy Directive and the
      General Data Protection Regulation (GDPR)

    1. `true` the consent popup will be displayed automatically as soon as `GoogleMobileAds Initialize` is successful if you use Admob for show ad, or `MaxSdk.InitializeSdk()`
       initalize completed when you use `max` for show ad
    2. `false` nothing happened
    3. you can call manual consent form by
    ```c#
        if (!GDPRHelper.CheckStatusGDPR())
        {
            Advertising.ShowConsentFrom();
        }
    ```

- Note:
    - You can also call manually by calling through `Advertising.ShowConsentForm()`
    - On android it will show consent form popup,
    - On ios it will show ATT popup

3. Privacy & Policy :
    - the link to the website containing your privacy policy information

#### _AUTO AD-LOADING_

1. Auto Ad-Loading Mode
    1. All : auto load `interstitial ad`, `rewarded ad`, `rewarded interstitial ad`, `app open ad`
2. Ad Checking Interval
    1. ad availability check time. ex: `Ad Checking Interval = 8` checking load ad after each 8 second
3. Ad Loading Interval
    1. time between 2 ad loads. ex: `Ad Loading Interval = 15` the next call to load the ad must be 15 seconds after the previous one

#### _ADMOB_

![2](https://user-images.githubusercontent.com/44673303/157592895-32e01024-3de7-41f4-8823-9a9b996371f2.png)

1. BannerAd:
    1. when size banner is SmartBanner you can choose option use Apdaptive Banner

#### _MAX_

![3](https://user-images.githubusercontent.com/44673303/157606179-7ea14705-175f-4297-bc96-d4516bee50cf.png)

1. Age Restrictd User

    - To ensure COPPA, GDPR, and Google Play policy compliance, you should indicate when a user is a child. If you know that the user is in an age-restricted category (i.e., under
      the age of 16), set the age-restricted user flag to true

    - If you know that the user is not in an age-restricted category (i.e., age 16 or older), set the age-restricted user flag to false

#### _Adverstising_

```c#
Advertising.ShowBannerAd()
Advertising.HideBannerAd()
Advertising.DestroyBannerAd()
Advertising.GetAdaptiveBannerHeight()


Advertising.LoadInsterstitialAd()
Advertising.IsInterstitialAdReady()
Advertising.ShowInterstitialAd()


Advertising.LoadRewardedAd()
Advertising.IsRewardedAdReady()
Advertising.ShowRewardedAd()


Advertising.LoadRewardedInterstitialAd()
Advertising.IsRewardedInterstitialAdReady()
Advertising.ShowRewardedInterstitialAd()


Advertising.LoadAppOpenAd()
Advertising.IsAppOpenAdReady()
Advertising.ShowAppOpenAd()


Advertising.ShowConsentFrom()

```


- you can attach your custom event callback by 
```c#
Action<EInterstitialAdNetwork> InterstitialAdCompletedEvent; // call when user completed watch interstitialAd


Action<ERewardedAdNetwork> RewardedAdCompletedEvent; // call when user completed receive reward form rewardedAd
Action<ERewardedAdNetwork> RewardedAdSkippedEvent; // call when user skip watching rewardedAd


Action<ERewardedInterstitialAdNetwork> RewardedInterstitialAdCompletedEvent; // call when user completed receive reward form rewardedInterstitialAd
Action<ERewardedInterstitialAdNetwork> RewardedInterstitialAdSkippedEvent; // call when user skip watching rewardedInterstitialAd


Action<EAppOpenAdNetwork> AppOpenAdCompletedEvent; // call when user completed watch appOpenAd
```

#### Update current use network
- by default admob will be used to show ad, you can use the following syntax
```c#
Advertising.SetCurrentNetwork("name network");

ex: Advertising.SetCurrentNetwork("applovin");
```
 1. "admob"
 2. "applovin"

