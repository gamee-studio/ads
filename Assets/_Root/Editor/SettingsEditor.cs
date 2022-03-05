using System;
using System.Collections.Generic;
using System.IO;
using Snorlax.Ads;
using UnityEditor;
using UnityEngine;
using Network = Snorlax.Ads.Network;

namespace Snorlax.AdsEditor
{
    [CustomEditor(typeof(Settings))]
    internal class SettingsEditor : Editor
    {
        private class Property
        {
            public SerializedProperty property;
            public GUIContent content;

            public Property(SerializedProperty property, GUIContent content)
            {
                this.property = property;
                this.content = content;
            }

            public Property(GUIContent content) { this.content = content; }
        }

        private static class AdProperties
        {
            public static SerializedProperty main;

            public static Property autoInit = new Property(null, new GUIContent("Auto Init", "Whether the ads should automatically initialize itself"));
            public static Property autoLoadAdsMode = new Property(null, new GUIContent("Auto Ad-Loading Mode"));
            public static Property adCheckingInterval = new Property(null, new GUIContent("Ad Checking Interval", "Time (seconds) between 2 ad-availability checks"));

            public static Property adLoadingInterval = new Property(null,
                new GUIContent("Ad Loading Interval",
                    "Minimum time (seconds) between two ad-loading requests, this is to restrict the number of requests sent to ad networks"));
        }

        private static class AdmobProperties
        {
            public static SerializedProperty main;
            public static Property enable = new Property(null, new GUIContent("Enable", "Enable using admob ad"));
            public static Property devicesTest = new Property(null, new GUIContent("Devices Test", "List devices show real ad but mark test user"));
            public static Property bannerAdUnit = new Property(null, new GUIContent("Banner ad"));
            public static Property interstitialAdUnit = new Property(null, new GUIContent("Interstitial ad"));
            public static Property rewardedAdUnit = new Property(null, new GUIContent("Rewarded Ad"));
            public static Property rewardedInterstitialAdUnit = new Property(null, new GUIContent("Rewarded interstitial ad"));
            public static Property appOpenAdUnit = new Property(null, new GUIContent("App open ad"));
            public static Property enableTestMode = new Property(null, new GUIContent("Enable Test Mode", "Enable true when want show test ad"));
            public static Property useAdaptiveBanner = new Property(null, new GUIContent("Use Adaptive Banner", "Use adaptive banner ad when use smart banner"));
        }

        #region properties

        //Runtime auto initialization
        private SerializedProperty _autoInitializeProperty;
        public static bool callFromEditorWindow = false;

        private const float ACTION_FIELD_WIDTH = 60f;
        private const float NETWORK_FIELD_MIN_WIDTH = 110f;
        private const float VERSION_FIELD_MIN_WIDTH = 100f;
        private static readonly GUILayoutOption NetworkWidthOption = GUILayout.Width(NETWORK_FIELD_MIN_WIDTH);
        private static readonly GUILayoutOption VersionWidthOption = GUILayout.Width(VERSION_FIELD_MIN_WIDTH);
        private static readonly GUILayoutOption FieldWidth = GUILayout.Width(ACTION_FIELD_WIDTH);
        private GUIContent _warningIcon;
        private GUIContent _iconUnintall;
        private GUIStyle _headerLabelStyle;

        #endregion

        #region api

