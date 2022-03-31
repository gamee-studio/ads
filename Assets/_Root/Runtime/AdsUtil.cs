using System;
using System.IO;
using System.Linq;
#if PANCAKE_LOCALE
using Snorlax.Locale;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Snorlax.Ads
{
    public class AdsUtil
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


        /// <summary>
        /// Compares its two arguments for order.  Returns <see cref="EVersionComparisonResult.Lesser"/>, <see cref="EVersionComparisonResult.Equal"/>,
        /// or <see cref="EVersionComparisonResult.Greater"/> as the first version is less than, equal to, or greater than the second.
        /// </summary>
        /// <param name="versionA">The first version to be compared.</param>
        /// <param name="versionB">The second version to be compared.</param>
        /// <returns>
        /// <see cref="EVersionComparisonResult.Lesser"/> if versionA is less than versionB.
        /// <see cref="EVersionComparisonResult.Equal"/> if versionA and versionB are equal.
        /// <see cref="EVersionComparisonResult.Greater"/> if versionA is greater than versionB.
        /// </returns>
        public static EVersionComparisonResult CompareVersions(string versionA, string versionB)
        {
            if (versionA.Equals(versionB)) return EVersionComparisonResult.Equal;

            // Check if either of the versions are beta versions. Beta versions could be of format x.y.z-beta or x.y.z-betaX.
            // Split the version string into beta component and the underlying version.
            int piece;
            var isVersionABeta = versionA.Contains("-beta");
            var versionABetaNumber = 0;
            if (isVersionABeta)
            {
                var components = versionA.Split(new[] { "-beta" }, StringSplitOptions.None);
                versionA = components[0];
                versionABetaNumber = int.TryParse(components[1], out piece) ? piece : 0;
            }

            var isVersionBBeta = versionB.Contains("-beta");
            var versionBBetaNumber = 0;
            if (isVersionBBeta)
            {
                var components = versionB.Split(new[] { "-beta" }, StringSplitOptions.None);
                versionB = components[0];
                versionBBetaNumber = int.TryParse(components[1], out piece) ? piece : 0;
            }

            // Now that we have separated the beta component, check if the underlying versions are the same.
            if (versionA.Equals(versionB))
            {
                // The versions are the same, compare the beta components.
                if (isVersionABeta && isVersionBBeta)
                {
                    if (versionABetaNumber < versionBBetaNumber) return EVersionComparisonResult.Lesser;

                    if (versionABetaNumber > versionBBetaNumber) return EVersionComparisonResult.Greater;
                }
                // Only VersionA is beta, so A is older.
                else if (isVersionABeta)
                {
                    return EVersionComparisonResult.Lesser;
                }
                // Only VersionB is beta, A is newer.
                else
                {
                    return EVersionComparisonResult.Greater;
                }
            }

            // Compare the non beta component of the version string.
            var versionAComponents = versionA.Split('.').Select(version => int.TryParse(version, out piece) ? piece : 0).ToArray();
            var versionBComponents = versionB.Split('.').Select(version => int.TryParse(version, out piece) ? piece : 0).ToArray();
            var length = Mathf.Max(versionAComponents.Length, versionBComponents.Length);
            for (var i = 0; i < length; i++)
            {
                var aComponent = i < versionAComponents.Length ? versionAComponents[i] : 0;
                var bComponent = i < versionBComponents.Length ? versionBComponents[i] : 0;

                if (aComponent < bComponent) return EVersionComparisonResult.Lesser;

                if (aComponent > bComponent) return EVersionComparisonResult.Greater;
            }

            return EVersionComparisonResult.Equal;
        }

        public static bool IsInEEA()
        {
#if PANCAKE_LOCALE
            string code = PreciseLocale.GetRegion();
            if (code.Equals("AT") || code.Equals("BE") || code.Equals("BG") || code.Equals("HR") || code.Equals("CY") || code.Equals("CZ") || code.Equals("DK") ||
                code.Equals("EE") || code.Equals("FI") || code.Equals("FR") || code.Equals("DE") || code.Equals("EL") || code.Equals("HU") || code.Equals("IE") ||
                code.Equals("IT") || code.Equals("LV") || code.Equals("LT") || code.Equals("LU") || code.Equals("MT") || code.Equals("NL") || code.Equals("PL") ||
                code.Equals("PT") || code.Equals("RO") || code.Equals("SK") || code.Equals("SI") || code.Equals("ES") || code.Equals("SE") || code.Equals("IS") ||
                code.Equals("LI") || code.Equals("NO"))
            {
                return true;
            }
#endif

            return false;
        }

#if UNITY_EDITOR
        public static void CreateMainTemplateGradle()
        {
            if (File.Exists(MAINTEMPALTE_GRADLE_PATH)) return;
            var maintemplate = (TextAsset)Resources.Load("mainTemplate", typeof(TextAsset));
            string maintemplateData = maintemplate.text;
            var writer = new StreamWriter(MAINTEMPALTE_GRADLE_PATH, false);
            writer.Write(maintemplateData);
            writer.Close();
            AssetDatabase.ImportAsset(MAINTEMPALTE_GRADLE_PATH);
        }

        public static void CreateGradleTemplateProperties()
        {
            if (File.Exists(GRADLETEMPALTE_PROPERTIES_PATH)) return;
            var gradleTemplate = (TextAsset)Resources.Load("gradleTemplate", typeof(TextAsset));
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
#endif
    }
}