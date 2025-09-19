using System;
using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using BG_Library.Common;

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
#if USE_ADMOB_NATIVE
using BG_Library.NET.Native_custom;
#endif
namespace _0.DucLib.Scripts.Ads
{
    public class NativeInterManager : MonoBehaviour
    {
#if USE_ADMOB_NATIVE
  public List<NativeFullUI> nativeFullUis;

        private Action showNextAds;

        private string nextDisplayName;
        private bool isActive;
        

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
#endif
      
    }
}