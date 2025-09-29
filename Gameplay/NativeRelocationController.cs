using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Splash;
using BG_Library.Common;
using UnityEngine;
#if USE_ADMOB_NATIVE
using BG_Library.NET.Native_custom;
#endif

namespace _0.DucTALib.Gameplay
{
    public class NativeRelocationController : SingletonMono<NativeRelocationController>
    {
        public List<NativeRelocationObj> NativeObjects;
#if USE_ADMOB_NATIVE

        protected override void Init()
        {
            base.Init();
#if IGNORE_ADS
            return;
#endif
            var config = CommonRemoteConfig.instance.relocationNativeConfig;
            if (NativeObjects.Count == 0) return;
            for (int i = 0; i < NativeObjects.Count; i++)
            {
                var obj = NativeObjects[i];
                var c = config.config.Find(x => x.id == NativeObjects[i].id);
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

        public bool ShowId(int id)
        {
            var obj = NativeObjects.Find(x => x.id == id);
            if (obj == null || !obj.active) return false;
            obj.ShowObject();
            return true;
        }

        public void HideId(int id)
        {
            var obj = NativeObjects.Find(x => x.id == id);
            if (obj == null || !obj.active) return;
            obj.HideObject();
        }
#else
     protected override void Init()
        {
            base.Init();
#if IGNORE_ADS
            return;
#endif
            foreach (var a in NativeObjects)
            {
                a.HideObject();
            }
        }

        public bool ShowId(int id)
        {
            return false;
        }

        public void HideId(int id)
        {
            
        }
#endif
    }
}