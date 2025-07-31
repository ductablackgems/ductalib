using System;
using System.Collections;
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

        public void Load()
        {
            int count = Mathf.Min(nativeFullUis.Count, CommonRemoteConfig.instance.splashConfig.completeIntroNative.Count);
            for (int i = 0; i < count; i++)
            {
                var na = nativeFullUis[i];
                var pos = CommonRemoteConfig.instance.splashConfig.completeIntroNative[i];

                na.native.Request(pos);
            }
        }

        public void Show(Action complete)
        {
            StartCoroutine(StartShowNA(complete));
        }

        private IEnumerator StartShowNA(Action complete)
        {
            NativeFullUI lastNative = new NativeFullUI();
            for (int i = 0; i < nativeFullUis.Count; i++)
            {
                var na = nativeFullUis[i];
                if (!na.native.IsReady) continue;
                if (lastNative != null) lastNative.native.FinishNative();
                lastNative = na;
                na.native.Show();
                na.closeNativeFullTimeTxt.HideObject();
                na.closeNativeFullImg.ShowObject();
                na.closeNativeFullButton.HideObject();
                float totalDelay = 5;
                float elapsed = 0f;
                while (elapsed < totalDelay)
                {
                    elapsed += Time.deltaTime;
                    float remaining = Mathf.Max(0, totalDelay - elapsed);
                    na.closeNativeFullTimeTxt.text = ((int)remaining).ToString();
                    na.closeNativeFullImg.fillAmount = elapsed / totalDelay;

                    yield return null;
                }

              
            }

            if (lastNative == null)
            {
                complete?.Invoke();
            }
            else
            {
                var btn = lastNative.closeNativeFullButton.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    complete?.Invoke();
                });
                lastNative.closeNativeFullTimeTxt.HideObject();
                lastNative.closeNativeFullImg.HideObject();
                lastNative.closeNativeFullButton.ShowObject();
            }
        }
    }
}