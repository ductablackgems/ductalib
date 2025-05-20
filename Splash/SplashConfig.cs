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
        
    }
    [Serializable]
    public class IntroConfig : SplashBaseConfig
    {
        public int delayShowButtonTime;
        public bool isAutoNext;
        public int nextTIme;

        
    }
    [Serializable]
    public class SelectLanguageConfig : SplashBaseConfig
    {
        
    }
    [Serializable]
    public class SplashCustomConfigValue
    {
        public bool loadIntro = false;
        public List<SplashConfig> splashConfigs = new List<SplashConfig>();
        public SelectAgeConfig selectAgeConfig;
        public IntroConfig introConfig;
        public SelectLanguageConfig selectLanguageConfig;
    }
}