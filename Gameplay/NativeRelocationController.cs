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
    public class NativeRelocationController : SingletonMono<NativeRelocationController>
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
                obj.active = c.active;
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

        public void ShowId(int id)
        {
          var obj =   NativeObjects.Find(x => x.id == id);
          if (obj == null || !obj.active) return;
          obj.ShowObject();
        }
        public void HideId(int id)
        {
            var obj =   NativeObjects.Find(x => x.id == id);
            if (obj == null || !obj.active) return;
            obj.HideObject();
        }
    }
}