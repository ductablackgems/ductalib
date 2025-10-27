using System;
using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using BG_Library.NET;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class TestMonet : MonoBehaviour
    {
        public DucTALib.Scripts.Common.UIDragObject trans;
        public GameObject objectImmersive;
        public TMP_InputField inputField;

        public void ShowAds()
        {
            CallAdsManager.ShowONA("Test1", trans.rect);
        }

        public void HideAds()
        {
            CallAdsManager.CloseONA("Test1");
        }

        public void LoadAds()
        {
            CallAdsManager.InitONA("Test1");
        }

        public void InitBN()
        {
            CallAdsManager.LoadBanner();
        }

        public void ShowBanner()
        {
            CallAdsManager.ShowBanner();
        }

        public void ShowCollapse()
        {
            CallAdsManager.ShowBannerCollapsible();
        }

        public void ShowInter()
        {
            string value = inputField.text;
            CallAdsManager.ShowInter(value);
        }

        public void InitInter()
        {
            string value = inputField.text;
            CallAdsManager.LoadInterByGroup(value);
        }

        public void ShowBN()
        {
            CallAdsManager.ShowBanner();
        }

        public void ShowRW()
        {
            CallAdsManager.ShowRewardVideo("", () =>
            {
                LogHelper.CheckPoint("reward done");
            });
        }

        public void ShowBNNA()
        {
            CallAdsManager.ShowBannerNA();
        }

        public void InitBNNA()
        {
            CallAdsManager.InitBannerNA();
        }

#if USE_IMMERSIVE_ADMOB
        public ImmersiveInGameDisplayAd ad;

        public void LoadImmersive()
        {
            ImmersiveInGameDisplayAd.Initialize(() =>
            {
                // Configure an Adloader with ad unit to be shown on loading screen
                ImmersiveInGameDisplayAdAspectRatio adSize = new ImmersiveInGameDisplayAdAspectRatio(1, 1);
                AdLoader adLoader = new AdLoader.Builder("ca-app-pub-8243023565158105/3998471597")
                    .ForImmersiveInGameDisplayAd()
                    .SetImmersiveInGameDisplayAdAspectRatio(adSize)
                    .Build();

                // Configure callbacks
                adLoader.OnImmersiveInGameDisplayAdLoaded += this.HandleImmersiveInGameDisplayAdLoaded;
                adLoader.OnAdLoadFailed += this.HandleAdLoadFailed;
                adLoader.OnImmersiveInGameDisplayAdPaidEvent += this.HandleAdPaidEvent;
                adLoader.OnImmersiveInGameDisplayAdClicked += this.HandleAdClicked;
                adLoader.OnImmersiveInGameDisplayAdImpression += this.HandleAdImpression;

                // Make the ad request
                var request = new AdRequest();
                adLoader.LoadAd(request);
            });
        }

        private void HandleImmersiveInGameDisplayAdLoaded(object sender,
            ImmersiveInGameDisplayAdEventArgs args)
        {
            LogHelper.CheckPoint();
            ad = args.ImmersiveInGameDisplayAd;
        }

        // Ad Load Failure.
        private void HandleAdLoadFailed(LoadAdError error)
        {
            Debug.Log("Immersive in-game display ad failed to load : " + error.GetMessage());
            // Retry ad load after a delay.
        }

        private void HandleAdImpression(object sender, EventArgs args)
        {
            Debug.Log("Immersive in-game ad impression fired for ad type ");
        }

        private void HandleAdClicked(object sender, EventArgs args)
        {
            Debug.Log("Immersive in-game ad clicked.");
        }

        private void HandleAdPaidEvent(AdValue adValue)
        {
            Debug.Log(String.Format("Immersive in-game ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        }
        public void ShowImmersiveAds()
        {
            ad.SetParent(objectImmersive);
            ad.SetLocalPosition(new Vector3(0, 0, -0.01f));
            ad.SetLocalRotation(new Quaternion(0, 0, 0, 0));
            ad.SetLocalScale(1.0f);
            ad.ShowAd();
        }
#endif
    }
}