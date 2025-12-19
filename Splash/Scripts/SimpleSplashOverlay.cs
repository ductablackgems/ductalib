using System;
using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Scripts.Loading;
using BG_Library.NET;
using DG.Tweening;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _0.DucTALib.Splash.Scripts
{
    [Serializable]
    public class SplashConfig
    {
        public bool useIntro;
        public float loadingTime;
        public float introStepTime;
        public float endIntroTime;
        public int stepCount;
        public List<string> adPositions;
        public List<string> endIntroAdPositions;
        public List<string> tipText;


        public static SplashConfig CreateDefault()
        {
            var value = new SplashConfig
            {
                adPositions = new List<string>() { " " },
                endIntroAdPositions = new List<string>() { " " },
                tipText = new List<string>() { " " },
            };
            return value;
        }
    }
    public class SimpleSplashOverlay : SingletonMono<SimpleSplashOverlay>
    {
 
        
        private float loadDuration;
        private float currentTime;
        private float currentProgress;
        private float targetProgress;
        private float stopTimer = 0f;
        private float smoothSpeed = 0.02f;
        private int currentMessageIndex;
        private bool dataFetched;
        public GameObject loading;
        public Image loadingBar;
        public TextMeshProUGUI loadingText;
        public TextMeshProUGUI currentProgressTxt;
        public IntroSplashOverlay introSplash;
        public SplashConfig splashConfig;
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

        private void InitFb()
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            splashConfig = JObject.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("splash_config").StringValue)
                .ToObject<SplashConfig>(JsonSerializer.Create(settings));
            dataFetched = true;
            LogHelper.CheckPoint("Splash config fetch done");
        }
        private IEnumerator AdsControl()
        {
            yield return new WaitUntil(() => CommonRemoteConfig.instance.fetchComplete);
            yield return new WaitUntil(() => AdmobMediation.IsInitComplete);
            InitFb();
            
            SplashTracking.SetBalanceAd(SplashTracking.SplashStepTracking.startLoading);
            
            CallAdsManager.InitBannerNA();
            CallAdsManager.LoadInterByGroup("launch");
            CallAdsManager.InitONA($"{splashConfig.adPositions[0]}");
            yield return new WaitUntil(CallAdsManager.BannerNAReady);
            loading.HideObject();
            CallAdsManager.ShowBannerNA();
        }

        private IEnumerator WaitToLoadScene()
        {
            loadingBar.fillAmount = 0;
            yield return new WaitForEndOfFrame();
            float fbTimeout = 8;
            while (fbTimeout > 0 && (!dataFetched))
            {
                fbTimeout -= Time.deltaTime;
                yield return null;
            }

            yield return new WaitForEndOfFrame();
            loadDuration = splashConfig.loadingTime;
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
            
            if (splashConfig.useIntro)
            {
                SplashTracking.SetBalanceAd(SplashTracking.SplashStepTracking.endLoading);
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

#if UNITY_EDITOR
        public string splashConfigDefault;
        [Button]
        public void CreateSplashConfig()
        {
            var value = SplashConfig.CreateDefault();
            splashConfigDefault = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            });
        }
#endif
    }
}