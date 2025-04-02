﻿using UnityEngine;

namespace _0.DucLib.Scripts.Common
{
    public static class GlobalData
    {
        public static bool IAP_RemoveAds
        {
            get => PlayerPrefs.GetInt("IAP_RemoveAds", 0) == 1;
            set => PlayerPrefs.SetInt("IAP_RemoveAds", value ? 1 : 0);
        }

        public static bool IsRate
        {
            get => PlayerPrefs.GetInt("IsRate", 0) == 1;
            set => PlayerPrefs.SetInt("IsRate", value ? 1 : 0);
        }

        public static int PlayerAge
        {
            get => PlayerPrefs.GetInt("PlayerAge", 0);
            set => PlayerPrefs.SetInt("PlayerAge", value);
        }
    }
}