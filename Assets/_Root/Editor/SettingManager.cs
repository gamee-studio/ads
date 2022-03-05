using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Snorlax.Ads;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Network = Snorlax.Ads.Network;

namespace Snorlax.AdsEditor
{
    /// <summary>
    /// Delegate to be called when downloading a plugin with the progress percentage. 
    /// </summary>
    /// <param name="pluginName">The name of the plugin being downloaded.</param>
    /// <param name="progress">Percentage downloaded.</param>
    /// <param name="done">Whether or not the download is complete.</param>
    public delegate void DownloadPluginProgressCallback(string pluginName, float progress, bool done);

    /// <summary>
    /// Delegate to be called when a plugin package is imported.
    /// </summary>
    /// <param name="network">The network data for which the package is imported.</param>
    public delegate void ImportPackageCompletedCallback(Network network);

    public class SettingManager
    {
        private static readonly SettingManager instance = new SettingManager();
        public static SettingManager Instance => instance;
        public static UnityWebRequest webRequest;
        public static readonly string DefaultPluginExportPath = Path.Combine("Assets", "GoogleMobileAds");
        private const string DEFAULT_ADMOB_SDK_ASSET_EXPORT_PATH = @"GoogleMobileAds\GoogleMobileAds.dll";
        private static readonly string AdmobSdkAssetExportPath = Path.Combine("GoogleMobileAds", "GoogleMobileAds.dll");
        public static DownloadPluginProgressCallback downloadPluginProgressCallback;
        public static ImportPackageCompletedCallback importPackageCompletedCallback;

        private static readonly List<string> PluginPathsToIgnoreMoveWhenPluginOutsideAssetsDirectory = new List<string>();

        public static bool IsPluginOutsideAssetsDirectory => !PluginParentDirectory.StartsWith("Assets");

        public static string PluginParentDirectory
        {
            get
            {
                // Search for the asset with the default exported path first, In most cases, we should be able to find the asset.
                // In some cases where we don't, use the platform specific export path to search for the asset (in case of migrating a project from Windows to Mac or vice versa).
                var admobSdkScriptAssetPath = GetAssetPathForExportPath(DEFAULT_ADMOB_SDK_ASSET_EXPORT_PATH);
                if (File.Exists(admobSdkScriptAssetPath))
                {
                    // admobSdkScriptAssetPath will always have AltDirectorySeparatorChar (/) as the path separator. Convert to platform specific path.
                    return admobSdkScriptAssetPath
                        .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
                        .Replace(DEFAULT_ADMOB_SDK_ASSET_EXPORT_PATH, "")
                        .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                }

                // We should never reach this line but leaving this in out of paranoia.
                return GetAssetPathForExportPath(AdmobSdkAssetExportPath)
                    .Replace(AdmobSdkAssetExportPath, "")
                    .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }
        }

        /// <summary>
        /// When the base plugin is outside the <c>Assets/</c> directory, the mediation plugin files are still imported to the default location under <c>Assets/</c>.
        /// Returns the parent directory where the mediation adapter plugins are imported.
        /// </summary>
        public static string MediationSpecificPluginParentDirectory => IsPluginOutsideAssetsDirectory ? "Assets" : PluginParentDirectory;

        public SettingManager()
        {
            AssetDatabase.importPackageCompleted += packageName =>
            {
                if (!IsImportingNetwork(packageName)) return;

                var pluginParentDir = PluginParentDirectory;
                var isPluginOutsideAssetsDir = IsPluginOutsideAssetsDirectory;
                MovePluginFilesIfNeeded(pluginParentDir, isPluginOutsideAssetsDir);
                AddLabelsToAssetsIfNeeded(pluginParentDir, isPluginOutsideAssetsDir);
                AssetDatabase.Refresh();

                CallImportPackageCompletedCallback(Settings.AdmobSettings.importingNetwork);
                Settings.AdmobSettings.importingNetwork = null;
            };

            AssetDatabase.importPackageCancelled += packageName =>
            {
                if (!IsImportingNetwork(packageName)) return;

                Settings.AdmobSettings.importingNetwork = null;
            };

            AssetDatabase.importPackageFailed += (packageName, errorMessage) =>
            {
                if (!IsImportingNetwork(packageName)) return;

                Debug.LogError(errorMessage);
                Settings.AdmobSettings.importingNetwork = null;
            };
        }

