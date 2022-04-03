using Snorlax.Ads;
using UnityEditor;

namespace Snorlax.AdsEditor
{
    public class OnPostDeleted : UnityEditor.AssetModificationProcessor
    {
        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            if (!SettingManager.IsAdmobSdkImported())
            {
                ScriptingDefinition.RemoveDefineSymbolOnAllPlatforms(AdsEditorUtil.SCRIPTING_DEFINITION_ADMOB);
            }

            if (!SettingManager.IsMaxSdkImported())
            {
                ScriptingDefinition.RemoveDefineSymbolOnAllPlatforms(AdsEditorUtil.SCRIPTING_DEFINITION_APPLOVIN);
            }

            return AssetDeleteResult.DidNotDelete;
        }
    }
}