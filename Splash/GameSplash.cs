using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using _0.Custom.Scripts;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Scripts.Loading;
using _0.DucTALib.Splash.Scripts;
using BG_Library.NET;
using BG_Library.NET.Native_custom;
using DG.Tweening;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CommonHelper = _0.DucLib.Scripts.Common.CommonHelper;
using Random = UnityEngine.Random;

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
        public string sceneName;

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

        public List<BaseStepSplash> steps = new List<BaseStepSplash>();
        public Image loadingBar;
        public TextMeshProUGUI loadingText;
        public TextMeshProUGUI currentProgressTxt;
        private float loadingDuration = 12f;
        private float[] speedStages = { 2f, 1f, 0.5f, 1.5f, 2.5f };
        private float loadDuration;
        private int currentMessageIndex = 0;
        private float currentTime = 0f;

        private float currentProgress = 0f;
        private float targetProgress = 0f;

        private float stopDuration = 1f;
        private float stopTimer = 0f;
        private float smoothSpeed = 0.02f;
        private float cooldown = 0;
        private int currentStep = 0;
        private BaseStepSplash currentStepPanel;

        [BoxGroup("Native")] public NativeUIManager native;

        private void Start()
        {
            SplashTracking.loading_duration.Reset();
            SplashTracking.loading_duration.Start();
            StartCoroutine(AdsControl());
            StartCoroutine(WaitToLoadScene());
        }

        IEnumerator AdsControl()
        {
            yield return new WaitUntil(() => AdmobMediation.IsInitComplete);
            native.Request("loading");

            yield return new WaitUntil(() => native.IsReady);
            native.Show();
        }

        IEnumerator WaitToLoadScene()
        {
            yield return new WaitForEndOfFrame();
            float fbTimeout = 5;
            while (fbTimeout > 0 && (RemoteConfig.Ins == null || RemoteConfig.Ins.isDataFetched))
            {
                fbTimeout -= Time.deltaTime;
                yield return null;
            }
            if (RemoteConfig.Ins.isDataFetched)
            {
                loadDuration = SplashRemoteConfig.CustomConfigValue.timeoutMin;
            }
            else
            {
                loadDuration = 7;
            }
            loadingBar.fillAmount = 0;
            currentProgressTxt.text = $"{(int)(loadingBar.fillAmount * 100)}%";
            while (currentTime < loadDuration)
            {
                var canStop = Random.Range(0, 4);
                if (stopTimer <= 0f && canStop == 3)
                {
                    float speedModifier = Random.Range(0.001f, 0.01f);
                    currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, speedModifier);
                    if (Random.Range(0f, 1f) > 0.9f)
                    {
                        stopTimer = Random.Range(0.35f, 0.85f);
                    }
                }
                else
                {
                    stopTimer -= Time.deltaTime;
                }

                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, currentProgress, smoothSpeed);
                currentProgressTxt.text = $"{(int)(loadingBar.fillAmount * 100)}%";

                if (currentProgress >= (float)(currentMessageIndex + 1) / loadingTxt.Length)
                {
                    currentMessageIndex++;
                    loadingText.text = loadingTxt[currentMessageIndex];
                }

                currentTime += Time.deltaTime;
                targetProgress = Mathf.Clamp01(currentTime / loadDuration);
                yield return null;
            }

            loadingText.text = "Starting game...";
#if IGNORE_INTRO
            native.FinishNative();
            AdsManager.InitBannerManually();
            loadingBar.DOFillAmount(1, 0.5f);
            currentProgressTxt.text = $"{100}%";
            CompleteAllStep();
            yield return new WaitForSeconds(1f);
            yield break;
#endif
            if (SplashRemoteConfig.CustomConfigValue == null)
            {
                LogHelper.LogLine();
                SplashRemoteConfig.instance.FetchComplete();
            }
            if (!RemoteConfig.Ins.isDataFetched || !SplashRemoteConfig.CustomConfigValue.loadIntro)
            {
                // remove native
                native.FinishNative();
                AdsManager.InitBannerManually();
                // init banner
                loadingBar.DOFillAmount(1, 0.5f);
                currentProgressTxt.text = $"{100}%";
                CompleteAllStep();
                yield return new WaitForSeconds(1f);
                yield break;
            }

            float timer = 0f;
            float timeoutLater = SplashRemoteConfig.CustomConfigValue.timeoutMax -
                                 SplashRemoteConfig.CustomConfigValue.timeoutMin;
            while (!AdsManager.IsMrecReady && timer < timeoutLater)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            loadingBar.DOFillAmount(1, 0.2f);
            currentProgressTxt.text = $"{100}%";
            SetUpStep();
            yield return new WaitForEndOfFrame();
            native.FinishNative();
            AdsManager.InitBannerManually();
            SplashTracking.LoadingEnd();
            currentProgressTxt.HideObject();
            currentStepPanel = steps[currentStep];
            StartStep();
        }

        private void SetUpStep()
        {
            var panelMap = steps.ToDictionary(x => x.splashType, x => x);
            var config = SplashRemoteConfig.CustomConfigValue.splashConfigs;

            var list = config
                .Select(a => panelMap[a.type])
                .ToList();

            steps.Clear();
            steps.AddRange(list);
        }

        private void StartStep()
        {
            currentStepPanel.Enter();
        }

        public void NextStep()
        {
            currentStep += 1;
            if (currentStep >= steps.Count)
            {
                CompleteAllStep();
                if (SplashRemoteConfig.CustomConfigValue.interComplete) CallAdsManager.ShowInter("complete_all_step");

                return;
            }

            currentStepPanel = steps[currentStep];
            StartStep();
        }

        private void CompleteAllStep()
        {
            currentStepPanel.HideObject();
            CallAdsManager.HideMRECApplovin();
            LoadingScene.instance.LoadMenu();
        }
    }
}