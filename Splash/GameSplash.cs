using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Scripts.Loading;
using _0.DucTALib.Splash.Scripts;
using BG_Library.NET;
using BG_Library.NET.Native_custom;
using Scripts.Common;

namespace _0.DucTALib.Splash
{
    public enum SplashType
    {
        Age,
        Intro,
        Language,
        Reward
    }

    public class GameSplash : SingletonMono<GameSplash>
    {
        [Header("UI")] public Image loadingBar;
        public TextMeshProUGUI loadingText;
        public TextMeshProUGUI currentProgressTxt;

        [Header("Steps")] public List<BaseStepSplash> steps = new List<BaseStepSplash>();
        private int currentStep = 0;
        private BaseStepSplash currentStepPanel;
            
        [BoxGroup("Native")] public NativeUIManager native;
        public bool ignoreNative;
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

        public bool hasShowNative;
        private void Start()
        {
            SplashTracking.loading_duration.Reset();
            SplashTracking.loading_duration.Start();
            StartCoroutine(AdsControl());
            StartCoroutine(WaitToLoadScene());
        }

        #region Coroutine: Loading + Native

        private IEnumerator AdsControl()
        {
            if (ignoreNative) yield break;
            yield return new WaitUntil(() => AdmobMediation.IsInitComplete);
            native.Request("loading");
            yield return new WaitUntil(() => native.IsReady);
            native.Show();
            LogHelper.LogLine();
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
            LogHelper.LogLine();
            loadDuration = RemoteConfig.Ins.isDataFetched
                ? SplashRemoteConfig.CustomConfigValue.timeoutMin
                : 12f;
            loadingBar.fillAmount = 0;
            currentProgressTxt.text = $"0%";
            while (currentTime < loadDuration)
            {
                HandleLoadingProgress();
                yield return null;
            }
            FinishLoadingPhase();
            SplashTracking.SetUserProperty();
            if (!RemoteConfig.Ins.isDataFetched || !SplashRemoteConfig.CustomConfigValue.loadIntro)
            {
                CompleteAllStep();
                yield return new WaitForSeconds(1f);
                yield break;
            }
            float timeoutLater = SplashRemoteConfig.CustomConfigValue.timeoutMax -
                                 SplashRemoteConfig.CustomConfigValue.timeoutMin;
            float timer = 0f;
            while (!AdsManager.IsMrecReady && timer < timeoutLater)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            loadingBar.DOFillAmount(1, 0.2f);
            currentProgressTxt.text = "100%";
            SetUpStep();
            LogHelper.LogLine();
            yield return new WaitForEndOfFrame();
            if(!ignoreNative)
                native.FinishNative();
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

        #endregion

        #region Splash Step

        private void SetUpStep()
        {
            var panelMap = steps.ToDictionary(x => x.splashType, x => x);
            var orderedSteps = SplashRemoteConfig.CustomConfigValue.splashConfigs
                .Select(cfg => panelMap[cfg.type])
                .ToList();

            steps.Clear();
            steps.AddRange(orderedSteps);
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
                CompleteAllStep();

                if (SplashRemoteConfig.CustomConfigValue.interComplete)
                    CallAdsManager.ShowInter("complete_all_step");

                return;
            }

            currentStepPanel = steps[currentStep];
            StartStep();
        }

        private void CompleteAllStep()
        {
            currentStepPanel.HideObject();
            AdsManager.ShowBanner();
            CallAdsManager.HideMRECApplovin();
            LoadingScene.instance.LoadMenu();
        }

        #endregion
    }
}