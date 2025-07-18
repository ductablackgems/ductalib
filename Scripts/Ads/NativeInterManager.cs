using System;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using BG_Library.Common;
using BG_Library.NET.Native_custom;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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


        #region NA

        private void Awake()
        {
            Setup();
        }

        [FoldoutGroup("NA")] public List<Sprite> btnSp;
        [FoldoutGroup("NA")] public List<NativeUIManager> na;
        [FoldoutGroup("NA")] public List<Texture> bgNative;
        [FoldoutGroup("NA")] public Texture iconSp;
        [FoldoutGroup("NA")] public List<Image> bg;
        [FoldoutGroup("NA")] public Sprite bgImg;
        [Button]
        public void Setup()
        {
            foreach (var a in bg)
            {
                a.sprite = bgImg;
            }

            for (int i = 0; i < na.Count; i++)
            {
                var a = na[i];
                Master.GetChildByName(a.gameObject, "CallToAction").GetComponent<Image>().sprite = btnSp[i];
                var bg = Master.GetChildByName(a.gameObject, "AdImage Size").transform.GetChild(0)
                    .GetComponent<RawImage>();
                var spBg = bgNative[Random.Range(0, bgNative.Count)];
                bg.texture = spBg;

                var icon = Master.GetChildByName(a.gameObject, "AdIcon").GetComponent<RawImage>();
                icon.texture = iconSp;
#if USE_ADMOB_NATIVE
                a.defaultIcon = iconSp;
                a.defaultImage = spBg;
#endif
            }
          
        }

        #endregion
        
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