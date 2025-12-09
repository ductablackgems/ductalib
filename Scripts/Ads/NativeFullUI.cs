#if USE_ADMOB_NATIVE
using BG_Library.NET.Native_custom;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Splash;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucLib.Scripts.Ads
{
    public class NativeFullUI : MonoBehaviour
    {
#if USE_ADMOB_NATIVE
        public NativeUIManager native;
        public float timeClose;
        public Text timeTxt;
        public Image fillTimeImg;
        public GameObject timeObj;
        public GameObject closeButton;
        public bool IsReady => native.IsReady;
        public Action<NativeFullUI> onClose;


        public void LoadAd(string pos)
        {
            native.Request(pos);
        }

        public bool ShowAds()
        {
#if UNITY_EDITOR
            native.ShowObject();
            StartCoroutine(CountToClose());
            return true;
#else
            if (native.IsReady)
            {
                native.Show();
                StartCoroutine(CountToClose());
                return true;
            }
#endif


            return false;
        }

        private IEnumerator CountToClose()
        {
            timeObj.ShowObject();
            timeTxt.text = $"{(int)timeClose}";
            fillTimeImg.fillAmount = 1;
            closeButton.HideObject();
            float timeAds = timeClose;
            while (timeAds > 0)
            {
                timeAds -= Time.deltaTime;
                timeTxt.text = $"{(int)timeAds}";
                float fill = timeAds / timeClose;
                fillTimeImg.fillAmount = fill;
                yield return null;
            }

            timeObj.HideObject();
            closeButton.ShowObject();
        }

        public void CloseAds()
        {
            onClose?.Invoke(this);
            native.FinishNative();
#if UNITY_EDITOR
            native.HideObject();
#else
             native.FinishNative();
#endif
        }
#endif
    }
}