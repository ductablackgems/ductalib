using System;
using System.Collections;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using BG_Library.Common;
using BG_Library.NET;
using GoogleMobileAds.Api;
using UnityEngine;

namespace _0.DucTALib.Splash
{
    public class LoadAdsManually : SingletonMono<LoadAdsManually>
    {
        private bool eventAdded = false;
        private void Start()
        {
            DontDestroyOnLoad(this);
            BG_Event.AdmobMediation.Mrec.OnAdLoaded += MRECLoadDone;
        }

        public void AddEndCardEvent()
        {
            if(eventAdded) return;
            eventAdded = true;
            AndroidMediationEvent.FullScreenNative.OnAdFullScreenContentClosed += CallEndCard;
            CallAdsManager.InitONA("endcard");
        }
        private void OnDestroy()
        {
            BG_Event.AdmobMediation.Mrec.OnAdLoaded -= MRECLoadDone;
            AndroidMediationEvent.FullScreenNative.OnAdFullScreenContentClosed -= CallEndCard;
        }


        public void CallEndCard(string groupName)
        {
            StartCoroutine(CallEndCardFullScreen());
        }

        private IEnumerator CallEndCardFullScreen()
        {
            yield return new WaitForSecondsRealtime(15f);
            CallAdsManager.ShowONA("endcard");
        }

        private void MRECLoadDone(string a, ResponseInfo info)
        {
            LogHelper.CheckPoint();
            if (AdsManager.ScreenName == "")
                AdsManager.HideMrec();
        }

        public static void LoadBanner()
        {
            AdsManager.InitBannerManually();
        }

        public static void LoadMrec()
        {
            LogHelper.CheckPoint();
            AdsManager.InitMrecManually();
        }

        public static void LoadInterByGroup(string group)
        {
            LogHelper.CheckPoint($"load inter group {group}");
            AdsManager.InitInterstitialManually(group);
        }

        public static void LoadReward()
        {
            LogHelper.CheckPoint();
            AdsManager.InitRewardManually();
        }
    }
}