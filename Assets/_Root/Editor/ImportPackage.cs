using System.IO;
using UnityEditor;

namespace Pancake.Editor
{
    public class ImportPackage
    {
        private const string MAX_PACKAGE_PATH = "Assets/_Root/Packages/applovin-max-v5.3.1.unitypackage";
        private const string MAX_PACKAGE_UPM_PATH = "Packages/com.gamee.ads/Packages/applovin-max-v5.3.1.unitypackage";

        public static void ImportMax()
        {
            string path = MAX_PACKAGE_PATH;
            if (!File.Exists(path)) path = !File.Exists(Path.GetFullPath(MAX_PACKAGE_UPM_PATH)) ? MAX_PACKAGE_PATH : MAX_PACKAGE_UPM_PATH;
            AssetDatabase.ImportPackage(path, true);
        }
    }
}