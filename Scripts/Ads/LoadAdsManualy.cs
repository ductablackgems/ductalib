using System;
using System.Collections;
using BG_Library.NET;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class LoadAdsManualy : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            StartCoroutine(LoadAds());
        }

        private IEnumerator LoadAds()
        {
            yield return new WaitForEndOfFrame();
            float timeout = 30f;
            float currentTime = 0;
            while (!AdsManager.IsMrecReady && currentTime < timeout)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitUntil(() => AdsManager.IsMrecReady);
            AdsManager.InitBannerManually();
            AdsManager.InitInterstitialManually();
            yield return new WaitUntil(() => AdsManager.IsInterstitialReady);
            AdsManager.InitAppOpenManually();
            yield return new WaitUntil(AdsManager.IsOpenAppReady);
            AdsManager.InitRewardManually();
            yield return new WaitUntil(() => AdsManager.IsRewardedReady);
        }
    }
}