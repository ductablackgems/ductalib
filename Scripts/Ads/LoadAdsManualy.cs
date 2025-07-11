﻿using System;
using System.Collections;
using _0.DucTALib.Splash;
using BG_Library.Common;
using BG_Library.NET;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _0.DucLib.Scripts.Ads
{
    public class LoadAdsManualy : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
#if USE_ADMOB_MEDIATION
            BG_Event.AdmobMediation.Mrec.OnAdLoaded += DestroyAds;
#endif
            
        }

        private void OnDestroy()
        {
#if USE_ADMOB_MEDIATION
            BG_Event.AdmobMediation.Mrec.OnAdLoaded -= DestroyAds;
#endif
            
        }

        private void Start()
        {
            StartCoroutine(LoadAds());
        }

        public void DestroyAds(string adUnitId, ResponseInfo info)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName != "Splash")
            {
                AdsManager.DestroyMrec();
            }
            else if (currentSceneName == "Splash" &&
                     (GameSplash.instance.currentStepPanel != null &&
                      GameSplash.instance.currentStepPanel.splashType > SplashType.AgeRight))
            {
                AdsManager.DestroyMrec();
            }
        }

        private IEnumerator LoadAds()
        {
            yield return new WaitForEndOfFrame();
            float timeout = 7;
            float currentTime = 0;
            if (AdsManager.Ins.NetInfor.TypeMediation == 0)
            {
                while (!AdsManager.IsMrecReady && currentTime < timeout)
                {
                    currentTime += Time.deltaTime;
                    yield return null;
                }
            }

            StartCoroutine(LoadOtherAds());
        }

        private IEnumerator LoadOtherAds()
        {
            AdsManager.InitInterstitialManually();
            AdsManager.InitBannerManually();
            yield return new WaitForSeconds(4f);
            // AdsManager.ShowBanner();
            if (NetConfigsSO.Ins.IsInitAOManually)
                AdsManager.InitAppOpenManually();
            yield return new WaitUntil(AdsManager.IsOpenAppReady);
            AdsManager.InitRewardManually();
            yield return new WaitUntil(() => AdsManager.IsRewardedReady);
        }
    }
}