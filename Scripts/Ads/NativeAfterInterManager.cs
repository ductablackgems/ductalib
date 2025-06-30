using System;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Splash;
using BG_Library.Common;
using BG_Library.NET;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class NativeAfterInterManager : MonoBehaviour
    {
        public List<NativeAdContainer> nativeObjects;
        private Action onAfterInterFinished;
        private NativeAdContainer currentNativeAd;

        #region Init

        private void Awake()
        {
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnBeforeShowInter;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;

            FetchComplete();
        }

        private void FetchComplete()
        {
            if (CommonRemoteConfig.adsConfig?.naConfigs == null) return;
            foreach (var config in CommonRemoteConfig.adsConfig.naConfigs)
            {
                var nativeObject = nativeObjects.Find(x => x.nativeUIName == config.nativeUIName);
                nativeObject.SetUp(config);
            }
        }

        private void OnDestroy()
        {
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnBeforeShowInter;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialHiddenEvent;
        }

        #endregion
        private void OnBeforeShowInter(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            onAfterInterFinished = null;

            foreach (var obj in nativeObjects)
            {
                if (!ShouldShow(obj, CallAdsManager.currentInterstitial)) continue;

                obj.ShowBeforeAds();
                currentNativeAd = obj;
                onAfterInterFinished = () => { currentNativeAd.Show(); };
                break;
            }
        }

        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            onAfterInterFinished?.Invoke();
        }

        private bool ShouldShow(NativeAdContainer obj, string pos)
        {
            return obj.currentData.isEnabled && obj.ShouldShowForInter(pos);
        }
        
        public void StartNativeChain(string pos)
        {
            foreach (var obj in nativeObjects)
            {
                if (!ShouldShow(obj, pos)) continue;

                obj.ShowBeforeAds();
                currentNativeAd = obj;
                currentNativeAd.Show();

                break;
            }
        }

    }
}