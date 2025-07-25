﻿using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Splash;
using BG_Library.Common;
using BG_Library.NET.Native_custom;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads.Native
{
    public class NativeAfterInterManager : MonoBehaviour
    {
        public NativeAfterInterConfig naConfigs;
        public GameObject parent;

        private Dictionary<string, NativeUIManager> managers = new();
        private Dictionary<string, UINativeController> uiControllers = new();


        private void Awake()
        {
            if (!CommonRemoteConfig.adsConfig.naAfterInter.isEnabled) return;
            LogHelper.CheckPoint("[NativeAfterInterManager] Awake → Register OnAdDisplayedEvent");
#if USE_MAX_MEDIATION
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += HandleOnAdHiddenEvent;

            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayed;

#else
            BG_Event.AdmobMediation.Interstitial.OnAdFullScreenContentOpened += OnInterstitialDisplayed;
            BG_Event.AdmobMediation.Interstitial.OnAdFullScreenContentClosed += HandleOnAdHiddenEvent;
#endif
        }

        private void OnDestroy()
        {
            if (!CommonRemoteConfig.adsConfig.naAfterInter.isEnabled) return;
            LogHelper.CheckPoint("[NativeAfterInterManager] OnDestroy → Unregister Events");
#if USE_MAX_MEDIATION
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnInterstitialDisplayed;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= HandleOnAdHiddenEvent;
#else
            BG_Event.AdmobMediation.Interstitial.OnAdFullScreenContentOpened -= OnInterstitialDisplayed;
            BG_Event.AdmobMediation.Interstitial.OnAdFullScreenContentClosed -= HandleOnAdHiddenEvent;
#endif
        }

        private void Start()
        {
            LogHelper.CheckPoint("[NativeAfterInterManager] Start");
            naConfigs = CommonRemoteConfig.adsConfig.naAfterInter;


            if (!naConfigs.isEnabled) return;

            for (int i = 0; i < naConfigs.nativePosition.Count; i++)
            {
                string pos = naConfigs.nativePosition[i];
                string uiName = naConfigs.nativeUIName[i];

                var manager = GetNativeManager(uiName);
                if (manager != null)
                {
                    LogHelper.CheckPoint($"[NativeAfterInterManager] Request initial native {pos} for {uiName}");
                    manager.Request(pos);
                }
            }
        }

#if USE_MAX_MEDIATION
         private void OnInterstitialDisplayed(string adUnitI, MaxSdkBase.AdInfo adInfo)
        {
            string interPos = CallAdsManager.currentInterstitial;

        }

        private void HandleOnAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LogHelper.CheckPoint("[NativeAfterInterManager] Inter closed → begin native show flow");

            Time.timeScale = 0;
            CallAdsManager.HideBanner();

            ShowSequence(naConfigs, 0);
        }
#else
        private void OnInterstitialDisplayed(string adUnitI)
        {
            string interPos = CallAdsManager.currentInterstitial;

        }

        private void HandleOnAdHiddenEvent(string adUnitId)
        {
            LogHelper.CheckPoint("[NativeAfterInterManager] Inter closed → begin native show flow");

            Time.timeScale = 0;
            CallAdsManager.HideBanner();

            ShowSequence(naConfigs, 0);
        }
#endif


        private void ShowSequence(NativeAfterInterConfig config, int step)
        {
            if (step >= config.nativePosition.Count)
            {
                LogHelper.CheckPoint("[NativeAfterInterManager] All natives finished");
                Time.timeScale = 1;
                CallAdsManager.ShowBanner();

                var loadedUI = new HashSet<string>();
                for (int i = 0; i < config.nativeUIName.Count; i++)
                {
                    string uiName = config.nativeUIName[i];
                    string pos = config.nativePosition[i];

                    if (!loadedUI.Contains(uiName))
                    {
                        var manager = GetNativeManager(uiName);
                        if (manager != null)
                        {
                            LogHelper.CheckPoint($"[NativeAfterInterManager] Re-request native {pos} for {uiName}");
                            manager.Request(pos);
                            loadedUI.Add(uiName);
                        }
                    }
                }

                return;
            }

            string uiName1 = config.nativeUIName[step];
            string pos1 = config.nativePosition[step];

            LogHelper.CheckPoint($"[NativeAfterInterManager] Try show native {pos1} ({uiName1})");

            var manager1 = GetNativeManager(uiName1);
            var ui = GetUIController(uiName1);

            if (manager1 == null || ui == null || !manager1.IsReady)
            {
                ShowSequence(config, step + 1);
                return;
                LogHelper.CheckPoint($"[NativeAfterInterManager] {uiName1} not ready or missing → finish all");
                Time.timeScale = 1;
                CallAdsManager.ShowBanner();

                var loadedUI = new HashSet<string>();
                for (int i = 0; i < config.nativeUIName.Count; i++)
                {
                    string firstUI = config.nativeUIName[i];
                    string firstPos = config.nativePosition[i];

                    if (!loadedUI.Contains(firstUI))
                    {
                        var m = GetNativeManager(firstUI);
                        if (m != null)
                        {
                            LogHelper.CheckPoint(
                                $"[NativeAfterInterManager] Re-request native {firstPos} for {firstUI}");
                            m.Request(firstPos);
                            loadedUI.Add(firstUI);
                        }
                    }
                }

                return;
            }

            ui.onStartAll = () => LogHelper.CheckPoint($"[NativeAfterInterManager] UI {uiName1} start");
            ui.onEndAll = () =>
            {
                LogHelper.CheckPoint($"[NativeAfterInterManager] UI {uiName1} end");
                ShowSequence(config, step + 1);
            };

            ui.SetQueue(new List<string> { pos1 }, 0);
            ui.ShowCurrent();

            for (int i = step + 1; i < config.nativeUIName.Count; i++)
            {
                if (config.nativeUIName[i] == uiName1)
                {
                    string nextPos = config.nativePosition[i];
                    LogHelper.CheckPoint($"[NativeAfterInterManager] Preload next native {nextPos} for {uiName1}");
                    manager1.Request(nextPos);
                    break;
                }
            }
        }

        private NativeUIManager GetNativeManager(string name)
        {
            if (managers.TryGetValue(name, out var result)) return result;

            foreach (Transform child in parent.transform)
            {
                if (child.name == name)
                {
                    var manager = child.GetComponent<NativeUIManager>();
                    if (manager != null)
                    {
                        managers[name] = manager;
                        return manager;
                    }
                }
            }

            return null;
        }

        private UINativeController GetUIController(string name)
        {
            if (uiControllers.TryGetValue(name, out var result)) return result;

            foreach (Transform child in parent.transform)
            {
                if (child.name == name)
                {
                    var ui = child.GetComponent<UINativeController>();
                    if (ui != null)
                    {
                        uiControllers[name] = ui;
                        return ui;
                    }
                }
            }

            return null;
        }
    }
}