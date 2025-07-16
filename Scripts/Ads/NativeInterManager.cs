using System;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class NativeInterManager : MonoBehaviour
    {
        public List<NativeFullUI> nativeFullUis;
        private List<string> adsPosition = new List<string>();
        private List<string> adsDisplay = new List<string>();

        private Action showNextAds;

        private string nextDisplayName;
        private bool isActive;

        public void Load(string pos)
        {
            LogHelper.LogLine();
            var ads = CommonRemoteConfig.adsConfig.naInterConfigs.Find(x => x.pos.Contains(pos));
            if (ads == null) return;
            LogHelper.LogLine();
            isActive = ads.isEnabled;
            if (!isActive) return;
            LogHelper.LogLine();
            adsDisplay.AddRange(ads.displayName);
            nextDisplayName = adsDisplay[0];
            adsDisplay.RemoveAt(0);

            adsPosition.AddRange(ads.pos);
            adsPosition.Remove(pos);

            var nativeObj = nativeFullUis.Find(x => x.displayName == nextDisplayName);

            nativeObj?.Request(pos, adsPosition.Count == 0);
            LogHelper.CheckPoint($"[Load NA] {pos}");
        }

        private void LoadNext()
        {
            if (!isActive) return;
            var pos = adsPosition[0];
            nextDisplayName = adsDisplay[0];
            adsDisplay.RemoveAt(0);
            adsPosition.RemoveAt(0);


            var nativeObj = nativeFullUis.Find(x => x.displayName == nextDisplayName);

            nativeObj?.Request(pos, adsPosition.Count == 0);

            LogHelper.CheckPoint($"[Load NEXT NA] {pos}");
        }

        public void CallNA(string pos, Action complete)
        {
            if (!isActive)
            {
                CallAdsManager.ShowInter(pos);
                complete?.Invoke();
                return;
            }
            var nativeObj = nativeFullUis.Find(x => x.displayName == nextDisplayName);
            bool fakeNotrd = false;
            if (nativeObj == null || !nativeObj.native.IsReady)
            {
                CallAdsManager.ShowInter(pos);
                complete?.Invoke();
                return;
            }

            if (adsPosition.Count > 0)
            {
                nativeObj.SetAction(null, () => { CallNA(pos, complete); });
                if (nativeObj.ShowNA())
                    LoadNext();
            }
            else
            {
                nativeObj.SetAction(complete, null);
                nativeObj.ShowNA();
            }


            LogHelper.CheckPoint($"[Show NA] {pos}");
        }
    }
}