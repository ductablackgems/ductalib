using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace _0.DucTALib.Scripts.Common
{
    public static class PlayerPrefHelper
    {
         public static void SetBool(string name, bool value)
        {
            PlayerPrefs.SetInt(name, value ? 1 : 0);
            PlayerPrefs.SetInt($"{name}", value ? 1 : 0);
        }

        public static bool GetBool(string name, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(name, defaultValue ? 1 : 0) == 1;
        }

        // ---- INT ----
        public static void SetInt(string name, int value)
        {
            PlayerPrefs.SetInt(name, value);
        }

        public static int GetInt(string name, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(name, defaultValue);
        }

        // ---- FLOAT ----
        public static void SetFloat(string name, float value)
        {
            PlayerPrefs.SetFloat(name, value);
        }

        public static float GetFloat(string name, float defaultValue = 0f)
        {
            return PlayerPrefs.GetFloat(name, defaultValue);
        }

        // ---- STRING ----
        public static void SetString(string name, string value)
        {
            PlayerPrefs.SetString(name, value);
        }

        public static string GetString(string name, string defaultValue = "")
        {
            return PlayerPrefs.GetString(name, defaultValue);
        }

        // ---- VECTOR2 ----
        public static void SetVector2(string name, Vector2 value)
        {
            PlayerPrefs.SetString(name, $"{value.x},{value.y}");
        }

        public static Vector2 GetVector2(string name, Vector2 defaultValue)
        {
            string data = PlayerPrefs.GetString(name, $"{defaultValue.x},{defaultValue.y}");
            string[] values = data.Split(',');
            if (values.Length == 2 && float.TryParse(values[0], out float x) && float.TryParse(values[1], out float y))
            {
                return new Vector2(x, y);
            }

            return defaultValue;
        }

        // ---- VECTOR3 ----
        public static void SetVector3(string name, Vector3 value)
        {
            PlayerPrefs.SetString(name, $"{value.x},{value.y},{value.z}");
        }

        public static Vector3 GetVector3(string name, Vector3 defaultValue)
        {
            string data = PlayerPrefs.GetString(name, $"{defaultValue.x},{defaultValue.y},{defaultValue.z}");
            string[] values = data.Split(',');
            if (values.Length == 3 && float.TryParse(values[0], out float x) &&
                float.TryParse(values[1], out float y) && float.TryParse(values[2], out float z))
            {
                return new Vector3(x, y, z);
            }

            return defaultValue;
        }

        // ---- QUATERNION ----
        public static void SetQuaternion(string name, Quaternion value)
        {
            PlayerPrefs.SetString(name, $"{value.x},{value.y},{value.z},{value.w}");
        }

        public static Quaternion GetQuaternion(string name, Quaternion defaultValue)
        {
            string data = PlayerPrefs.GetString(name,
                $"{defaultValue.x},{defaultValue.y},{defaultValue.z},{defaultValue.w}");
            string[] values = data.Split(',');
            if (values.Length == 4 && float.TryParse(values[0], out float x) &&
                float.TryParse(values[1], out float y) && float.TryParse(values[2], out float z) &&
                float.TryParse(values[3], out float w))
            {
                return new Quaternion(x, y, z, w);
            }

            return defaultValue;
        }

        // ---- COLOR ----
        public static void SetColor(string name, Color color)
        {
            PlayerPrefs.SetString(name, $"{color.r},{color.g},{color.b},{color.a}");
        }

        public static Color GetColor(string name, Color defaultValue)
        {
            string data = PlayerPrefs.GetString(name,
                $"{defaultValue.r},{defaultValue.g},{defaultValue.b},{defaultValue.a}");
            string[] values = data.Split(',');
            if (values.Length == 4 && float.TryParse(values[0], out float r) &&
                float.TryParse(values[1], out float g) && float.TryParse(values[2], out float b) &&
                float.TryParse(values[3], out float a))
            {
                return new Color(r, g, b, a);
            }

            return new Color();
        }
        
        public static void SaveObject<T>(string key, T obj)
        {
            LogHelper.CheckPoint("save data");
            string json = JsonUtility.ToJson(obj);
            PlayerPrefs.SetString(key, json);
        }

        public static T LoadObject<T>(string key) where T : class, new()
        {
            if (PlayerPrefs.HasKey(key))
            {
                LogHelper.CheckPoint("load old value data");
                string json = PlayerPrefs.GetString(key);
                return JsonUtility.FromJson<T>(json);
            }
            var newValue = new T();
            string newJson = JsonUtility.ToJson(newValue); 
            PlayerPrefs.SetString(key, newJson);
            LogHelper.CheckPoint("load new value data");
            return newValue;
        }
        
        public static void ResetData(string key)
        {
            LogHelper.CheckPoint("reset data");
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
            }
        }
        
        public static void SetList<T>(string key, IList<T> list)
        {
            var json = JsonConvert.SerializeObject(list ?? new List<T>());
            PlayerPrefs.SetString(key, json);
        }

        public static List<T> GetList<T>(string key, List<T> defaultValue = null)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                var def = defaultValue ?? new List<T>();
                PlayerPrefs.SetString(key, JsonConvert.SerializeObject(def));
                return def;
            }

            var json = PlayerPrefs.GetString(key);
            try
            {
                return JsonConvert.DeserializeObject<List<T>>(json) ?? (defaultValue ?? new List<T>());
            }
            catch
            {
                return defaultValue ?? new List<T>();
            }
        }
    }
}