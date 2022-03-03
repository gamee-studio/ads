using System;

namespace Snorlax.Ads
{
    [Serializable]
    public class Network
    {
        public string name;
        public string displayName;
        public string[] versions;
        public string path;
        public string lastVersion;
        [NonSerialized] public string currentVersion;
        [NonSerialized] public EVersionComparisonResult CurrentToLatestVersionComparisonResult = EVersionComparisonResult.Lesser;
        [NonSerialized] public bool requireUpdate;
    }
}