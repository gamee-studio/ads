using Pancake.Monetization;
using UnityEditor;

namespace Pancake.Editor
{
    public static class MenuManager
    {
        [MenuItem("Tools/Snorlax/Ads %e", false, 1)]
        public static void MenuOpenSettings()
        {
            // Load settings object or create a new one if it doesn't exist.
            var instance = Settings.LoadSetting();
            if (instance == null) SettingCreator.CreateSettingsAsset();

            SettingsWindow.ShowWindow();
        }
    }
}