#if PANCAKE_IRONSOURCE_ENABLE
using UnityEngine;

namespace Snorlax.Ads
{
    public class IronSourceStateHandler : MonoBehaviour
    {
        private void OnApplicationPause(bool pause) { IronSource.Agent.onApplicationPause(pause); }
    }
}
#endif