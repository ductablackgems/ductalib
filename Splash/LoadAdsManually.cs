using System;
using System.Collections;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using BG_Library.Common;
using BG_Library.NET;
using GoogleMobileAds.Api;
using UnityEngine;

namespace _0.DucTALib.Splash
{
    public class LoadAdsManually : SingletonMono<LoadAdsManually>
    {
        private bool eventAdded = false;
        private int currentInter = 0;
        private void Start()
        {
            DontDestroyOnLoad(this);
            BG_Event.AdmobMediation.Mrec.OnAdLoaded += MRECLoadDone;
        }

        public void AddEndCardEvent()
        {
#if UNITY_ANDROID
            if (eventAdded) return;
            eventAdded = true;
            AndroidMediationEvent.FullScreenNative.OnAdFullScreenContentClosed += CallEndCard;
            CallAdsManager.InitONA("endcard");
#endif
        }

        private void OnDestroy()
        {
            BG_Event.AdmobMediation.Mrec.OnAdLoaded -= MRECLoadDone;
#if UNITY_ANDROID
            AndroidMediationEvent.FullScreenNative.OnAdFullScreenContentClosed -= CallEndCard;
#endif
        }


        private void CallEndCard(string groupName)
        {
#if UNITY_ANDROID
            currentInter += 1;
            if (currentInter >= CommonRemoteConfig.ins.androidConfig.interstitialsBeforeMRECCount)
            {
                currentInter = 0;
                StartCoroutine(CallEndCardFullScreen());
            }
            else LogHelper.CheckPoint($"Interstitial count not reached MREC threshold : {currentInter}");
#endif
        }

        private IEnumerator CallEndCardFullScreen()
        {
            yield return new WaitForSecondsRealtime(15f);
            CallAdsManager.ShowONA("endcard");
        } 

        private void MRECLoadDone(string a, ResponseInfo info)
        {
            LogHelper.CheckPoint();
            if (CallAdsManager.sceneName == "")
                AdsManager.HideMrec();
        }

    }
}