        private static void CallImportPackageCompletedCallback(Network network)
        {
            if (importPackageCompletedCallback == null) return;

            importPackageCompletedCallback(network);
        }

        /// <summary>
        /// Adds labels to assets so that they can be easily found.
        /// </summary>
        /// <param name="pluginParentDir">The GoogleMobileAds Unity plugin's parent directory.</param>
        /// <param name="isPluginOutsideAssetsDirectory">Whether or not the plugin is outside the Assets directory.</param>
        public static void AddLabelsToAssetsIfNeeded(string pluginParentDir, bool isPluginOutsideAssetsDirectory)
        {
            if (isPluginOutsideAssetsDirectory)
            {
                var defaultPluginLocation = Path.Combine("Assets", "GoogleMobileAds");
                if (Directory.Exists(defaultPluginLocation))
                {
                    AddLabelsToAssets(defaultPluginLocation, "Assets");
                }
            }

            var pluginDir = Path.Combine(pluginParentDir, "GoogleMobileAds");
            AddLabelsToAssets(pluginDir, pluginParentDir);
        }

        private static void AddLabelsToAssets(string directoryPath, string pluginParentDir)
        {
            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                if (file.EndsWith(".meta")) continue;

                UpdateAssetLabelsIfNeeded(file, pluginParentDir);
            }

            var directories = Directory.GetDirectories(directoryPath);
            foreach (var directory in directories)
            {
                // Add labels to this directory asset.
                UpdateAssetLabelsIfNeeded(directory, pluginParentDir);

                // Recursively add labels to all files under this directory.
                AddLabelsToAssets(directory, pluginParentDir);
            }
        }

        private static void UpdateAssetLabelsIfNeeded(string assetPath, string pluginParentDir)
        {
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            var labels = AssetDatabase.GetLabels(asset);

            var labelsToAdd = labels.ToList();
            var didAddLabels = false;
            if (!labels.Contains("Al_admob"))
            {
                labelsToAdd.Add("Al_admob");
                didAddLabels = true;
            }

            var exportPathLabel = "Al_admob_export_path-" + assetPath.Replace(pluginParentDir, "");
            if (!labels.Contains(exportPathLabel))
            {
                labelsToAdd.Add(exportPathLabel);
                didAddLabels = true;
            }

            // We only need to set the labels if they changed.
            if (!didAddLabels) return;

            AssetDatabase.SetLabels(asset, labelsToAdd.ToArray());
        }

        /// <summary>
        /// Moves the imported plugin files to the GoogleMobileAds directory if the publisher has moved the plugin to a different directory. This is a failsafe for when some plugin files are not imported to the new location.
        /// </summary>
        /// <returns>True if the adapters have been moved.</returns>
        public static bool MovePluginFilesIfNeeded(string pluginParentDirectory, bool isPluginOutsideAssetsDirectory)
        {
            var pluginDir = Path.Combine(pluginParentDirectory, "GoogleMobileAds");

            // Check if the user has moved the Plugin and if new assets have been imported to the default directory.
            if (DefaultPluginExportPath.Equals(pluginDir) || !Directory.Exists(DefaultPluginExportPath)) return false;

            MovePluginFiles(DefaultPluginExportPath, pluginDir, isPluginOutsideAssetsDirectory);
            if (!isPluginOutsideAssetsDirectory)
            {
                FileUtil.DeleteFileOrDirectory(DefaultPluginExportPath + ".meta");
            }

            AssetDatabase.Refresh();
            return true;
        }

        /// <summary>
        /// Gets the path of the asset in the project for a given GoogleMobileAds plugin export path.
        /// ex : Al_admob_export_path-GoogleMobileAds\GoogleMobileAds.dll
        /// </summary>
        /// <param name="exportPath">The actual exported path of the asset.</param>
        /// <returns>The exported path of the MAX plugin asset or the default export path if the asset is not found.</returns>
        public static string GetAssetPathForExportPath(string exportPath)
        {
            var defaultPath = Path.Combine("Assets", exportPath);
            var assetGuids = AssetDatabase.FindAssets("l:Al_admob_export_path-" + exportPath);

            return assetGuids.Length < 1 ? defaultPath : AssetDatabase.GUIDToAssetPath(assetGuids[0]);
        }

