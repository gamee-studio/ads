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
        public string dependenciesFilePath;
        public string[] pluginFilePath;
        public NetworkVersion lastVersion;
        [NonSerialized] public NetworkVersion currentVersion;
        [NonSerialized] public EVersionComparisonResult CurrentToLatestVersionComparisonResult = EVersionComparisonResult.Lesser;
        [NonSerialized] public bool requireUpdate;
    }

    [Serializable]
    public class NetworkVersion
    {
        public string android;
        public string ios;
        public string unity;

        public NetworkVersion()
        {
            android = "";
            ios = "";
            unity = "";
        }

        public override bool Equals(object value)
        {
            return value is NetworkVersion versions && unity.Equals(versions.unity) && (android == null || android.Equals(versions.android)) &&
                   (ios == null || ios.Equals(versions.ios));
        }

        public bool HasEqualSdkVersions(NetworkVersion versions)
        {
            return versions != null && AdapterSdkVersion(android).Equals(AdapterSdkVersion(versions.android)) &&
                   AdapterSdkVersion(ios).Equals(AdapterSdkVersion(versions.ios));
        }

        public override int GetHashCode() { return new { unity, android, ios }.GetHashCode(); }

        private static string AdapterSdkVersion(string adapterVersion)
        {
            var index = adapterVersion.LastIndexOf(".");
            return index > 0 ? adapterVersion.Substring(0, index) : adapterVersion;
        }
    }
}