using System;
using UnityEngine;

namespace Snorlax.Ads
{
    [Serializable]
    public class AdSettings
    {
        [SerializeField] private bool autoInit = true;
        [SerializeField] private EAutoLoadingAd autoLoadingAd = EAutoLoadingAd.All;
        [Range(5, 100)] [SerializeField] private float adCheckingInterval = 8f;

        [Range(5, 100)] [SerializeField] private float adLoadingInterval = 15f;

        public bool AutoInit { get => autoInit; set => autoInit = value; }

        public float AdCheckingInterval { get => adCheckingInterval; set => adCheckingInterval = value; }

        public float AdLoadingInterval { get => adLoadingInterval; set => adLoadingInterval = value; }

        public EAutoLoadingAd AutoLoadingAd { get => autoLoadingAd; set => autoLoadingAd = value; }
    }
}