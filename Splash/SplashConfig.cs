using System;
using System.Collections.Generic;
using _0.DucTALib.CustomButton;
using _0.DucTALib.Splash.Scripts;
using GoogleMobileAds.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace _0.DucTALib.Splash
{
    [Serializable]
    public class SplashConfig
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public SplashType type;
    }

    [Serializable]
    public class SplashBaseConfig
    {

        [JsonConverter(typeof(StringEnumConverter))]
        public ButtonCustom.ButtonType buttonType;

        [JsonConverter(typeof(StringEnumConverter))]
        public ButtonCustom.ButtonPosition buttonPos;

        public List<string> adsPosition;
    }

    [Serializable]
    public class SelectAgeConfig : SplashBaseConfig
    {
        public int nextTime;
        public string textValue;
        public string buttonColor;
        public string textColor;
        public float delayShowButtonTime;
        public StepSelectAge.NextType nextType;
    }

    [Serializable]
    public class IntroConfig : SplashBaseConfig
    {
        public int delayShowButtonTime;
        public int tutorialCount;
        public bool isAutoNext;
        public int delayShowClose;
        public int nextTime;
        public string textValue;
        public string buttonColor;
        public string textColor;
        public List<string> tipText;
    }

    [Serializable]
    public class SplashCustomConfigValue
    {
        public bool loadIntro = false;
        public int testSegment;
        public bool launchInter;
        public CompleteAdsType completeAdsType;
        public int timeoutMin;
        public int timeoutMax;
        public List<SplashConfig> splashConfigs = new List<SplashConfig>();
        public SelectAgeConfig selectAgeConfig;
        public IntroConfig introConfig;
    }

    

    [Serializable]
    public class GameplayNative
    {
        public int id;
        public string adsPosition;
        public bool active;
    }

    [Serializable]
    public class D_AdsConfig
    {
        public List<GameplayNative> relocationNative;
        public NativeAfterInterConfig naAfterInter;
        public List<NativeInterConfig> naInterConfigs;
    }

    [Serializable]
    public class NativeInterConfig
    {
        public bool isEnabled;
        public List<string> pos;
        public List<string> displayName;
        public bool isAutoReload;
    }
    [Serializable]
    public class NativeAfterInterConfig
    {
        public bool isEnabled;
        public bool isReload;
        public List<string> nativePosition;
        public List<string> nativeUIName;
        public List<string> interAdPositions;
    }

    [Serializable]
    public class CommonConfig
    {
        public bool isProduct;
    }

    public enum CompleteAdsType
    {
        Inter, NA, InterImage, InterVideo, Appopen
    }
}