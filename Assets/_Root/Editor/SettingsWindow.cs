using Snorlax.Ads;
using UnityEditor;
using UnityEngine;
using Network = Snorlax.Ads.Network;

namespace Snorlax.AdsEditor
{
    public class SettingsWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private Editor _editor;

        private void OnGUI()
        {
            if (_editor == null) _editor = Editor.CreateEditor(Settings.Instance);

            if (_editor == null)
            {
                EditorGUILayout.HelpBox("Coundn't create the settings resources editor.", MessageType.Error);
                return;
            }

            SettingsEditor.callFromEditorWindow = true;

            _editor.DrawHeader();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.BeginVertical(new GUIStyle {padding = new RectOffset(6, 3, 3, 3)});
            _editor.OnInspectorGUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            SettingsEditor.callFromEditorWindow = false;
        }

        private static SettingsWindow GetWindow()
        {
            // Get the window and make sure it will be opened in the same panel with inspector window.
            var editorAsm = typeof(Editor).Assembly;
            var inspWndType = editorAsm.GetType("UnityEditor.InspectorWindow");
            var window = GetWindow<SettingsWindow>(inspWndType);
            window.titleContent = new GUIContent("Ads");

            return window;
        }

        public static void ShowWindow()
        {
            var window = GetWindow();
            if (window == null)
            {
                Debug.LogError("Coundn't open the ads settings window.");
                return;
            }

            window.minSize = new Vector2(475, 0);

            window.Show();
        }

        private void OnEnable()
        {
            SettingManager.downloadPluginProgressCallback = OnDownloadPluginProgress;
            SettingManager.importPackageCompletedCallback = OnImportPackageCompleted;

            SettingManager.Instance.Load();
        }

        private void OnDisable()
        {
            SettingManager.webRequest?.Abort();
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Callback method that will be called when package import completed
        /// </summary>
        /// <param name="network"></param>
        private static void OnImportPackageCompleted(Network network)
        {
            SettingManager.SetNetworkUnityVersion(network.name, network.lastVersion.unity);
            SettingManager.Instance.UpdateCurrentVersion(network);
            SettingManager.Instance.RemoveMediationExtras(network);
        }

        /// <summary>
        /// Callback method that will be called with progress updates when the plugin is being downloaded.
        /// </summary>
        public static void OnDownloadPluginProgress(string pluginName, float progress, bool done)
        {
            // Download is complete. Clear progress bar.
            if (done)
            {
                EditorUtility.ClearProgressBar();
            }
            // Download is in progress, update progress bar.
            else
            {
                if (EditorUtility.DisplayCancelableProgressBar("Ads", string.Format("Downloading {0} plugin...", pluginName), progress))
                {
                    SettingManager.webRequest?.Abort();
                    EditorUtility.ClearProgressBar();
                }
            }
        }
    }
}