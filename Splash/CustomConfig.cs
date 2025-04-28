using System;
using System.Collections.Generic;
using BG_Library.NET;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace _0.DucTALib.Splash
{
    [Serializable]
    public class CustomConfigValue
    {
        public List<SplashConfig> splashConfigs = new List<SplashConfig>();
        public bool loadIntro = false;
    }
    public class CustomConfig : MonoBehaviour
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
            // CustomConfigValue = JsonUtility.FromJson<CustomConfigValue>(RemoteConfig.Ins.custom_config);
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            CustomConfigValue = JsonConvert.DeserializeObject<CustomConfigValue>(RemoteConfig.Ins.custom_config, settings);
        }
    }
}