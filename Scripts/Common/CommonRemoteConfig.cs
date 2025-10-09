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
    public class CommonRemoteConfig : SingletonMono<CommonRemoteConfig>
    {
        public bool fetchComplete = false;
#if USE_ADMOB_NATIVE
          public SplashCustomConfigValue splashConfig;
        public RelocationNativeValue relocationNativeConfig;
#endif


        public CommonConfig commonConfig;
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
            yield return new WaitUntil(()=> RemoteConfig.Ins.isDataFetched);
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
#if USE_ADMOB_NATIVE
             splashConfig =
 JObject.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("splash_config").StringValue)
                .ToObject<SplashCustomConfigValue>(JsonSerializer.Create(settings));
            relocationNativeConfig =
 JObject.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("relocation_native_config").StringValue)
                .ToObject<RelocationNativeValue>(JsonSerializer.Create(settings));
#endif
            commonConfig = JObject.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("common_config").StringValue)
                .ToObject<CommonConfig>(JsonSerializer.Create(settings));
            fetchComplete = true;
        }
     


#if UNITY_EDITOR
        public string splashConfigDefault;
        public string relocationNativeDefault;
        public string commonConfigDefault;

        [Button]
        public void CreateSplashConfig()
        {
            var value = SplashCustomConfigValue.CreateDefault();
            splashConfigDefault = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            });
        }

        [Button]
        public void CreateRelocationConfig()
        {
            var value = RelocationNativeValue.CreateDefault();
            relocationNativeDefault = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            });
        }

        [Button]
        public void CreateCommonConfig()
        {
            var value = CommonConfig.CreateDefault();
            commonConfigDefault = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            });
        }
#endif
    }
}