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
        public AdFormatType adsType;

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
    public class SelectLanguageConfig : SplashBaseConfig
    {
    }

    [Serializable]
    public class SplashCustomConfigValue
    {
        public bool loadIntro = false;
        public int testSegment;
        public bool interComplete;
        public int timeoutMin;
        public int timeoutMax;
        public List<SplashConfig> splashConfigs = new List<SplashConfig>();
        public SelectAgeConfig selectAgeConfig;
        public IntroConfig introConfig;
        public SelectLanguageConfig selectLanguageConfig;
    }

    [Serializable]
    public class GameplayNativeConfig
    {
        public List<GameplayNative> configs;
        public bool isFakeRating;
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
        public List<NativeAfterInterConfig> naConfigs;
        public List<NativeInterConfig> naInterConfigs;
        public bool IsUIActive(string uiName)
        {
            return naConfigs.Find(x => x.nativeUIName == uiName).isEnabled;
        }
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
        public List<string> nativePosition;
        public string nativeUIName;
        public List<string> interAdPositions;
    }
}