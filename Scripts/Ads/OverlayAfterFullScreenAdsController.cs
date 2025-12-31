using System;
using System.Collections;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using BG_Library.NET;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class OverlayAfterFullScreenAdsController : MonoBehaviour
    {
        public ONALayout ONAPos;
        private int currentAdsFullScreen;

        private void Awake()
        {
#if USE_ANDROID_MEDIATION
            AndroidMediationEvent.FullScreenNative.OnAdFullScreenContentClosed += CallOverlayAfterFullScreenAds;

#endif
        }

        private void Start()
        {
            StartCoroutine(LoadAds());
        }

        private IEnumerator LoadAds()
        {
            yield return new WaitUntil(() => CommonRemoteConfig.instance.fetchComplete);
            CallAdsManager.InitONA("OverlayAfterFullScreenAds");
        }

        private void OnDestroy()
        {
#if USE_ANDROID_MEDIATION
            AndroidMediationEvent.FullScreenNative.OnAdFullScreenContentClosed -= CallOverlayAfterFullScreenAds;
#endif
        }

        private void CallOverlayAfterFullScreenAds(string ads)
        {
#if USE_ANDROID_MEDIATION
            currentAdsFullScreen += 1;
            if (currentAdsFullScreen >= CommonRemoteConfig.instance.commonConfig.interstitialsBeforeMRECCount)
            {
                currentAdsFullScreen = 0;
                StartCoroutine(CallOverlayAfterFullScreenAdsIE());
            }
            else LogHelper.CheckPoint($"Interstitial count not reached MREC threshold : {currentAdsFullScreen}");
#endif
        }

        private IEnumerator CallOverlayAfterFullScreenAdsIE()
        {
            LogHelper.CheckPoint("start call overlay after full screen ads");
            yield return new WaitForSecondsRealtime(15f);
            CallAdsManager.ShowONA("OverlayAfterFullScreenAds", ONAPos);
            yield return new WaitForSecondsRealtime(15f);
            CallAdsManager.CloseONA("OverlayAfterFullScreenAds");
        }
    }
}