        /// <summary>
        /// A helper function to move all the files recursively from the default plugin dir to a custom location the publisher moved the plugin to.
        /// </summary>
        private static void MovePluginFiles(string fromDirectory, string pluginRoot, bool isPluginOutsideAssetsDirectory)
        {
            var files = Directory.GetFiles(fromDirectory);
            foreach (var file in files)
            {
                // We have to ignore some files, if the plugin is outside the Assets/ directory.
                if (isPluginOutsideAssetsDirectory &&
                    PluginPathsToIgnoreMoveWhenPluginOutsideAssetsDirectory.Any(pluginPathsToIgnore => file.Contains(pluginPathsToIgnore))) continue;

                // Check if the destination folder exists and create it if it doesn't exist
                var parentDirectory = Path.GetDirectoryName(file);
                var destinationDirectoryPath = parentDirectory.Replace(DefaultPluginExportPath, pluginRoot);
                if (!Directory.Exists(destinationDirectoryPath))
                {
                    Directory.CreateDirectory(destinationDirectoryPath);
                }

                // If the meta file is of a folder asset and doesn't have labels (it is auto generated by Unity), just delete it.
                if (IsAutoGeneratedFolderMetaFile(file))
                {
                    FileUtil.DeleteFileOrDirectory(file);
                    continue;
                }

                var destinationPath = file.Replace(DefaultPluginExportPath, pluginRoot);

                // Check if the file is already present at the destination path and delete it.
                if (File.Exists(destinationPath))
                {
                    FileUtil.DeleteFileOrDirectory(destinationPath);
                }

                FileUtil.MoveFileOrDirectory(file, destinationPath);
            }

            var directories = Directory.GetDirectories(fromDirectory);
            foreach (var directory in directories)
            {
                // We might have to ignore some directories, if the plugin is outside the Assets/ directory.
                if (isPluginOutsideAssetsDirectory &&
                    PluginPathsToIgnoreMoveWhenPluginOutsideAssetsDirectory.Any(pluginPathsToIgnore => directory.Contains(pluginPathsToIgnore))) continue;

                MovePluginFiles(directory, pluginRoot, isPluginOutsideAssetsDirectory);
            }

            if (!isPluginOutsideAssetsDirectory)
            {
                FileUtil.DeleteFileOrDirectory(fromDirectory);
            }
        }

        private static bool IsAutoGeneratedFolderMetaFile(string assetPath)
        {
            // Check if it is a meta file.
            if (!assetPath.EndsWith(".meta")) return false;

            var lines = File.ReadAllLines(assetPath);
            var isFolderAsset = false;
            var hasLabels = false;
            foreach (var line in lines)
            {
                if (line.Contains("folderAsset: yes"))
                {
                    isFolderAsset = true;
                }

                if (line.Contains("labels:"))
                {
                    hasLabels = true;
                }
            }

            // If it is a folder asset and doesn't have a label, the meta file is auto generated by 
            return isFolderAsset && !hasLabels;
        }

        private bool IsImportingNetwork(string packageName)
        {
            // Note: The pluginName doesn't have the '.unitypacakge' extension included in its name but the pluginFileName does. So using Contains instead of Equals.
            return Settings.AdmobSettings.importingNetwork != null && GetPluginFileName(Settings.AdmobSettings.importingNetwork).Contains(packageName);
        }

        private string GetPluginFileName(Network network) { return $"GoogleMobileAds{network.displayName}Mediation.unitypackage"; }

        public void Load()
        {
            using var curl = new WebClient();
            curl.Headers.Add(HttpRequestHeader.UserAgent, "request");
            const string url = "https://gist.githubusercontent.com/yenmoc/d79936098344befbd8edfa882c17bf20/raw";
            string json = curl.DownloadString(url);
            Settings.AdmobSettings.MediationNetworks = JsonConvert.DeserializeObject<List<Network>>(json);
            foreach (var n in Settings.AdmobSettings.MediationNetworks)
            {
                UpdateCurrentVersion(n);
            }
        }

