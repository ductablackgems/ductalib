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
    public class SplashRemoteConfig : SingletonMono<SplashRemoteConfig>
    {
        public static SplashCustomConfigValue CustomConfigValue;
        private void Awake()
        {
            RemoteConfig.OnFetchComplete += FetchComplete;
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            RemoteConfig.OnFetchComplete -= FetchComplete;
        }

        public void FetchComplete()
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            JObject root = JObject.Parse(RemoteConfig.Ins.custom_config);
            CustomConfigValue = root["Splash"]?.ToObject<SplashCustomConfigValue>(JsonSerializer.Create(settings));
        }
       
    }
}