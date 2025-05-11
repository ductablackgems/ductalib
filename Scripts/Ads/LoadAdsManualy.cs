using System;
using System.Collections;
using BG_Library.NET;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class LoadAdsManualy : SingletonMono<LoadAdsManualy>
    {
        private bool mrecReady;

        private void Start()
        {
            AdsManager.Event_OnMediationInitComplete += InitMaxComplete;
        }

        private void OnDestroy()
        {
            AdsManager.Event_OnMediationInitComplete -= InitMaxComplete;
        }

        private void InitMaxComplete()
        {
            // AdsManager.InitMrecManually();
            // AdsManager.InitBannerManually();
        }

        public void LoadAdsOther()
        {
            AdsManager.InitAppOpenManually();
            AdsManager.InitInterstitialManually();
            AdsManager.InitRewardManually();
        }
        // private IEnumerator LoadAds()
        // {
        //     yield return new WaitForEndOfFrame();
        //     yield return new WaitUntil(() => AdsManager.IsMrecReady);
        //     Debug.Log("mrec ready==========");
        //     // AdsManager.InitBannerManually();
        //     // AdsManager.InitInterstitialManually();
        //     // AdsManager.InitAppOpenManually();
        //     // AdsManager.InitRewardManually();
        // }
    }
}