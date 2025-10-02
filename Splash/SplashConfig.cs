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
    public class SplashBaseConfig
    {
        public List<string> adsPosition;
    }

    [Serializable]
    public class SelectAgeConfig : SplashBaseConfig
    {
        public int nextTime;
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
        public List<string> tipText;
    }

    [Serializable]
    public class SplashCustomConfigValue
    {
        public bool loadIntro = false;
        public int testSegment;
        public CompleteAdsType completeAdsType;
        public int timeoutMin;

        public List<SplashType> splashConfigs = new List<SplashType>();

        public SelectAgeConfig selectAgeConfig;
        public IntroConfig introConfig;
        public List<string> completeIntroNative;

        public static SplashCustomConfigValue CreateDefault()
        {
            var value = new SplashCustomConfigValue
            {
                testSegment = 1000,
                timeoutMin = 12,
                completeAdsType = CompleteAdsType.NA,

                splashConfigs = new List<SplashType>
                {
                    SplashType.Intro
                },
                selectAgeConfig = new SelectAgeConfig
                {
                    adsPosition = new List<string>
                    {
                        "Temp1"
                    }
                },
                introConfig = new IntroConfig
                {
                    delayShowButtonTime = 3,
                    nextTime = 4,
                    delayShowClose = 5,
                    tutorialCount = 4,
                    tipText = new List<string>
                    {
                        "Temp1", "Temp2", "Temp3", "Temp4"
                    },
                    adsPosition = new List<string>
                    {
                        "Tutorial_0",
                        "Tutorial_1",
                        "Tutorial_2",
                        "Tutorial_3"
                    }
                },
                completeIntroNative = new List<string>
                {
                    "complete_all_step", "complete_all_step_1"
                }
            };
            return value;
        }
    }


    [Serializable]
    public class RelocationNativeConfig
    {
        public int id;
        public string adsPosition;
        public bool active;
    }

    [Serializable]
    public class RelocationNativeValue
    {
        public List<RelocationNativeConfig> config;

        public static RelocationNativeValue CreateDefault()
        {
            var value = new RelocationNativeValue
            {
                config = new List<RelocationNativeConfig>
                {
                    new RelocationNativeConfig() { id = 0, adsPosition = "temp", active = false },
                    new RelocationNativeConfig() { id = 1, adsPosition = "temp", active = false }
                },
            };
            return value;
        }
    }


    [Serializable]
    public class CommonConfig
    {
        public bool isProduct;
        public int testSegment;
        public static CommonConfig CreateDefault()
        {
            var value = new CommonConfig
            {
                isProduct = false
            };
            return value;
        }
    }

    [Serializable]
    public class AndroidAdsConfig
    {
        public int interstitialsBeforeMRECCount =1;
    }
    public enum CompleteAdsType
    {
        Inter,
        NA,
        InterImage,
        InterVideo,
        Appopen
    }
}