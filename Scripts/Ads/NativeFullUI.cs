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
public string displayName;
        public NativeUIManager native;
        public float timeClose;
        public Image closeNativeFullImg;
        public Text closeNativeFullTimeTxt;
        public GameObject closeNativeFullButton;
        [ReadOnly] public bool isLastAds;
        public bool IsReady => native.IsReady;
        private Action onClose;
        private Action onShowNext;
        private Action adNotReady;
#endif
    }
}