        private void Init()
        {
            _warningIcon = IconContent("console.warnicon.sml", "Adapter not compatible, please update to the latest version.");
            _iconUnintall = IconContent("d_TreeEditor.Trash", "Uninstall");
            _headerLabelStyle = new GUIStyle(EditorStyles.label) {fontSize = 12, fontStyle = FontStyle.Bold, fixedHeight = 18};

            _autoInitializeProperty = serializedObject.FindProperty("runtimeAutoInitialize");

            AdProperties.main = serializedObject.FindProperty("adSettings");
            AdProperties.autoInit.property = AdProperties.main.FindPropertyRelative("autoInit");
            AdProperties.autoLoadAdsMode.property = AdProperties.main.FindPropertyRelative("autoLoadingAd");
            AdProperties.adCheckingInterval.property = AdProperties.main.FindPropertyRelative("adCheckingInterval");
            AdProperties.adLoadingInterval.property = AdProperties.main.FindPropertyRelative("adLoadingInterval");

            AdmobProperties.main = serializedObject.FindProperty("admobSettings");
            AdmobProperties.enable.property = AdmobProperties.main.FindPropertyRelative("enable");
            AdmobProperties.devicesTest.property = AdmobProperties.main.FindPropertyRelative("devicesTest");
            AdmobProperties.bannerAdUnit.property = AdmobProperties.main.FindPropertyRelative("bannerAdUnit");
            AdmobProperties.interstitialAdUnit.property = AdmobProperties.main.FindPropertyRelative("interstitialAdUnit");
            AdmobProperties.rewardedAdUnit.property = AdmobProperties.main.FindPropertyRelative("rewardedAdUnit");
            AdmobProperties.rewardedInterstitialAdUnit.property = AdmobProperties.main.FindPropertyRelative("rewardedInterstitialAdUnit");
            AdmobProperties.appOpenAdUnit.property = AdmobProperties.main.FindPropertyRelative("appOpenAdUnit");
            AdmobProperties.enableTestMode.property = AdmobProperties.main.FindPropertyRelative("enableTestMode");
            AdmobProperties.useAdaptiveBanner.property = AdmobProperties.main.FindPropertyRelative("useAdaptiveBanner");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Init();

            if (!callFromEditorWindow)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox(
                    "This ScriptableObject holds all the settings of Ads. Please go to menu Tools > Snorlax > Ads or click the button below to edit it.",
                    MessageType.Info);
                if (GUILayout.Button("Edit")) SettingsWindow.ShowWindow();
                return;
            }

            EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);

            #region draw

            DrawUppercaseSection("AUTO_INITIALIZE_FOLDOUT_KEY",
                "AUTO INITIALIZE",
                () => { EditorGUILayout.PropertyField(AdProperties.autoInit.property, AdProperties.autoInit.content); });

            EditorGUILayout.Space();
            DrawUppercaseSection("AUTO_AD_LOADING_CONFIG_FOLDOUT_KEY",
                "AUTO AD-LOADING",
                () =>
                {
                    EditorGUILayout.PropertyField(AdProperties.autoLoadAdsMode.property, AdProperties.autoLoadAdsMode.content);
                    if (Settings.AdSettings.AutoLoadingAd != EAutoLoadingAd.None)
                    {
                        EditorGUILayout.PropertyField(AdProperties.adCheckingInterval.property, AdProperties.adCheckingInterval.content);
                        EditorGUILayout.PropertyField(AdProperties.adLoadingInterval.property, AdProperties.adLoadingInterval.content);
                    }
                });

