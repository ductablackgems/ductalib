using System;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Splash;
using BG_Library.Common;
using BG_Library.NET;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace _0.DucTALib.Scripts.Common
{
    public class CommonRemoteConfig : SingletonMono<CommonRemoteConfig>
    {
        public static SplashCustomConfigValue CustomConfigValue;
        public static GameplayNativeConfig GameplayNativeConfig;
        public static D_AdsConfig adsConfig;
        public static Action FetchDone;
        private void Awake()
        {
            RemoteConfig.OnFetchComplete += FetchComplete;
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            RemoteConfig.OnFetchComplete -= FetchComplete;
        }

        private void FetchComplete()
        {
            LogHelper.LogLine();
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            JObject root = JObject.Parse(RemoteConfig.Ins.custom_config);
            LogHelper.LogLine();
            CustomConfigValue = root["Splash"]?.ToObject<SplashCustomConfigValue>(JsonSerializer.Create(settings));
            LogHelper.LogLine();
            GameplayNativeConfig = root["Gameplay"]?.ToObject<GameplayNativeConfig>(JsonSerializer.Create(settings));
            LogHelper.LogLine();
            adsConfig = root["AdsConfig"]?.ToObject<D_AdsConfig>(JsonSerializer.Create(settings));
            LogHelper.LogLine();
            FetchDone?.Invoke();
        }
       
    }
}