﻿using System.Diagnostics;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using BG_Library.NET;
using Firebase.Analytics;
using UnityEngine;

namespace _0.DucTALib.Splash
{
    public static class SplashTracking
    {
        public static Stopwatch loading_duration = new Stopwatch();
        public static Stopwatch loadMenuDuration = new Stopwatch();
        public static bool isFirstTime = false;
        private static int CurrentGame
        {
            get => PlayerPrefs.GetInt("CurrentGame", 0);
            set => PlayerPrefs.SetInt("CurrentGame", value);
        }


        public static void SetUserProperty()
        {
            CurrentGame += 1;
            FirebaseEvent.SetUserProperty("current_game", $"{CurrentGame}");
            FirebaseEvent.SetUserProperty("test_segment", $"{SplashRemoteConfig.CustomConfigValue.testSegment}");
            LoadingStart();
        }

        public static void LoadingStart()
        {
            LogHelper.LogPurple($"[TRACKING] fn_loading_start");
            FirebaseEvent.LogEvent("fn_loading_start");
            loadMenuDuration.Reset();
            loadMenuDuration.Start();
        }

        public static void LoadingEnd()
        {
            loading_duration.Stop();
            int milliseconds = (int)loading_duration.ElapsedMilliseconds;
            LogHelper.LogPurple($"[TRACKING] fn_loading_end_{milliseconds}");
            Parameter paramTime = new Parameter("loading_duration", milliseconds.ToString());
            FirebaseEvent.LogEvent("fn_loading_end", paramTime);
        }

        public static void PolicyShow()
        {
            LogHelper.LogPurple($"[TRACKING] fn_policy_show");
            FirebaseEvent.LogEvent("fn_policy_show");
        }
        public static void PolicyEnd(bool isAutoClose)
        {
            string autoCloseStr = isAutoClose ? "1" : "0";
            LogHelper.LogPurple($"[TRACKING] fn_policy_end_isAutoClose_{autoCloseStr}");
            Parameter param = new Parameter("is_auto_close", autoCloseStr);
            FirebaseEvent.LogEvent("fn_policy_end", param);
        }

        // === Onboarding ===
        public static void OnboardingShow(int step)
        {
            string eventName = $"fn_onboarding_0{step}_show";
            LogHelper.LogPurple($"[TRACKING] {eventName}");
            FirebaseEvent.LogEvent(eventName);
        }

        public static void OnboardingNext(int step)
        {
            string eventName = $"fn_onboarding_0{step}_next";
            LogHelper.LogPurple($"[TRACKING] {eventName}");
            FirebaseEvent.LogEvent(eventName);
        }

        public static void ShowNativeFull()
        {
            string eventName = $"fn_show_native_full";
            LogHelper.LogPurple($"[TRACKING] {eventName}");
            FirebaseEvent.LogEvent(eventName);
        }
        public static void HomeShow()
        {
            if(isFirstTime) return;
            isFirstTime = true;
            loadMenuDuration.Stop();
            int milliseconds = (int)loading_duration.ElapsedMilliseconds;
            LogHelper.LogPurple($"[TRACKING] fn_home_show_{milliseconds}");
            Parameter paramTime = new Parameter("duration", milliseconds.ToString());
            FirebaseEvent.LogEvent("fn_home_show", paramTime);
        }
        
        public static void SelectLanguageShow()
        {
            LogHelper.LogPurple($"[TRACKING] fn_language_show");
            FirebaseEvent.LogEvent("fn_language_show");
        }
        
        public static void SelectLanguageHide()
        {
            LogHelper.LogPurple($"[TRACKING] fn_language_hide");
            FirebaseEvent.LogEvent("fn_language_hide");
        }
    }
}