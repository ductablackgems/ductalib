#if USE_ADMOB_NATIVE
using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Scripts.Loading;
using BG_Library.NET;
using BG_Library.NET.Native_custom;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.Splash.Scripts
{
    public class SimpleSplashNative : MonoBehaviour
    {
        public List<NativeFullUI> nativeLaunch;
        public List<NativeFullUI> activeNativeLaunch;
        public List<string> launchPos;
        public NativeUIManager nativeBanner;
        
        private float loadDuration;
        private float currentTime;
        private float currentProgress;
        private float targetProgress;
        private float stopTimer = 0f;
        private float smoothSpeed = 0.02f;
        private int currentMessageIndex;
        public GameObject loading;
        public Image loadingFill;
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
            yield return new WaitUntil(() => CommonRemoteConfig.instance.fetchComplete);
            yield return new WaitUntil(() => AdmobMediation.IsInitComplete);
            
#if UNITY_EDITOR
            loading.HideObject();
            nativeBanner.ShowObject();
            yield break;
#endif
            nativeBanner.Request("loading");
            for (int i = 0; i < nativeLaunch.Count; i++)
            {
                var native =  nativeLaunch[i];
                var pos = launchPos[i];
                native.LoadAd(pos);
            }

            yield return new WaitUntil(()=> nativeBanner.IsReady);
            loading.HideObject();
            nativeBanner.Show();
        }

        private IEnumerator WaitToLoadScene()
        {
            loadingFill.fillAmount = 0;
            yield return new WaitForEndOfFrame();

            float fbTimeout = 8;
            while (fbTimeout > 0 && (RemoteConfig.Ins == null || !RemoteConfig.Ins.isDataFetched))
            {
                fbTimeout -= Time.deltaTime;
                yield return null;
            }
            yield return new WaitForEndOfFrame();
            loadDuration = CommonRemoteConfig.instance.commonConfig.splashTime;
            currentProgressTxt.text = $"0%";
            while (currentTime < loadDuration)
            {
                HandleLoadingProgress();
                yield return null;
            }

            FinishLoadingPhase();
            AppOpenCaller.IgnoreAppOpenResume = true;
            SplashTracking.SetUserProperty();
            foreach (var native in nativeLaunch)
            {
#if UNITY_EDITOR
                activeNativeLaunch.Add(native);
                native.onClose += CloseNativeLaunch;
#else
                if (native.IsReady)
                {
                    activeNativeLaunch.Add(native);
                    native.onClose += CloseNativeLaunch;
                }
#endif
            }

            activeNativeLaunch[0].ShowAds();
            yield return new WaitUntil(()=> activeNativeLaunch.Count <= 0);
            FinishLoading();
        }

        private void CloseNativeLaunch(NativeFullUI native)
        {
            if (activeNativeLaunch.Contains(native))
            {
                activeNativeLaunch.Remove(native);
                native.onClose = null;
            }
            if(activeNativeLaunch.Count > 0) activeNativeLaunch[0].ShowAds();
        }

        private void FinishLoadingPhase()
        {
            loadingText.text = "Starting game...";
            loadingFill.DOFillAmount(1, 0.5f);
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

            loadingFill.fillAmount = Mathf.Lerp(loadingFill.fillAmount, currentProgress, smoothSpeed);
            currentProgressTxt.text = $"{(int)(loadingFill.fillAmount * 100)}%";

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
            CallAdsManager.LoadBanner();
            nativeBanner.FinishNative();
            CallAdsManager.LoadInterByGroup("gameplay");
            CallAdsManager.LoadInterByGroup("break");
            LoadingScene.instance.LoadMenu();
            AppOpenCaller.IgnoreAppOpenResume = false;
        }
    }
}

#endif