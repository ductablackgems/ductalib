using System;
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
        public string displayName;
        public NativeUIManager native;
        [SerializeField] private float timeClose;
        [SerializeField] private Image closeNativeFullImg;
        [SerializeField] private Text closeNativeFullTimeTxt;
        [SerializeField] private GameObject closeNativeFullButton;
        [ReadOnly] public bool isLastAds;
        public bool IsReady => native.IsReady;
        private Action onClose;
        private Action onShowNext;
        private Action adNotReady;


        public void Request(string pos, bool isLastAds)
        {
            this.isLastAds = isLastAds;
            native.Request(pos);
        }

        public void SetAction(Action close, Action showNext)
        {
            onClose = null;
            onShowNext = null;
            onClose = close;
            onShowNext = showNext;
        }

        public bool ShowNA()
        {
            if (!native.IsReady)
            {
                gameObject.HideObject();
                return false;
            }
            native.Show();
            StartCoroutine(DelayShowClose());
            return true;
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
                onShowNext?.Invoke();
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
                onClose?.Invoke();
            }
        }

        private void OnDestroy()
        {
            native.FinishNative();
        }
    }
}