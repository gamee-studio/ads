using System.IO;
using Snorlax.Ads;
using UnityEditor;
using UnityEngine;

namespace Snorlax.AdsEditor
{
    internal class SettingCreator
    {
        internal static Settings CreateSettingsAsset()
        {
            // Stop if the asset is already created.
            var instance = Settings.LoadSetting();
            if (instance != null)
            {
                return instance;
            }

            // Create Resources folder if it doesn't exist.
            EnsureFolderExists("Assets/Resources");

            // Now create the asset inside the Resources folder.
            instance = Settings.Instance; // this will create a new instance of the EMSettings scriptable.
            AssetDatabase.CreateAsset(instance, "Assets/Resources/AdSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Settings.asset was created at Assets/Resources/AdSettings.asset");

            return instance;
        }

        /// <summary>
        /// Creates the folder if it doesn't exist.
        /// </summary>
        /// <param name="path">Path - the slashes will be corrected.</param>
        private static void EnsureFolderExists(string path)
        {
            path = SlashesToPlatformSeparator(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Replaces / in file path to be the os specific separator.
        /// </summary>
        /// <returns>The path.</returns>
        /// <param name="path">Path with correct separators.</param>
        public static string SlashesToPlatformSeparator(string path) { return path.Replace("/", Path.DirectorySeparatorChar.ToString()); }
    }
}