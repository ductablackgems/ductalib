using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Splash.Scripts;
using BG_Library.NET;
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
        Age, Intro, Language, Reward
    }
    [Serializable]
    public class SplashConfig
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public SplashType type;
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
        public GameObject loadingObj;
        public Image loadingBar;
        public TextMeshProUGUI loadingText;
        public TextMeshProUGUI currentProgressTxt;
        private float loadingDuration = 12f;
        private float[] speedStages = { 2f, 1f, 0.5f, 1.5f, 2.5f };
        public float loadDuration = 12f;
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
        private void Start()
        {
            StartCoroutine(WaitToLoadScene());
        }

        IEnumerator WaitToLoadScene()
        {
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

            loadingText.text = "Starting game";
            Debug.Log($"rererer {AdsManager.IsMrecReady}");
            yield return new WaitUntil(() => AdsManager.IsMrecReady);
            loadingBar.DOFillAmount(1, 0.2f);
            currentProgressTxt.text = $"{100}%";
            
            if (!SplashRemoteConfig.CustomConfigValue.loadIntro)
            {
                CompleteAllStep();
                yield break;
            }
            SetUpStep();
            yield return new WaitForSeconds(0.2f);
            CommonHelper.HideObject(currentProgressTxt);
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
                return;
            }
            
            currentStepPanel = steps[currentStep];
            StartStep();
        }

        private void CompleteAllStep()
        {
            loadingObj.ShowObject();
            currentStepPanel.HideObject();
            CallAdsManager.DestroyMRECApplovin();
            DOVirtual.DelayedCall(2.5f, () =>
            {
                SceneManager.LoadScene(sceneName);
            });
        }
    }
}