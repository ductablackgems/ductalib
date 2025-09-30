using System;
using System.Collections;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Scripts.Loading;
using BG_Library.NET;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _0.DucTALib.Splash.Scripts
{
    public class SimpleSplash : SingletonMono<SimpleSplash>
    {
        private float loadDuration;
        private float currentTime;
        private float currentProgress;
        private float targetProgress;
        private float stopTimer = 0f;
        private float smoothSpeed = 0.02f;
        private int currentMessageIndex;
        public GameObject loading;
        public Image loadingBar;
        public TextMeshProUGUI loadingText;
        public TextMeshProUGUI currentProgressTxt;

        [ReadOnly] public string[] loadingTxt = new string[]
        {
            "Checking network connection...",
            "Loading user profile data...",
            "Initializing game assets...",
            "Fetching remote configurations...",
            "Initializing Firebase services...",
            "Loading ads configuration...",
            "Preparing game environment...",
            "Syncing player data...",
            "Setting up audio manager...",
            "Loading game resources...",
            "Initializing leaderboard...",
            "Establishing server connection...",
            "Loading in-game tutorials...",
            "Setting up user preferences...",
            "Finalizing game setup..."
        };

        private void Start()
        {
            SplashTracking.loading_duration.Reset();
            SplashTracking.loading_duration.Start();
            StartCoroutine(AdsControl());
            StartCoroutine(WaitToLoadScene());
        }


        private IEnumerator AdsControl()
        {
            yield return new WaitUntil(() => RemoteConfig.Ins.isDataFetched);
            CallAdsManager.InitBannerLoading();
            LoadAdsManually.LoadInterByGroup("launch");
            yield return new WaitUntil(CallAdsManager.BannerLoadingReady);
            CallAdsManager.ShowBanner();
            loading.HideObject();
        }

        private IEnumerator WaitToLoadScene()
        {
            yield return new WaitForEndOfFrame();

            float fbTimeout = 5;
            while (fbTimeout > 0 && (RemoteConfig.Ins == null || RemoteConfig.Ins.isDataFetched))
            {
                fbTimeout -= Time.deltaTime;
                yield return null;
            }

            loadDuration = 15f;
            loadingBar.fillAmount = 0;
            currentProgressTxt.text = $"0%";
            while (currentTime < loadDuration)
            {
                HandleLoadingProgress();
                yield return null;
            }

            FinishLoadingPhase();
            AppOpenCaller.IgnoreAppOpenResume = true;
            SplashTracking.SetUserProperty();
            CallAdsManager.ShowInter("launch");
            FinishLoading();
        }

        private void FinishLoadingPhase()
        {
            loadingText.text = "Starting game...";
            loadingBar.DOFillAmount(1, 0.5f);
            currentProgressTxt.text = "100%";
        }

        private void HandleLoadingProgress()
        {
            int canStop = Random.Range(0, 4);
            if (stopTimer <= 0f && canStop == 3)
            {
                float speedModifier = Random.Range(0.001f, 0.01f);
                currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, speedModifier);
                if (Random.value > 0.9f)
                    stopTimer = Random.Range(0.35f, 0.85f);
            }
            else
            {
                stopTimer -= Time.deltaTime;
            }

            loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, currentProgress, smoothSpeed);
            currentProgressTxt.text = $"{(int)(loadingBar.fillAmount * 100)}%";

            if (currentProgress >= (float)(currentMessageIndex + 1) / loadingTxt.Length &&
                currentMessageIndex < loadingTxt.Length - 1)
            {
                currentMessageIndex++;
                loadingText.text = loadingTxt[currentMessageIndex];
            }

            currentTime += Time.deltaTime;
            targetProgress = Mathf.Clamp01(currentTime / loadDuration);
        }

        private void FinishLoading()
        {
            CallAdsManager.HideBannerLoading();
            CallAdsManager.DestroyBannerLoading();
            LoadAdsManually.LoadInterByGroup("gameplay");
            LoadAdsManually.LoadInterByGroup("break");
            LoadAdsManually.LoadBanner();
            LoadAdsManually.LoadReward();
            LoadAdsManually.ins.AddEndCardEvent();
            LoadingScene.instance.LoadMenu();
            AppOpenCaller.IgnoreAppOpenResume = false;
        }
    }
}