using System;
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