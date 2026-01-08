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

    

    public class CommonRemoteConfig : SingletonMono<CommonRemoteConfig>
    {
        public bool fetchComplete = false;

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
            yield return new WaitUntil(() => RemoteConfig.Ins.isDataFetched);
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            commonConfig = JObject.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("common_config").StringValue)
                .ToObject<CommonConfig>(JsonSerializer.Create(settings));

           
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

       
#endif
    }
}