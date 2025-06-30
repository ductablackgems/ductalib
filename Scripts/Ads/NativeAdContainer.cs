using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Splash;
using BG_Library.NET.Native_custom;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class NativeAdContainer : MonoBehaviour
    {
        public string nativeUIName;
        [SerializeField] private List<NativeFullUI> native;
        [ReadOnly] public NativeAfterInterConfig currentData;
        [ReadOnly] public NativeFullUI currentNative;
        private List<string> naPos = new List<string>();

        public void Refresh()
        {
            currentNative = null;
            CallAdsManager.ShowBanner();
        }

        public void SetUp(NativeAfterInterConfig config)
        {
            currentData = config;
            naPos.AddRange(currentData.nativePosition);
            Request();
        }

        private void Close()
        {
            LogHelper.CheckPoint($"Close native after inter");
            Refresh();
            foreach (var config in CommonRemoteConfig.adsConfig.naConfigs)
            {
                if (config.nativeUIName == nativeUIName)
                {
                    SetUp(config);
                    LogHelper.CheckPoint($"reinitialized natuve ");
                    break;
                }
            }
        }

        public void Request()
        {
            if (!currentData.isEnabled) return;
            if (naPos.Count <= 0) return;
            var pos = naPos[0];
            naPos.RemoveAt(0);
            currentNative = (currentNative == null || native.IndexOf(currentNative) == 1)
                ? native[0]
                : native[1];
            Debug.Log($"NativeAdContainer::Request {currentNative.name}");
            currentNative.SetAction(Close, Show);
            currentNative.Request(pos, naPos.Count <= 0);
        }


        public void ShowBeforeAds()
        {
            gameObject.ShowObject();
            native[0].ShowObject();
        }

        public void Show()
        {
            if (!currentData.isEnabled || !currentNative.IsReady)
            {
                currentNative.HideObject();
                Close();
                CallAdsManager.ShowBanner();
                return;
            }
            CallAdsManager.HideBanner();
            if (currentNative.ShowNA())
            {
                Request();
            }
           
        }
        

        public bool ShouldShowForInter(string interPos)
        {
            return currentData != null &&
                   currentData.isEnabled &&
                   currentData.interAdPositions != null &&
                   currentData.interAdPositions.Contains(interPos);
        }
    }
}