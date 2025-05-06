using System;
using System.Collections.Generic;
using _0.DucTALib.Splash;
using BG_Library.NET;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace _0.DucTALib.Scripts.Common
{
    [Serializable]
    public class CustomConfigValue
    {
        public List<SplashConfig> splashConfigs = new List<SplashConfig>();
        public bool loadIntro = false;
    }

    public class SplashRemoteConfig : MonoBehaviour
    {
        public static CustomConfigValue CustomConfigValue;
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
            CustomConfigValue = JsonConvert.DeserializeObject<CustomConfigValue>(RemoteConfig.Ins.custom_config, settings);
        }
    }
}