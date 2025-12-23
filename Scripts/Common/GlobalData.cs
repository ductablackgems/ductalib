using _0.DucTALib.Scripts.Common;
using UnityEngine;

namespace _0.DucLib.Scripts.Common
{
    public static class GlobalData
    {
        private static string GeneratePlayerID(int length)
        {
            const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            System.Text.StringBuilder sb = new System.Text.StringBuilder(length);
            System.Random random = new System.Random();

            for (int i = 0; i < length; i++)
            {
                sb.Append(CHARS[random.Next(CHARS.Length)]);
            }

            return sb.ToString();
        }

        public static string PlayerID
        {
            get
            {
                string defaultId = GeneratePlayerID(8);
                return PlayerPrefHelper.GetString("PlayerID", defaultId);
            }
        }

        public static bool FirstTimeAction
        {
            get => PlayerPrefHelper.GetBool("FirstTimeAction");
            set => PlayerPrefHelper.SetBool("FirstTimeAction", value);
        }

        public static bool Reviewed
        {
            get => PlayerPrefHelper.GetBool("Reviewed");
            set => PlayerPrefHelper.SetBool("Reviewed", value);
        }

        public static int PlayDay
        {
            get => PlayerPrefHelper.GetInt("PlayDay");
            set => PlayerPrefHelper.SetInt("PlayDay", value);
        }

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