        public IEnumerator DownloadPlugin(Network network)
        {
            string pathFile = Path.Combine(Application.temporaryCachePath, $"{network.name.ToLowerInvariant()}_{network.lastVersion.unity}.zip");
            string urlDownload = string.Format(network.path, network.lastVersion.unity);
            var downloadHandler = new DownloadHandlerFile(pathFile);
            webRequest = new UnityWebRequest(urlDownload) {method = UnityWebRequest.kHttpVerbGET, downloadHandler = downloadHandler};
            var operation = webRequest.SendWebRequest();

            static void CallDownloadPluginProgressCallback(string pluginName, float progress, bool isDone)
            {
                if (downloadPluginProgressCallback == null) return;

                downloadPluginProgressCallback(pluginName, progress, isDone);
            }

            while (!operation.isDone)
            {
                yield return new WaitForSeconds(0.1f); // Just wait till webRequest is completed. Our coroutine is pretty rudimentary.
                CallDownloadPluginProgressCallback(network.displayName, operation.progress, operation.isDone);
            }

#if UNITY_2020_1_OR_NEWER
            if (webRequest.result != UnityWebRequest.Result.Success)
#elif UNITY_2017_2_OR_NEWER
            if (webRequest.isNetworkError || webRequest.isHttpError)
#else
            if (webRequest.isError)
#endif
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Settings.AdmobSettings.importingNetwork = network;

                string folderUnZip = Path.Combine(Application.temporaryCachePath, "UnZip");
                UnZip(folderUnZip, File.ReadAllBytes(pathFile));

                AssetDatabase.ImportPackage(Path.Combine(folderUnZip, $"{network.displayName}UnityAdapter-{network.lastVersion.unity}", GetPluginFileName(network)),
                    true);
            }

