using System;
using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Loading;
using _0.DucTALib.Splash;
using BG_Library.Common;
using BG_Library.NET;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;

namespace _0.DucTALib.Scripts.Common
{
    [Serializable]
    public class CommonConfig
    {
        public bool expandBanner;
        public float timeExpandBanner;
        public bool isProduct;
        public int testSegment;
        public int interstitialsBeforeMRECCount = 1;
        public float splashTime = 12;

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
    public class SplashConfig
    {
        public bool useIntro;
        public float loadingTime;
        public float introStepTime;
        public float endIntroTime;
        public int stepCount;
        public List<string> adPositions;
        public List<string> endIntroAdPositions;
        public List<string> tipText;


        public static SplashConfig CreateDefault()
        {
            var value = new SplashConfig
            {
                adPositions = new List<string>() { " " },
                endIntroAdPositions = new List<string>() { " " },
                tipText = new List<string>() { " " },
            };
            return value;
        }
    }

    public class CommonRemoteConfig : SingletonMono<CommonRemoteConfig>
    {
        public bool fetchComplete = false;

        public CommonConfig commonConfig;
        public SplashConfig splashConfig;
        public static Action FetchDone;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            StartCoroutine(WaitFirebaseInit());
        }

        private IEnumerator WaitFirebaseInit()
        {
            yield return new WaitUntil(() => RemoteConfig.Ins.isDataFetched);
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            commonConfig = JObject.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("common_config").StringValue)
                .ToObject<CommonConfig>(JsonSerializer.Create(settings));

            splashConfig = JObject.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("splash_config").StringValue)
                .ToObject<SplashConfig>(JsonSerializer.Create(settings));

            fetchComplete = true;
        }


#if UNITY_EDITOR
        public string commonConfigDefault;
        public string splashConfigDefault;

        [Button]
        public void CreateCommonConfig()
        {
            var value = CommonConfig.CreateDefault();
            commonConfigDefault = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            });
        }

        [Button]
        public void CreateSplashConfig()
        {
            var value = SplashConfig.CreateDefault();
            splashConfigDefault = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            });
        }
#endif
    }
}