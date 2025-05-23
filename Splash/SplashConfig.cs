using System;
using System.Collections.Generic;
using _0.DucTALib.CustomButton;
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
    }
    [Serializable]
    public class SelectAgeConfig : SplashBaseConfig
    {
        public int nextTime;
        public string textValue;
        public string buttonColor;
        public string textColor;
    }
    [Serializable]
    public class IntroConfig : SplashBaseConfig
    {
        public int delayShowButtonTime;
        public int tutorialCount;
        public bool isAutoNext;
        public int nextTime;

        
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
        public List<SplashConfig> splashConfigs = new List<SplashConfig>();
        public SelectAgeConfig selectAgeConfig;
        public IntroConfig introConfig;
        public SelectLanguageConfig selectLanguageConfig;
    }
}