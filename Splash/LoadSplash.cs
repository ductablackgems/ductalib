using System.Collections;
using _0.Custom.Scripts;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using BG_Library.NET;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _0.DucTALib.Splash
{
    public class LoadSplash : SingletonMono<LoadSplash>
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

        [SerializeField] private FadeUI fadeUI;
        [SerializeField] private SetAgePanel selectAgePanel;
        [SerializeField] private IntroPanel introPanel;
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private Image loadingBar;
        [SerializeField] private RectTransform buttonContinue;
        public RectTransform ButtonContinue => buttonContinue;
        public FadeUI FadeUI => fadeUI;

        private float loadDuration = 12f;
        private float currentTime = 0f;
        private int currentMessageIndex = 0;

        private float currentProgress = 0f;
        private float targetProgress = 0f;

        private float stopDuration = 1f;
        private float stopTimer = 0f;
        private float smoothSpeed = 0.02f;
        private float cooldown = 0;

        private Coroutine loadingCoroutine;

        protected override void Init()
        {
            base.Init();
            // MobileAds.Initialize(initStatus => { });
            //
            // // Cấu hình thiết bị kiểm tra (Test Device ID)
            // RequestConfiguration requestConfiguration = new RequestConfiguration
            // {
            //     TestDeviceIds = new List<string>
            //     {
            //         "EF4384257930FCD9A8726EC9839C6316", 
            //     }
            // };
            //
            // MobileAds.SetRequestConfiguration(requestConfiguration);
            //
            // Debug.Log("Test device IDs configured successfully.");
        }

        void Start()
        {
            loadingBar.fillAmount = 0f;

            loadingCoroutine = StartCoroutine(FirstLoading());
            // if (PlayerDataPrefs.PlayerAge == 0)
            // {
            //     StartCoroutine(FirstLoading());
            // }
            // else
            // {
            //     StartCoroutine(FakeLoading());
            // }
        }
        IEnumerator WaitFirebaseIE()
        {
            float loadingDuration = 5f;
            float dotInterval = 0.5f;
            float elapsedTime = 0f;
            int dotCount = 0;
            var baseText = "Checking network connection";

            while (!AdsManager.Ins.AdsCoreIns)
            {
                loadingText.text = baseText + new string('.', dotCount);

                dotCount = (dotCount + 1) % 4;

                yield return new WaitForSeconds(dotInterval);
                elapsedTime += dotInterval;
            }

        }
        IEnumerator FirstLoading()
        {
            var cam = Camera.main;
            yield return StartCoroutine(WaitFirebaseIE());
            // CR loading
            // while (currentMessageIndex < 4)
            // {
            //     var canStop = Random.Range(0, 4);
            //     if (stopTimer <= 0f && canStop == 3)
            //     {
            //         float speedModifier = Random.Range(0.001f, 0.01f);
            //         currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, speedModifier);
            //
            //         if (Random.Range(0f, 1f) > 0.9f)
            //         {
            //             stopTimer = Random.Range(0.35f, 0.45f);
            //         }
            //     }
            //     else
            //     {
            //         stopTimer -= Time.deltaTime;
            //     }
            //
            //     loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, currentProgress, smoothSpeed);
            //
            //     if (currentProgress >= (float)(currentMessageIndex + 1) / loadingTxt.Length)
            //     {
            //         currentMessageIndex++;
            //         loadingText.text = loadingTxt[currentMessageIndex];
            //     }
            //
            //     currentTime += Time.deltaTime;
            //
            //     targetProgress = Mathf.Clamp01(currentTime / loadDuration);
            //
            //     yield return null;
            // }
            LogHelper.CheckPoint($"fetch {RemoteConfig.Ins.isDataFetched}");
            yield return new WaitForSeconds(0.5f);
            SplashTracking.TrackingIntro("show_select_age");
            selectAgePanel.ShowObject();
            CallAdsManager.ShowMRECApplovin(selectAgePanel.bannerPos.gameObject, cam);
            // CallAdsManager.ShowMRECAdmob(selectAgePanel.bannerPos);
            loadingText.text = "AUTO CLOSE LATER 6S";
            cooldown = 10;
            while (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
                loadingText.text = $"AUTO CLOSE LATER {(int)cooldown}S";
                yield return null;
            }
            CallAdsManager.DestroyMRECApplovin();
            HideAgeSelectPanel();
            yield return new WaitForSeconds(0.3f);
            CallAdsManager.ShowMRECApplovin(introPanel.bannerPos.gameObject, cam);
            // Todo: show downloading progress
            float downloadProgress = 0f;
            float displayedProgress = 0f;
            float fakeDownloadDuration = 5f;
            float stopTime = 0f;

            while (downloadProgress < 1f)
            {
                if (stopTime <= 0f)
                {
                    float randomSpeedModifier = Random.Range(0.001f, 0.01f);
                    downloadProgress += randomSpeedModifier;

                    if (Random.Range(0f, 1f) > 0.85f)
                    {
                        stopTime = Random.Range(0.3f, 0.7f);
                    }
                }
                else
                {
                    stopTime -= Time.deltaTime;
                }

                downloadProgress = Mathf.Clamp01(downloadProgress);
                displayedProgress = Mathf.Lerp(displayedProgress, downloadProgress, 0.1f);
                loadingText.text = $"Downloading... {Mathf.FloorToInt(displayedProgress * 100)}%";
                yield return null;
            }

            
            loadingText.text = "Download Complete!";
            yield return new WaitForSeconds(0.5f);
            // endtodo
            // CR loading
            loadingText.text = loadingTxt[currentMessageIndex];
            while (currentMessageIndex < 14)
            {
                var canStop = Random.Range(0, 4);
                if (stopTimer <= 0f && canStop == 3)
                {
                    float speedModifier = Random.Range(0.001f, 0.005f);
                    currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, speedModifier);

                    if (Random.Range(0f, 1f) > 0.9f)
                    {
                        stopTimer = Random.Range(0.4f, 0.75f);
                    }
                }
                else
                {
                    stopTimer -= Time.deltaTime;
                }

                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, currentProgress, smoothSpeed);

                if (currentProgress >= (float)(currentMessageIndex + 1) / loadingTxt.Length)
                {
                    currentMessageIndex++;
                    loadingText.text = loadingTxt[currentMessageIndex];
                }

                currentTime += Time.deltaTime;

                targetProgress = Mathf.Clamp01(currentTime / loadDuration);

                yield return null;
            }

            loadingBar.fillAmount = 1;
            // ShowScene();
        }

        public void StopCRLoading()
        {
            loadingBar.fillAmount = 1;
            loadingText.text = loadingTxt[^1];
            StopCoroutine(loadingCoroutine);
            SplashTracking.TrackingIntro("show_inter_intro");
            CallAdsManager.ShowInter(ConstAdsPos.end_intro.ToString());
            SplashTracking.TrackingIntro("complete_inter_intro");
            ShowScene();
        }


        IEnumerator FakeLoading()
        {
            loadingText.text = loadingTxt[currentMessageIndex];

            while (currentMessageIndex < 15)
            {
                var canStop = Random.Range(0, 4);
                if (stopTimer <= 0f && canStop == 3)
                {
                    float speedModifier = Random.Range(0.001f, 0.005f);
                    currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, speedModifier);

                    if (Random.Range(0f, 1f) > 0.9f)
                    {
                        stopTimer = Random.Range(0.4f, 0.9f);
                    }
                }
                else
                {
                    stopTimer -= Time.deltaTime;
                }

                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, currentProgress, smoothSpeed);

                if (currentProgress >= (float)(currentMessageIndex + 1) / loadingTxt.Length)
                {
                    currentMessageIndex++;
                    loadingText.text = loadingTxt[currentMessageIndex];
                }

                currentTime += Time.deltaTime;

                targetProgress = Mathf.Clamp01(currentTime / loadDuration);

                yield return null;
            }

            // ShowScene();
        }


        public void ResetCooldown(float cooldown)
        {
            this.cooldown = cooldown;
        }

        public void HideAgeSelectPanel()
        {
            SplashTracking.TrackingIntro("show_inter_select_age");
            CallAdsManager.ShowInter(ConstAdsPos.hide_select_age.ToString());
            SplashTracking.TrackingIntro("complete_inter_select_age");
            fadeUI.Fade(() =>
            {
                selectAgePanel.HideObject();
                introPanel.ShowObject();
            });
        }

        void ShowScene()
        {
            CallAdsManager.DestroyMRECApplovin();
            fadeUI.Fade(() =>
            {
                introPanel.HideObject();
                loadingPanel.ShowObject();
            });
            
            DOVirtual.DelayedCall(5, () =>
            {
                SceneManager.LoadScene("Menu");
            });
        }
    }
}