using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
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
        [ReadOnly]public NativeFullUI currentNative;
        private List<string> naPos = new List<string>();
        
        public void Refresh()
        {
            LogHelper.CheckPoint("NativeAdContainer refresh");
            currentNative = null;
            CallAdsManager.ShowBanner();
        }
        public void SetUp(NativeAfterInterConfig config)
        {
            currentData = config;
            naPos.AddRange(currentData.nativePosition);
            Request();
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
            currentNative.Request(pos, naPos.Count <= 0);
        }


        public void ShowBeforeAds()
        {
            gameObject.ShowObject();
            native[0].ShowObject();
        }
        public void Show()
        {
            if (!currentData.isEnabled || !currentNative.IsReady) return;
            CallAdsManager.HideBanner();
            currentNative.Show();
            Request();
           
            
        }
        public bool IsLastNative()
        {
            LogHelper.CheckPoint($"NativeAdContainer isLastNative {naPos.Count} ");
            return naPos.Count <=0;
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