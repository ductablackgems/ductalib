using System.Collections.Generic;
using UnityEngine;

namespace _0.DucTALib.Scripts.Common.Pool
{
    public class PoolingObj<T> where T : MonoBehaviour
    {
        private readonly Dictionary<string, Queue<T>> _pools = new();

        public T GetObject(string key, T prefab, Transform parent)
        {
            if (_pools.ContainsKey(key) && _pools[key].Count > 0)
            {
                T obj = _pools[key].Dequeue();
                obj.transform.SetParent(parent);
                obj.gameObject.SetActive(true);
                return obj;
            }

            T newObj = Object.Instantiate(prefab, parent);
            newObj.name = key + "(Clone)";
            return newObj;
        }

        public T GetObject(string key, T prefab, Vector3 pos, Quaternion rotation, Transform parent)
        {
            if (_pools.ContainsKey(key) && _pools[key].Count > 0)
            {
                T obj = _pools[key].Dequeue();
                obj.transform.position = pos;
                obj.transform.rotation = rotation;
                obj.transform.SetParent(parent);
                obj.gameObject.SetActive(true);
                return obj;
            }

            T newObj = Object.Instantiate(prefab, pos, rotation, parent);
            newObj.name = key + "(Clone)";
            return newObj;
        }

        public void ReturnObject(string key, T obj, Transform waitingParent)
        {
            if (!_pools.ContainsKey(key))
            {
                _pools[key] = new Queue<T>();
            }

            obj.gameObject.SetActive(false);
            obj.transform.SetParent(waitingParent);
            _pools[key].Enqueue(obj);
        }
    }
}