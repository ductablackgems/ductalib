using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Splash;
using BG_Library.Common;
using BG_Library.NET.Native_custom;
using UnityEngine;

namespace _0.DucTALib.Gameplay
{
    public class NativeRelocationController : MonoBehaviour
    {
        public List<NativeRelocationObj> NativeObjects;
        private void Awake()
        {

            var config = SplashRemoteConfig.GameplayNativeConfig.configs;
            if(NativeObjects.Count == 0) return;
            for (int i = 0; i < NativeObjects.Count; i++)
            {
                var obj = NativeObjects[i];
                var c = config.Find(x=>x.id == NativeObjects[i].id);
                if (c.active)
                {
                    obj.Show(c.adsPosition);
                }
                else
                {
                    obj.HideObject();
                }
            }
        }

        
      
    }
}