using _0.DucLib.Scripts.Common;
using BG_Library.NET;
using Firebase.Analytics;
using UnityEngine;

namespace _0.DucTALib.Splash
{
    public static class SplashTracking 
    {
        public static bool loadMenuFirstTime = false;
        public static void TrackingIntro(string value)
        {
            var param = new Parameter("progress",value);
            LogHelper.LogPurple($"tracking_progress_{value}");
            FirebaseEvent.LogEvent("tracking_intro_progress",param);
        }
    }
}