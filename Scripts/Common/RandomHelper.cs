using System;
using System.Collections.Generic;

namespace _0.DucTALib.Scripts.Common
{
    public static class RandomHelper
    {
        /// <summary>
        /// Trả về true/false ngẫu nhiên.
        /// </summary>
        public static bool RandomBool()
        {
            return UnityEngine.Random.value < 0.5f;
        }

        /// <summary>
        /// Trả về true nếu random ra nhỏ hơn tỉ lệ (%).
        /// Example: RandomByPercent(30) => ~30% true
        /// </summary>
        public static bool RandomByPercent(float percent)
        {
            return UnityEngine.Random.Range(0f, 100f) < percent;
        }

        /// <summary>
        /// Random int trong [min, max). 
        /// </summary>
        public static int RandomInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        /// <summary>
        /// Random float trong [min, max).
        /// </summary>
        public static float RandomFloat(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        /// <summary>
        /// Lấy ngẫu nhiên 1 phần tử trong list.
        /// </summary>
        public static T RandomElement<T>(IList<T> list)
        {
            if (list == null || list.Count == 0) throw new ArgumentException("List is null or empty");
            int index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        /// <summary>
        /// Xáo trộn list (Fisher-Yates shuffle).
        /// </summary>
        public static void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}