using System;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Splash;
using BG_Library.Common;
using BG_Library.NET;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class NativeAfterInterManager : SingletonMono<NativeAfterInterManager>
    {
        public static D_AdsConfig adsConfig;
        public List<NativeAdContainer> nativeObjects;
        private Action onAfterInterFinished;
        private NativeAdContainer currentNativeAd;
        #region Init

        protected override void Init()
        {
            base.Init();
            DontDestroyOnLoadSelf();
            RegisterEvents();
        }

        private void DontDestroyOnLoadSelf()
        {
            gameObject.transform.SetParent(DDOL.Instance.transform);
        }

        private void RegisterEvents()
        {
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnBeforeShowInter;
            RemoteConfig.OnFetchComplete += FetchComplete;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        }

        private void FetchComplete()
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            JObject root = JObject.Parse(RemoteConfig.Ins.custom_config);
            adsConfig = root["AdsConfig"]?.ToObject<D_AdsConfig>(JsonSerializer.Create(settings));
            InitNA();
        }

        private void OnDestroy()
        {
            RemoteConfig.OnFetchComplete -= FetchComplete;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent  -= OnBeforeShowInter;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialHiddenEvent;
        }

        private void InitNA()
        {
            if (adsConfig?.naConfigs == null) return;
            foreach (var config in adsConfig.naConfigs)
            {
                var nativeObject = nativeObjects.Find(x => x.nativeUIName == config.nativeUIName);
                nativeObject.SetUp(config);
            }
        }

        #endregion
       
        private void OnBeforeShowInter(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // onAfterInterFinished = null;
            // currentNativeAd = null;
            // foreach (var obj in nativeObjects)
            // {
            //     if (!ShouldShow(obj, CallAdsManager.currentInterstitial)) continue;
            //     obj.ShowBeforeAds();
            //     currentNativeAd = obj;
            //     currentNativeAd.Refresh();
            //     onAfterInterFinished = () =>
            //     {
            //         currentNativeAd?.Show();
            //     };
            //
            //     break;
            // }
            
           
                onAfterInterFinished = null;
            
                foreach (var obj in nativeObjects)
                {
                    if (!ShouldShow(obj, CallAdsManager.currentInterstitial)) continue;
            
                    obj.ShowBeforeAds();
                    currentNativeAd = obj;
                    onAfterInterFinished = () =>
                    {
                        currentNativeAd.Show();
                    };
            
                    break;
                }
            
        }

        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            onAfterInterFinished?.Invoke();
        }
        
        private bool ShouldShow(NativeAdContainer obj, string pos)
        {
            LogHelper.CheckPoint($"{obj.currentData.isEnabled}_{obj.ShouldShowForInter(pos)}");
            return obj.currentData.isEnabled && obj.ShouldShowForInter(pos);
        }
    }
}