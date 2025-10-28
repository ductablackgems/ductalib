using System;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads.Immersive
{
    public class ImmersiveAdObject : MonoBehaviour
    {
        public string pos;
        public bool showInStart;

#if USE_IMMERSIVE_ADMOB
        private void Start()
        {
            if (showInStart)
                ShowAds();
        }

        public void ShowAds()
        {
            CallAdsManager.ShowImmersive(pos, this.gameObject);
        }

        public void DestroyAd()
        {
            CallAdsManager.DestroyImmersive(pos);
        }
        public void LoadAds()
        {
            CallAdsManager.InitImmersive(pos);
        }
        
#endif
        
    }
}