            EditorGUILayout.Space();
            DrawUppercaseSection("ADMOB_MODULE",
                "ADMOB",
                () =>
                {
                    EditorGUILayout.PropertyField(AdmobProperties.enable.property, AdmobProperties.enable.content);
                    if (Settings.AdmobSettings.Enable)
                    {
                        EditorGUILayout.PropertyField(AdmobProperties.bannerAdUnit.property, AdmobProperties.bannerAdUnit.content, true);
                        EditorGUILayout.PropertyField(AdmobProperties.interstitialAdUnit.property, AdmobProperties.interstitialAdUnit.content, true);
                        EditorGUILayout.PropertyField(AdmobProperties.rewardedAdUnit.property, AdmobProperties.rewardedAdUnit.content, true);
                        EditorGUILayout.PropertyField(AdmobProperties.rewardedInterstitialAdUnit.property, AdmobProperties.rewardedInterstitialAdUnit.content, true);
                        EditorGUILayout.PropertyField(AdmobProperties.appOpenAdUnit.property, AdmobProperties.appOpenAdUnit.content, true);

                        if (Settings.AdmobSettings.BannerAdUnit.size == EBannerSize.SmartBanner)
                        {
                            EditorGUILayout.PropertyField(AdmobProperties.useAdaptiveBanner.property, AdmobProperties.useAdaptiveBanner.content);
                        }

                        DrawUppercaseSection("ADMOB_MODULE_MEDIATION",
                            "MEDIATION",
                            () =>
                            {
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    GUILayout.Space(5);
                                    EditorGUILayout.LabelField("Network", _headerLabelStyle, NetworkWidthOption);
                                    EditorGUILayout.LabelField("Current Version", _headerLabelStyle, VersionWidthOption);
                                    GUILayout.Space(3);
                                    EditorGUILayout.LabelField("Latest Version", _headerLabelStyle, VersionWidthOption);
                                    GUILayout.Space(3);
                                    GUILayout.FlexibleSpace();
                                    GUILayout.Button("Actions", _headerLabelStyle, FieldWidth);
                                    GUILayout.Space(5);
                                }

                                foreach (var network in Settings.AdmobSettings.MediationNetworks)
                                {
                                    DrawNetworkDetailRow(network);
                                }
                            });

                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(AdmobProperties.enableTestMode.property, AdmobProperties.enableTestMode.content);
                        if (Settings.AdmobSettings.EnableTestMode)
                        {
                            EditorGUILayout.PropertyField(AdmobProperties.devicesTest.property, AdmobProperties.devicesTest.content);
                        }
                    }
                });

            #endregion

            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNetworkDetailRow(Network network)
        {
            string action;
            string currentVersion = network.currentVersion != null ? network.currentVersion.unity : "";
            string latestVersion = network.lastVersion.unity;
            bool isActionEnabled;
            bool isInstalled;
            if (string.IsNullOrEmpty(currentVersion))
            {
                action = "Install";
                currentVersion = "Not Installed";
                isActionEnabled = true;
                isInstalled = false;
            }
            else
            {
                isInstalled = true;

                var comparison = network.CurrentToLatestVersionComparisonResult;
                // A newer version is available
                if (comparison == EVersionComparisonResult.Lesser)
                {
                    action = "Upgrade";
                    isActionEnabled = true;
                }
                // Current installed version is newer than latest version from DB (beta version)
                else if (comparison == EVersionComparisonResult.Greater)
                {
                    action = "Installed";
                    isActionEnabled = false;
                }
                // Already on the latest version
                else
                {
                    action = "Installed";
                    isActionEnabled = false;
                }
            }

            GUILayout.Space(4);
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandHeight(false)))
            {
                GUILayout.Space(5);
                EditorGUILayout.LabelField(new GUIContent(network.displayName), NetworkWidthOption);
                EditorGUILayout.LabelField(new GUIContent(currentVersion), VersionWidthOption);
                GUILayout.Space(3);
                EditorGUILayout.LabelField(new GUIContent(latestVersion), VersionWidthOption);
                GUILayout.Space(3);
                GUILayout.FlexibleSpace();

                if (network.requireUpdate)
                {
                    GUILayout.Label(_warningIcon);
                }

                GUI.enabled = isActionEnabled && !EditorApplication.isCompiling;
                if (GUILayout.Button(new GUIContent(action), FieldWidth))
                {
                    // Download the plugin.
                    EditorCoroutine.StartCoroutine(SettingManager.Instance.DownloadPlugin(network));
                }

                GUI.enabled = true;
                GUILayout.Space(2);

                GUI.enabled = isInstalled && !EditorApplication.isCompiling;
                if (GUILayout.Button(_iconUnintall))
                {
                    EditorUtility.DisplayProgressBar("Ads", "Deleting " + network.displayName + "...", 0.5f);
                    var pluginRoot = SettingManager.MediationSpecificPluginParentDirectory;
                    foreach (var pluginFilePath in network.pluginFilePath)
                    {
                        if (pluginFilePath.StartsWith("Plugins"))
                        {
                            FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", pluginFilePath));
                            FileUtil.DeleteFileOrDirectory(Path.Combine("Assets", pluginFilePath + ".meta"));
                        }
                        else
                        {
                            FileUtil.DeleteFileOrDirectory(Path.Combine(pluginRoot, pluginFilePath));
                            FileUtil.DeleteFileOrDirectory(Path.Combine(pluginRoot, pluginFilePath + ".meta"));
                        }
                    }

                    SettingManager.RemoveAllEmptyFolder(new DirectoryInfo(pluginRoot));
                    SettingManager.Instance.UpdateCurrentVersion(network);

                    // Refresh UI
                    AssetDatabase.Refresh();
                    EditorUtility.ClearProgressBar();
                }

                GUI.enabled = true;
                GUILayout.Space(5);
            }

            if (isInstalled)
            {
            }
        }

        #endregion

        #region gui

        private readonly Dictionary<string, bool> _uppercaseSectionsFoldoutStates = new Dictionary<string, bool>();
        private static readonly Dictionary<string, GUIStyle> CustomStyles = new Dictionary<string, GUIStyle>();
        private static GUISkin skin;
        private const string SKIN_PATH = "Assets/_Root/GUISkins/";
        private const string UPM_SKIN_PATH = "Packages/com.snorlax.ads/GUISkins/";
        private static GUIStyle uppercaseSectionHeaderExpand;
        private static GUIStyle uppercaseSectionHeaderCollapse;
        private static Texture2D chevronUp;
        private static Texture2D chevronDown;
        private const int CHEVRON_ICON_WIDTH = 10;
        private const int CHEVRON_ICON_RIGHT_MARGIN = 5;

        public static GUIStyle UppercaseSectionHeaderExpand { get { return uppercaseSectionHeaderExpand ??= GetCustomStyle("Uppercase Section Header"); } }

        public static GUIStyle UppercaseSectionHeaderCollapse
        {
            get { return uppercaseSectionHeaderCollapse ??= new GUIStyle(GetCustomStyle("Uppercase Section Header")) {normal = new GUIStyleState()}; }
        }

        public static GUIStyle GetCustomStyle(string styleName)
        {
            if (CustomStyles.ContainsKey(styleName)) return CustomStyles[styleName];

            if (Skin != null)
            {
                var style = Skin.FindStyle(styleName);

                if (style == null) Debug.LogError("Couldn't find style " + styleName);
                else CustomStyles.Add(styleName, style);

                return style;
            }

            return null;
        }

        public static GUISkin Skin
        {
            get
            {
                if (skin != null) return skin;

                const string upmPath = UPM_SKIN_PATH + "Dark.guiskin";
                string path = !File.Exists(Path.GetFullPath(upmPath)) ? SKIN_PATH + "Dark.guiskin" : upmPath;
                skin = AssetDatabase.LoadAssetAtPath(path, typeof(GUISkin)) as GUISkin;

                if (skin == null) Debug.LogError("Couldn't load the GUISkin at " + path);

                return skin;
            }
        }

        public static Texture2D ChevronDown
        {
            get
            {
                if (chevronDown != null) return chevronDown;
                const string upmPath = UPM_SKIN_PATH + "Icons/icon-chevron-down-dark.psd";
                string path = !File.Exists(Path.GetFullPath(upmPath)) ? SKIN_PATH + "Icons/icon-chevron-down-dark.psd" : upmPath;
                chevronDown = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
                return chevronDown;
            }
        }

        public static Texture2D ChevronUp
        {
            get
            {
                if (chevronUp != null) return chevronUp;
                const string upmPath = UPM_SKIN_PATH + "Icons/icon-chevron-up-dark.psd";
                string path = !File.Exists(Path.GetFullPath(upmPath)) ? SKIN_PATH + "Icons/icon-chevron-up-dark.psd" : upmPath;
                chevronUp = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;

                return chevronUp;
            }
        }

        private Texture2D GetChevronIcon(bool foldout) { return foldout ? ChevronUp : ChevronDown; }

        private void DrawUppercaseSection(string key, string sectionName, Action drawer, Texture2D sectionIcon = null, bool defaultFoldout = true)
        {
            if (!_uppercaseSectionsFoldoutStates.ContainsKey(key))
                _uppercaseSectionsFoldoutStates.Add(key, defaultFoldout);

            bool foldout = _uppercaseSectionsFoldoutStates[key];

            EditorGUILayout.BeginVertical(GetCustomStyle("Uppercase Section Box"), GUILayout.MinHeight(foldout ? 30 : 0));

            EditorGUILayout.BeginHorizontal(foldout ? UppercaseSectionHeaderExpand : UppercaseSectionHeaderCollapse);

            // Header label (and button).
            if (GUILayout.Button(sectionName, GetCustomStyle("Uppercase Section Header Label")))
                _uppercaseSectionsFoldoutStates[key] = !_uppercaseSectionsFoldoutStates[key];

            // The expand/collapse icon.
            var buttonRect = GUILayoutUtility.GetLastRect();
            var iconRect = new Rect(buttonRect.x + buttonRect.width - CHEVRON_ICON_WIDTH - CHEVRON_ICON_RIGHT_MARGIN,
                buttonRect.y,
                CHEVRON_ICON_WIDTH,
                buttonRect.height);
            GUI.Label(iconRect, GetChevronIcon(foldout), GetCustomStyle("Uppercase Section Header Chevron"));

            EditorGUILayout.EndHorizontal();

            // Draw the section content.
            if (foldout) GUILayout.Space(5);

            if (foldout && drawer != null) drawer();

            EditorGUILayout.EndVertical();
        }

        private static GUIContent IconContent(string name, string tooltip)
        {
            var builtinIcon = EditorGUIUtility.IconContent(name);
            return new GUIContent(builtinIcon.image, tooltip);
        }

        #endregion
    }
}