using System;
using System.Collections;
using _0.DucTALib.Splash;
using BG_Library.Common;
using BG_Library.NET;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _0.DucLib.Scripts.Ads
{
    public class LoadAdsManualy : SingletonMono<LoadAdsManualy>
    {

        public void LoadInterByGroup(string group)
        {
            AdsManager.InitInterstitialManually();
        }
    }
}