using System.Collections.Generic;
using UnityEngine;

namespace _0.DucTALib.Scripts.Common.Pool
{
    public class DemoPoolManager : SingletonMono<DemoPoolManager>
    {
        public DemoItemPool prefab;
        public Transform waitingParent;
        private PoolingObj<DemoItemPool> pools = new();
        
        public DemoItemPool SpawnItem(Transform parent)
        {
            DemoItemPool newObject = pools.GetObject("DemoItemPool", prefab, parent);
            newObject.Init();
            return newObject;
        }

        public void RemoveItem(DemoItemPool item)
        {
            string key = item.name.Replace("(Clone)", "").Trim();
            pools.ReturnObject(key, item, waitingParent);
        }
    }
}