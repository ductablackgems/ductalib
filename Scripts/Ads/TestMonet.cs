using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using _0.DucLib.Scripts.Common;
using BG_Library.NET;
using Firebase;
using Firebase.RemoteConfig;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class TestMonet : MonoBehaviour
    {
        public DucTALib.Scripts.Common.UIDragObject trans;
        public GameObject objectImmersive;
        public TMP_InputField inputField;
        public TextMeshProUGUI _tmp;

        private void Start()
        {
            _tmp.text = "Remote Config — collecting condition info...";

            StartCoroutine(BuildAndShow());
        }

        private IEnumerator BuildAndShow()
        {
            // Init Firebase
            yield return new WaitUntil(() => RemoteConfig.Ins.isDataFetched);

            var location = FirebaseRemoteConfig.DefaultInstance.GetValue("location").StringValue;

            // Fetch + Activate để chắc dữ liệu mới nhất
            var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            while (!fetchTask.IsCompleted) yield return null;

            var activateTask = FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
            while (!activateTask.IsCompleted) yield return null;

            // Thu thập thông tin "condition-related"
            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            var sb = new StringBuilder();

            // — App / Device / Locale —
            string appName = Application.productName;
            string bundle = Application.identifier;
            string appVer = Application.version;
            string platform =
#if UNITY_ANDROID
                "Android";
#elif UNITY_IOS
            "iOS";
#elif UNITY_EDITOR
            "Editor";
#else
            Application.platform.ToString();
#endif
            string device = SystemInfo.deviceModel;
            string lang = Application.systemLanguage.ToString();
            string culture = CultureInfo.CurrentCulture.Name;
            string region = "";
            try
            {
                region = new RegionInfo(CultureInfo.CurrentCulture.LCID).TwoLetterISORegionName;
            }
            catch
            {
                region = "??";
            }

            string timezone = TimeZoneInfo.Local.DisplayName;

            // — (tuỳ chọn) App Instance ID — hữu ích để target theo danh sách
            string appInstanceId = null;

            // — Tóm tắt nguồn tham số (Remote/Default/Static) —
            int cRemote = 0, cDefault = 0, cStatic = 0;
            foreach (var k in FirebaseRemoteConfig.DefaultInstance.Keys)
            {
                switch (FirebaseRemoteConfig.DefaultInstance.GetValue(k).Source)
                {
                    case ValueSource.RemoteValue: cRemote++; break;
                    case ValueSource.DefaultValue: cDefault++; break;
                    case ValueSource.StaticValue: cStatic++; break;
                }
            }

            // — Render text —
            sb.AppendLine("════════ Remote Config — Condition Info ════════");
            sb.AppendLine($"App           : {appName} ({bundle})");
            sb.AppendLine($"Version       : {appVer}");
            sb.AppendLine($"Platform      : {platform} — {device}");
            sb.AppendLine($"Language      : {lang} ({culture})");
            sb.AppendLine($"Location        : {location}");
            sb.AppendLine($"Time Zone     : {timezone}");
            sb.AppendLine();
            sb.AppendLine("— RemoteConfig Fetch Status —");
            sb.AppendLine($"LastFetchStatus : {info.LastFetchStatus}");
            sb.AppendLine($"LastFetchTime   : {info.FetchTime}");
            sb.AppendLine($"ThrottledEnd    : {info.ThrottledEndTime}");
            sb.AppendLine();

            _tmp.text = sb.ToString();
        }

        public void ShowAds()
        {
            CallAdsManager.ShowONA("Test1", trans.rect);
        }

        public void HideAds()
        {
            CallAdsManager.CloseONA("Test1");
        }

        public void LoadAds()
        {
            CallAdsManager.InitONA("Test1");
        }

        public void InitBN()
        {
            CallAdsManager.LoadBanner();
        }

        public void ShowBanner()
        {
            CallAdsManager.ShowBanner();
        }

        public void ShowCollapse()
        {
            CallAdsManager.ShowBannerCollapsible();
        }

        public void ShowInter()
        {
            string value = inputField.text;
            CallAdsManager.ShowInter(value);
        }

        public void InitInter()
        {
            string value = inputField.text;
            CallAdsManager.LoadInterByGroup(value);
        }

        public void ShowBN()
        {
            CallAdsManager.ShowBanner();
        }

        public void ShowRW()
        {
            CallAdsManager.ShowRewardVideo("", () => { LogHelper.CheckPoint("reward done"); });
        }

        public void ShowBNNA()
        {
            CallAdsManager.ShowBannerNA();
        }

        public void InitBNNA()
        {
            CallAdsManager.InitBannerNA();
        }

#if USE_IMMERSIVE_ADMOB
        public ImmersiveInGameDisplayAd ad;

        public void LoadImmersive()
        {
            ImmersiveInGameDisplayAd.Initialize(() =>
            {
                // Configure an Adloader with ad unit to be shown on loading screen
                ImmersiveInGameDisplayAdAspectRatio adSize = new ImmersiveInGameDisplayAdAspectRatio(1, 1);
                AdLoader adLoader = new AdLoader.Builder("ca-app-pub-8243023565158105/3998471597")
                    .ForImmersiveInGameDisplayAd()
                    .SetImmersiveInGameDisplayAdAspectRatio(adSize)
                    .Build();

                // Configure callbacks
                adLoader.OnImmersiveInGameDisplayAdLoaded += this.HandleImmersiveInGameDisplayAdLoaded;
                adLoader.OnAdLoadFailed += this.HandleAdLoadFailed;
                adLoader.OnImmersiveInGameDisplayAdPaidEvent += this.HandleAdPaidEvent;
                adLoader.OnImmersiveInGameDisplayAdClicked += this.HandleAdClicked;
                adLoader.OnImmersiveInGameDisplayAdImpression += this.HandleAdImpression;

                // Make the ad request
                var request = new AdRequest();
                adLoader.LoadAd(request);
            });
        }

        private void HandleImmersiveInGameDisplayAdLoaded(object sender,
            ImmersiveInGameDisplayAdEventArgs args)
        {
            LogHelper.CheckPoint();
            ad = args.ImmersiveInGameDisplayAd;
        }

        // Ad Load Failure.
        private void HandleAdLoadFailed(LoadAdError error)
        {
            Debug.Log("Immersive in-game display ad failed to load : " + error.GetMessage());
            // Retry ad load after a delay.
        }

        private void HandleAdImpression(object sender, EventArgs args)
        {
            Debug.Log("Immersive in-game ad impression fired for ad type ");
        }

        private void HandleAdClicked(object sender, EventArgs args)
        {
            Debug.Log("Immersive in-game ad clicked.");
        }

        private void HandleAdPaidEvent(AdValue adValue)
        {
            Debug.Log(String.Format("Immersive in-game ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        }
        public void ShowImmersiveAds()
        {
            ad.SetParent(objectImmersive);
            ad.SetLocalPosition(new Vector3(0, 0, -0.01f));
            ad.SetLocalRotation(new Quaternion(0, 0, 0, 0));
            ad.SetLocalScale(1.0f);
            ad.ShowAd();
        }
#endif

#if USE_ADMOB_MEDIATION
        public PreloadConfiguration infoAdsInter;

        public void PreloadAdsInterstitial()
        {
            infoAdsInter = new PreloadConfiguration
            {
                AdUnitId = "ca-app-pub-7184522407199366/6047410270",
                Request = new AdRequest(),
                BufferSize = 5
            };

            // InterstitialAdPreloader.Preload(
            //     // The Preload ID can be any unique string to identify this configuration.
            //     "ca-app-pub-7184522407199366/6047410270",
            //     infoAdsInter,
            //     onAdPreloaded,
            //     onAdFailedToPreload,
            //     onAdsExhausted);
        }

        public void ShowAdsInterstitial()
        {
            // var ad = InterstitialAdPreloader.DequeueAd("ca-app-pub-7184522407199366/6047410270");
            // LogHelper.CheckPoint($"Count inter 1 {infoAdsInter.BufferSize}");
            // if (ad != null)
            // {
            //     LogHelper.CheckPoint($"Count inter  2 {infoAdsInter.BufferSize}");
            //     // [Optional] Interact with the ad object as needed.
            //     ad.OnAdPaid += (AdValue value) =>
            //     {
            //         LogHelper.CheckPoint($"Ad paid: {value.CurrencyCode} {value.Value}");
            //     };
            //     ad.Show();
            //     LogHelper.CheckPoint($"Count inter 3 {infoAdsInter.BufferSize}");
            // }
        }

        void onAdPreloaded(string preloadId, ResponseInfo responseInfo)
        {
            LogHelper.CheckPoint($"Preload ad configuration {preloadId} was preloaded.");
        }

        void onAdFailedToPreload(string preloadId, AdError adError)
        {
            string errorMessage = $"Preload ad configuration {preloadId} failed to " +
                                  $"preload with error : {adError.GetMessage()}.";
            LogHelper.CheckPoint(errorMessage);
        }

        void onAdsExhausted(string preloadId)
        {
            LogHelper.CheckPoint($"Preload ad configuration {preloadId} was exhausted");
        }
#endif
    }
}