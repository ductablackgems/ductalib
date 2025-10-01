using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Ads.Native;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Scripts.Loading;
using _0.DucTALib.Splash.Scripts;
using BG_Library.Common;
using BG_Library.NET;
using Random = UnityEngine.Random;
#if USE_ADMOB_NATIVE
using BG_Library.NET.Native_custom;
#endif

namespace _0.DucTALib.Splash
{
#if USE_ADMOB_NATIVE
    [Serializable]
    public class NativeObject
    {
        public string nativeUIName;
        public string adsPosition;
        public NativeUIManager native;
        public bool isNativeFull;
    }

#endif

    public enum AdFormatType
    {
        Native,
        MREC,
        Inter,
        Banner,
        AppOpen,
    }

    public enum SplashType
    {
        Age,
        Intro,
        Language,
        Reward
    }

    public enum PagePosition
    {
        Left,
        Right
    }

    public class GameSplash : SingletonMono<GameSplash>
    {
#if USE_ADMOB_NATIVE
        [Header("UI")] public Image loadingBar;
        public TextMeshProUGUI loadingText;
        public TextMeshProUGUI currentProgressTxt;

        public NativeInterManager nativeEnd;
        [Header("Steps")] public List<BaseStepSplash> steps = new List<BaseStepSplash>();
        private int currentStep = 0;
        [ReadOnly] public BaseStepSplash currentStepPanel;

        [BoxGroup("Native")] public NativeUIManager native;
        public GameObject loading;

        public SplashNativeSetup splashNativeSetup;

        [Header("Loading Config")] [ReadOnly] public string[] loadingTxt = new string[]
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

        private float loadDuration;
        private float currentTime;
        private float currentProgress;
        private float targetProgress;
        private float stopTimer = 0f;
        private float smoothSpeed = 0.02f;
        private int currentMessageIndex;

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
            LoadAdsManually.LoadInterByGroup("launch");
#if USE_ADMOB_NATIVE
            native.Request("loading");
            yield return new WaitUntil(() => native.IsReady);
            native.Show();
            loading.HideObject();
#endif
        }

        private void SetLoadDuration()
        {
            loadDuration = 0;
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

            loadDuration = RemoteConfig.Ins.isDataFetched
                ? CommonRemoteConfig.instance.splashConfig.timeoutMin
                : 15f;
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
#if USE_ADMOB_NATIVE
            native.FinishNative();
#endif
            loadingBar.DOFillAmount(1, 0.2f);
            currentProgressTxt.text = "100%";
            CallAdsManager.ShowInter("launch");
            if (!RemoteConfig.Ins.isDataFetched || !CommonRemoteConfig.instance.splashConfig.loadIntro)
            {
                CompleteAllStep();
                yield return new WaitForSeconds(1f);
                yield break;
            }


            SetUpStep();
            yield return new WaitForEndOfFrame();


            LoadAdsManually.LoadInterByGroup("intro");
            LoadAdsManually.LoadInterByGroup("complete_intro");
            SplashTracking.LoadingEnd();
            currentProgressTxt.HideObject();
            currentStepPanel = steps[currentStep];
            StartStep();
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

        private void FinishLoadingPhase()
        {
            loadingText.text = "Starting game...";
            loadingBar.DOFillAmount(1, 0.5f);
            currentProgressTxt.text = "100%";
        }

        private void SetUpStep()
        {
            if (CommonRemoteConfig.instance.splashConfig.splashConfigs.Count == 0)
            {
                steps.Clear();
            }
            else
            {
                var panelMap = steps.ToDictionary(x => x.splashType, x => x);
                var orderedSteps = CommonRemoteConfig.instance.splashConfig.splashConfigs
                    .Select(cfg => panelMap[cfg])
                    .ToList();

                steps.Clear();
                steps.AddRange(orderedSteps);
            }

            var tut = steps.Find(x => x.splashType == SplashType.Intro);
            if (tut != null)
            {
                splashNativeSetup.SetupTutorialView();
            }

            if (CommonRemoteConfig.ins.splashConfig.completeAdsType == CompleteAdsType.NA)
            {
                nativeEnd.Load();
            }
        }

        private void StartStep()
        {
            currentStepPanel.Enter();
        }

        public void NextStep()
        {
            currentStep++;
            if (currentStep >= steps.Count)
            {
                Finish();
                return;
            }

            currentStepPanel = steps[currentStep];
            StartStep();
        }

        private void Finish()
        {
            if (CommonRemoteConfig.instance.splashConfig.completeAdsType == CompleteAdsType.Inter)
            {
                CallAdsManager.ShowInter("complete_all_step");
                CompleteAllStep();
            }
            else
            {
                nativeEnd.Show(CompleteAllStep);
            }
        }

        private void CompleteAllStep()
        {
            LoadAdsManually.LoadInterByGroup("gameplay");
            LoadAdsManually.LoadInterByGroup("break");
            LoadAdsManually.LoadReward();
            LoadAdsManually.LoadBanner();
            currentStepPanel.HideObject();
            LoadingScene.instance.LoadMenu();
            AppOpenCaller.IgnoreAppOpenResume = false;
        }
#endif
    }
}