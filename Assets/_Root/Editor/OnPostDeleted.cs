using System.IO;
using Snorlax.Ads;
using UnityEditor;

namespace Snorlax.AdsEditor
{
    public class OnPostDeleted : UnityEditor.AssetModificationProcessor
    {
        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            if (Path.GetFileName(assetPath).Equals(Path.GetFileName("GoogleMobileAds.dll")) || assetPath.Equals("Assets/GoogleMobileAds"))
                ScriptingDefinition.RemoveDefineSymbolOnAllPlatforms(AdsUtil.SCRIPTING_DEFINITION_ADMOB);

            return AssetDeleteResult.DidNotDelete;
        }
    }
}