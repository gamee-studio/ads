# What

Integration module for implementing in-game advertising for Unity 3d

## Support network

1. Admob mediation
2. Applovin mediation (MAX)
3. IronSource

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
"com.gamee.ads": "https://github.com/gamee-studio/ads.git?path=Assets/_Root#1.0.9",
"com.snorlax.locale": "https://github.com/snorluxe/locale.git?path=Assets/_Root#1.0.2",
"com.google.external-dependency-manager": "https://github.com/snorluxe/external-dependency-manager.git?path=Assets/_Root#1.2.169",
```

To `Packages/manifest.json`

## Usage

![1](https://user-images.githubusercontent.com/44673303/161428593-fce3bccd-e05c-435f-b482-7f3a3a68b2ef.png)

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

3. Multi Dex:
   - enable multi dex to fix build gradle error
   
4. Current Network:
   - the ad network currently used to display ads

6. Privacy & Policy : displayed to edit when GDPR enabled
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
or: Advertising.SetCurrentNetwork(EAdNetwork.AppLovin);
```

1. "admob"
2. "applovin"
3. "ironsource"

#### Notes

1. [Setting scripting symbols for Editor script compilation](https://docs.unity3d.com/Manual/CustomScriptingSymbols.html)

```text
If you need to define scripting symbols via scripts in the Editor so that your Editor scripts are affected by the change, you must use PlayerSettings.SetScriptingDefineSymbolsForGroup. However, there are some important details to note about how this operates.

Important: this method does not take immediate effect. Calling this method from script does not immediately apply and recompile your scripts. For your directives to take effect based on a change in scripting symbols, you must allow control to be returned to the Editor, where it then asynchronously reloads the scripts and recompiles them based on your new symbols and the directives which act on them.

So, for example, if you use this method in an Editor script, then immediately call BuildPipeline.BuildPlayer on the following line in the same script, at that point Unity is still running your Editor scripts with the old set of scripting symbols, because they have not yet been recompiled with the new symbols. This means if you have Editor scripts which run as part of your BuildPlayer execution, they run with the old scripting symbols and your player might not build as you expected.
```

2. IronSource SDK
   - In case you have successfully imported ironSOurce but Unity Editor still says plugin not found `IronSource plugin not found. Please import it to show ads from IronSource`
     ![image](https://user-images.githubusercontent.com/44673303/161428343-19750d61-b75e-4f37-a532-2a01a3e379e7.png)
   
     Open ProjectSetting and navigate to Scripting Definition Symbol then remove the line PANCAKE_IRONSOURCE_ENABLE -> wait editor complie and add symbol again
     ![Screenshot_1](https://user-images.githubusercontent.com/44673303/161428348-2e330f02-ca78-4b6d-8f4c-25d539c771b4.png)

3. AppLovin SDK (fixed in version 8.4.1.1)
    - Mediation adapter Chartboost 8.4.1 is crashing and not building on Unity after they updated to Java 11
    ![image (1)](https://user-images.githubusercontent.com/44673303/161477158-1deae20f-ce7c-436a-8e8c-d4c5fe196ed7.png)
    - so you need use old version of Chartboot (8.2.1.0)
    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <dependencies>
        <androidPackages>
            <androidPackage spec="com.applovin.mediation:chartboost-adapter:8.2.1.0" />
            <androidPackage spec="com.google.android.gms:play-services-base:16.1.0" />
        </androidPackages>
        <iosPods>
            <iosPod name="AppLovinMediationChartboostAdapter" version="8.4.2.0" />
        </iosPods>
    </dependencies>
    ```
