using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Splash;
using BG_Library.NET.Native_custom;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucLib.Scripts.Ads
{
    [RequireComponent(typeof(NativeUIManager))]
    public class NativeFullUI : MonoBehaviour
    {
        public NativeUIManager native;
        [SerializeField] private float timeClose;
        [SerializeField] private Image closeNativeFullImg;
        [SerializeField] private Text closeNativeFullTimeTxt;
        [SerializeField] private GameObject closeNativeFullButton;
        public NativeAdContainer controller;
        public bool isLastAds;
        public bool IsReady => native.IsReady;
        public void DelayShow()
        {
            StartCoroutine(DelayShowClose());
        }

        private string poss;
        public void Request(string pos, bool isLastAds)
        {
            poss = pos;
            this.isLastAds = isLastAds;
            native.Request(pos);
        }

        public void Show()
        {
          LogHelper.CheckPoint($"native rd {poss} {native.IsReady}");
            if(!native.IsReady) return;
            native.Show();
            StartCoroutine(DelayShowClose());
        }
        private IEnumerator DelayShowClose()
        {
            closeNativeFullTimeTxt.HideObject();
            closeNativeFullImg.ShowObject();
            closeNativeFullButton.HideObject();
            float totalDelay = timeClose;
            float elapsed = 0f;

            while (elapsed < totalDelay)
            {
                elapsed += Time.deltaTime;
                float remaining = Mathf.Max(0, totalDelay - elapsed);

                closeNativeFullTimeTxt.text = ((int)remaining).ToString();
                closeNativeFullImg.fillAmount = elapsed / totalDelay;

                yield return null;
            }

            if (!isLastAds)
            {
                controller.Show();
                OnCloseAds();
                yield break;
            }
            closeNativeFullTimeTxt.HideObject();
            closeNativeFullImg.HideObject();
            closeNativeFullButton.ShowObject();
        }
        public void OnCloseAds()
        {
            native.FinishNative();
            gameObject.HideObject();
            if (isLastAds)
            {
                controller.Refresh();
                controller.Request();
            }
        }

       
       
    }
}