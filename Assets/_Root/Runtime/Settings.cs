using UnityEngine;

namespace Pancake.Monetization
{
    public class Settings : ScriptableObject
    {
        private static Settings instance;

        public static Settings Instance
        {
            get
            {
                if (instance != null) return instance;

                instance = LoadSetting();

                if (instance == null)
                {
#if !UNITY_EDITOR
                        Debug.LogError("Ads settings not found! Please go to menu Tools > Snorlax > Ads to setup the plugin.");
#endif
                    instance = CreateInstance<Settings>(); // Create a dummy scriptable object for temporary use.
                }

                return instance;
            }
        }

        #region member

        [SerializeField] private bool runtimeAutoInitialize = true;
        [SerializeField] private AdSettings adSettings = new AdSettings();
        [SerializeField] private AdmobSettings admobSettings = new AdmobSettings();
        [SerializeField] private MaxSettings maxSettings = new MaxSettings();
        [SerializeField] private IronSourceSettings ironSourceSettings = new IronSourceSettings();

        #endregion

        #region properties

        public static bool RuntimeAutoInitialize => Instance.runtimeAutoInitialize;

        public static AdSettings AdSettings => Instance.adSettings;

        public static AdmobSettings AdmobSettings => Instance.admobSettings;

        public static MaxSettings MaxSettings => Instance.maxSettings;
        public static IronSourceSettings IronSourceSettings => Instance.ironSourceSettings;

        public static EAdNetwork CurrentNetwork => Instance.adSettings.CurrentNetwork;

        #endregion

        #region api

        public static Settings LoadSetting() { return Resources.Load<Settings>("AdSettings"); }

        #endregion
    }
}