            webRequest = null;
        }

        /// <summary>
        /// Write the given bytes data under the given filePath. 
        /// The filePath should be given with its path and filename. (e.g. c:/tmp/test.zip)
        /// </summary>
        private static void UnZip(string filePath, byte[] data)
        {
            using (var s = new ZipInputStream(new MemoryStream(data)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        var dirPath = Path.Combine(filePath, directoryName);

                        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                    }

                    if (fileName != string.Empty)
                    {
                        // retrieve directory name only from persistence data path.
                        var entryFilePath = Path.Combine(filePath, theEntry.Name);
                        using (var streamWriter = File.Create(entryFilePath))
                        {
                            var size = 2048;
                            var fdata = new byte[size];
                            while (true)
                            {
                                size = s.Read(fdata, 0, fdata.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(fdata, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                } //end of while
            } //end of using
        }

        public void UpdateCurrentVersion(Network network)
        {
            var dependencyFilePath = Path.Combine(PluginParentDirectory, network.dependenciesFilePath);
            var currentVersion = GetCurrentVersion(dependencyFilePath, network.name);
            network.currentVersion = currentVersion;
            SetNetworkUnityVersion(network.name, network.currentVersion.unity);

            try
            {
                var unityVersionComparison = AdsUtil.CompareVersions(network.currentVersion.unity, network.lastVersion.unity);
                var androidVersionComparison = AdsUtil.CompareVersions(network.currentVersion.android, network.lastVersion.android);
                var iosVersionComparison = AdsUtil.CompareVersions(network.currentVersion.ios, network.lastVersion.ios);

                // Overall version is same if all the current and latest (from db) versions are same.
                if (unityVersionComparison == EVersionComparisonResult.Equal && androidVersionComparison == EVersionComparisonResult.Equal &&
                    iosVersionComparison == EVersionComparisonResult.Equal)
                {
                    network.CurrentToLatestVersionComparisonResult = EVersionComparisonResult.Equal;
                }
                // One of the installed versions is newer than the latest versions which means that the publisher is on a beta version.
                else if (unityVersionComparison == EVersionComparisonResult.Greater || androidVersionComparison == EVersionComparisonResult.Greater ||
                         iosVersionComparison == EVersionComparisonResult.Greater)
                {
                    network.CurrentToLatestVersionComparisonResult = EVersionComparisonResult.Greater;
                }
                // We have a new version available if all Android, iOS and Unity has a newer version available in db.
                else
                {
                    network.CurrentToLatestVersionComparisonResult = EVersionComparisonResult.Lesser;
                }
            }
            catch (Exception)
            {
                Debug.Log("S");
            }
        }

        /// <summary>
        /// since MediationExtras class is already added in Admob package
        /// so we don't need MediationExtras class from importing mediation package anymore it will be confusing
        /// </summary>
        public void RemoveMediationExtras(Network network)
        {
            if (network.name.Equals("ADCOLONY_NETWORK") || network.name.Equals("VUNGLE_NETWORK"))
            {
                FileUtil.DeleteFileOrDirectory(Path.Combine(MediationSpecificPluginParentDirectory, "GoogleMobileAds/Api/Mediation/MediationExtras.cs"));
                FileUtil.DeleteFileOrDirectory(Path.Combine(MediationSpecificPluginParentDirectory, "GoogleMobileAds/Api/Mediation/MediationExtras.cs.meta"));
                AssetDatabase.Refresh();
            }
        }

        private NetworkVersion GetCurrentVersion(string dependencyFilePath, string nameNetwork)
        {
            XDocument dependency;
            try
            {
                dependency = XDocument.Load(dependencyFilePath);
            }
            catch (Exception)
            {
                return new NetworkVersion();
            }

            string androidVersion = null;
            string iosVersion = null;
            var dependenciesElement = dependency.Element("dependencies");
            if (dependenciesElement != null)
            {
                var androidPackages = dependenciesElement.Element("androidPackages");
                if (androidPackages != null)
                {
                    var adapterPackage = androidPackages.Descendants()
                        .FirstOrDefault(element =>
                            element.Name.LocalName.Equals("androidPackage") && element.FirstAttribute.Name.LocalName.Equals("spec") &&
                            element.FirstAttribute.Value.StartsWith("com.google.ads"));
                    if (adapterPackage != null)
                    {
                        androidVersion = adapterPackage.FirstAttribute.Value.Split(':').Last();
                        // Hack alert: Some Android versions might have square brackets to force a specific version. Remove them if they are detected.
                        if (androidVersion.StartsWith("["))
                        {
                            androidVersion = androidVersion.Trim('[', ']');
                        }
                    }
                }

                var iosPods = dependenciesElement.Element("iosPods");
                if (iosPods != null)
                {
                    var adapterPod = iosPods.Descendants()
                        .FirstOrDefault(element =>
                            element.Name.LocalName.Equals("iosPod") && element.FirstAttribute.Name.LocalName.Equals("name") &&
                            element.FirstAttribute.Value.StartsWith("GoogleMobileAds"));
                    if (adapterPod != null)
                    {
                        iosVersion = adapterPod.Attributes().First(attribute => attribute.Name.LocalName.Equals("version")).Value;
                    }
                }
            }

            var currentVersion = new NetworkVersion();
            if (!string.IsNullOrEmpty(androidVersion) && !string.IsNullOrEmpty(iosVersion))
            {
                currentVersion.android = androidVersion;
                currentVersion.ios = iosVersion;
            }

            currentVersion.unity = GetNetworkUnityVersion(nameNetwork);

            return currentVersion;
        }

        private static bool GetEmptyDirectories(DirectoryInfo dir, List<DirectoryInfo> results)
        {
            var isEmpty = true;
            try
            {
                isEmpty = dir.GetDirectories().Count(x => !GetEmptyDirectories(x, results)) == 0 // Are sub directories empty?
                          && dir.GetFiles("*.*").All(x => x.Extension == ".meta"); // No file exist?
            }
            catch
            {
            }

            // Store empty directory to results.
            if (isEmpty) results.Add(dir);

            return isEmpty;
        }

        public static void RemoveAllEmptyFolder(DirectoryInfo dir)
        {
            var result = new List<DirectoryInfo>();
            GetEmptyDirectories(dir, result);

            if (result.Count > 0)
            {
                foreach (var d in result)
                {
                    FileUtil.DeleteFileOrDirectory(d.FullName);
                    FileUtil.DeleteFileOrDirectory(d.Parent + "\\" + d.Name + ".meta"); // unity 2020.2 need to delete the meta too
                }

                AssetDatabase.Refresh();
            }
        }

        public static void SetNetworkUnityVersion(string name, string version) { EditorPrefs.SetString($"{Application.identifier}_ads_{name}_unity", version); }
        public static string GetNetworkUnityVersion(string name) { return EditorPrefs.GetString($"{Application.identifier}_ads_{name}_unity"); }
    }
}