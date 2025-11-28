using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Ads.Native;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Scripts.Loading;
using _0.DucTALib.Splash.Scripts;
using BG_Library.Common;
using BG_Library.NET;
using Random = UnityEngine.Random;
#if USE_ADMOB_NATIVE
using BG_Library.NET.Native_custom;
#endif

namespace _0.DucTALib.Splash
{
#if USE_ADMOB_NATIVE
    [Serializable]
    public class NativeObject
    {
        public string nativeUIName;
        public string adsPosition;
        public NativeUIManager native;
        public bool isNativeFull;
    }

#endif

    public enum AdFormatType
    {
        Native,
        MREC,
        Inter,
        Banner,
        AppOpen,
    }

    public enum SplashType
    {
        Age,
        Intro,
        Language,
        Reward
    }

    public enum PagePosition
    {
        Left,
        Right
    }
    
}