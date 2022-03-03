using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
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
        private const int LEFT = 5;
        private const int RIGHT = 5;
        private const int TOP = 3;
        private const int BOTTOM = 3;


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
            EditorGUILayout.BeginVertical(new GUIStyle {padding = new RectOffset(LEFT, RIGHT, TOP, BOTTOM)});
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

        private static void Load()
        {
            using var curl = new WebClient();
            curl.Headers.Add(HttpRequestHeader.UserAgent, "request");
            const string url = "https://gist.githubusercontent.com/yenmoc/d79936098344befbd8edfa882c17bf20/raw";
            string json = curl.DownloadString(url);
            Settings.AdmobSettings.MediationNetworks = JsonConvert.DeserializeObject<List<Network>>(json);
        }

        public static void ShowWindow()
        {
            var window = GetWindow();
            if (window == null)
            {
                Debug.LogError("Coundn't open the ads settings window.");
                return;
            }

            window.minSize = new Vector2(300, 0);

            Load();
            window.Show();
            
        }
    }
}