using UnityEngine;

namespace Snorlax.Ads
{
    // ReSharper disable once InconsistentNaming
    public class GDPRHelper : MonoBehaviour
    {
        public const string GDPR_STORE_KEY = "gdpr_store_key";

        public static int GetValueGDPR() => StorageUtil.GetInt(GDPR_STORE_KEY, -1);

        public static bool CheckStatusGDPR()
        {
            if (GetValueGDPR() == -1)
            {
                return false;
            }

            return true;
        }

        public static void ClearStatusGDPR() { StorageUtil.SetInt(GDPR_STORE_KEY, -1); }

        public void OnButtonAcceptPressed()
        {
            StorageUtil.SetInt(GDPR_STORE_KEY, 0);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void OnButtonClosePressed()
        {
            StorageUtil.SetInt(GDPR_STORE_KEY, 1);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void OnButtonPrivacyPolicyPressed() { Application.OpenURL(Settings.AdSettings.PrivacyPolicyUrl); }
    }
}