using System;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads.Immersive
{
    public class ImmersiveAdObject : MonoBehaviour
    {
        public string pos;

        private void Start()
        {
            ShowAds();
        }

        public void ShowAds()
        {
            CallAdsManager.ShowImmersive(pos, this.gameObject);
        }
    }
}