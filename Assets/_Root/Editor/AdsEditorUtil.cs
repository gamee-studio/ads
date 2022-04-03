using System.IO;
using UnityEditor;
using UnityEngine;

namespace Snorlax.AdsEditor
{
    public class AdsEditorUtil
    {
        public const string SCRIPTING_DEFINITION_ADMOB = "PANCAKE_ADMOB_ENABLE";
        public const string SCRIPTING_DEFINITION_APPLOVIN = "PANCAKE_MAX_ENABLE";
        public const string SCRIPTING_DEFINITION_IRONSOURCE = "PANCAKE_IRONSOURCE_ENABLE";
        public const string SCRIPTING_DEFINITION_MULTIPLE_DEX = "PANCAKE_MULTIPLE_DEX";
        public const string DEFAULT_FILTER_ADMOB_DLL = "l:gvhp_exportpath-GoogleMobileAds/GoogleMobileAds.dll";
        public const string DEFAULT_FILTER_MAX_MAXSDK = "l:al_max_export_path-MaxSdk/Scripts/MaxSdk.cs";
        public const string DEFAULT_FILTER_IRONSOURCE_SDK = "l:pancake_ironsource_export_path-IronSource/Scripts/IronSource.cs";
        
        private const string MAINTEMPALTE_GRADLE_PATH = "Assets/Plugins/Android/mainTemplate.gradle";
        private const string GRADLETEMPALTE_PROPERTIES_PATH = "Assets/Plugins/Android/gradleTemplate.properties";

        public static void CreateMainTemplateGradle()
        {
            if (File.Exists(MAINTEMPALTE_GRADLE_PATH)) return;
            var maintemplate = (TextAsset) Resources.Load("mainTemplate", typeof(TextAsset));
            string maintemplateData = maintemplate.text;
            var writer = new StreamWriter(MAINTEMPALTE_GRADLE_PATH, false);
            writer.Write(maintemplateData);
            writer.Close();
            AssetDatabase.ImportAsset(MAINTEMPALTE_GRADLE_PATH);
        }

        public static void CreateGradleTemplateProperties()
        {
            if (File.Exists(GRADLETEMPALTE_PROPERTIES_PATH)) return;
            var gradleTemplate = (TextAsset) Resources.Load("gradleTemplate", typeof(TextAsset));
            string maintemplateData = gradleTemplate.text;
            var writer = new StreamWriter(GRADLETEMPALTE_PROPERTIES_PATH, false);
            writer.Write(maintemplateData);
            writer.Close();
            AssetDatabase.ImportAsset(GRADLETEMPALTE_PROPERTIES_PATH);
        }

        public static void DeleteMainTemplateGradle()
        {
            if (!File.Exists(MAINTEMPALTE_GRADLE_PATH)) return;
            FileUtil.DeleteFileOrDirectory(MAINTEMPALTE_GRADLE_PATH);
            FileUtil.DeleteFileOrDirectory(MAINTEMPALTE_GRADLE_PATH + ".meta");
            AssetDatabase.Refresh();
        }

        public static void DeleteGradleTemplateProperties()
        {
            if (!File.Exists(GRADLETEMPALTE_PROPERTIES_PATH)) return;
            FileUtil.DeleteFileOrDirectory(GRADLETEMPALTE_PROPERTIES_PATH);
            FileUtil.DeleteFileOrDirectory(GRADLETEMPALTE_PROPERTIES_PATH + ".meta");
            AssetDatabase.Refresh();
        }
    }
}