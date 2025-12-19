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

namespace _0.DucTALib.Splash.Scripts
{
    public class SimpleSplashOverlay : SingletonMono<SimpleSplashOverlay>
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
        public IntroSplashOverlay introSplash;

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
            yield return new WaitUntil(() => CommonRemoteConfig.instance.fetchComplete);
            yield return new WaitUntil(() => AdmobMediation.IsInitComplete);
            CallAdsManager.InitBannerNA();
            CallAdsManager.LoadInterByGroup("launch");
            CallAdsManager.InitONA($"{CommonRemoteConfig.instance.splashConfig.adPositions[0]}");
            yield return new WaitUntil(CallAdsManager.BannerNAReady);
            loading.HideObject();
            CallAdsManager.ShowBannerNA();
        }

        private IEnumerator WaitToLoadScene()
        {
            loadingBar.fillAmount = 0;
            yield return new WaitForEndOfFrame();
            float fbTimeout = 8;
            while (fbTimeout > 0 && (RemoteConfig.Ins == null || !RemoteConfig.Ins.isDataFetched))
            {
                fbTimeout -= Time.deltaTime;
                yield return null;
            }

            yield return new WaitForEndOfFrame();
            loadDuration = CommonRemoteConfig.instance.splashConfig.loadingTime;
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
            yield return new WaitForEndOfFrame();
            if (CommonRemoteConfig.instance.splashConfig.useIntro)
            {
                CallAdsManager.StopReloadBNNA();
                CallAdsManager.HideBannerNA();
                CallAdsManager.ClearBannerNA();
                CallAdsManager.StopReloadFS("launch");
                StartIntro();
            }
            else FinishLoading();
        }

        private void StartIntro()
        {
            introSplash.ShowObject();
            introSplash.StartIntro();
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
            CallAdsManager.instance.FinishSplash();
            LoadingScene.instance.LoadMenu();
            AppOpenCaller.IgnoreAppOpenResume = false;
        }
    }
}