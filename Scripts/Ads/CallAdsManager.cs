using System;
using _0.DucLib.Scripts.Common;
using BG_Library.Common;
using BG_Library.NET;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    [CreateAssetMenu(fileName = "CallAdsManager", menuName = "DucLib/CallAdsManager")]
    public class CallAdsManager : ResourceSOManager<CallAdsManager>
    {
        public static Action rewardNotReadyAction;
        public static string currentInterstitial => AdsManager.LastInterstitialPos;

        public static void ShowInter(string pos, Action actionDone = null)
        {
#if IGNORE_ADS
            actionDone?.Invoke();
            return;
#endif
            LogHelper.CheckPoint($"show inter {pos}");
            AdsManager.ShowInterstitial(pos.ToString());
            actionDone?.Invoke();
        }
        public  static void LoadInterByGroup(string group)
        {
            AdsManager.InitInterstitialManually(group);
        }
        public static bool RewardedIsReady()
        {
            return AdsManager.IsRewardedReady;
        }

        public static void ShowRewardVideo(string pos, Action actionDone)
        {
#if IGNORE_ADS
                actionDone?.Invoke();
                return;
#endif
            if (!RewardedIsReady())
            {
                rewardNotReadyAction?.Invoke();
                return;
            }

            AdsManager.ShowRewardVideo(pos, actionDone);
        }

        public static void ShowBanner()
        {
            AdsManager.ShowBanner();
        }

        public static void HideBanner()
        {
            AdsManager.HideBanner();
        }

       
        public static void ShowMREC(GameObject target)
        {
            LogHelper.CheckPoint("Call MREC overlay");
#if IGNORE_ADS
            return;
#endif
            AdsManager.ShowMrec(target);
        }

        public static void ShowMREC(GameObject target, Camera cam, string pos)
        {
            LogHelper.CheckPoint("Call MREC screen space");
#if IGNORE_ADS
            return;
#endif
            AdsManager.ScreenName = pos;
            AdsManager.ShowMrec(target, cam);
        }

        public static void UpdateMRECPosition(GameObject target, Camera cam, string pos)
        {
            AdsManager.ScreenName = pos;
            AdsManager.Ins.AdsCoreIns.UpdateMrecPos(target, cam);
        }
        public static void HideMRECApplovin()
        {
#if IGNORE_ADS
            return;
#endif
            AdsManager.HideMrec();
        }

    

        public static bool CheckInternet()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        public static void ShowNativeOverlay()
        {
            //AdsManager.Ins.nativeOverlayManager.ShowAd();
        }
        
        public static Vector2 GetMRECSize()
        {
            return AdsManager.GetSizeMrec;
        }

        public static float GetDPIDevice()
        {
            return Screen.dpi / 160f;
        }
        public static Vector2 GetSizeMrec(RectTransform image)
        {
            float screenDensity = GetDPIDevice();
            float mrecWidthPx = 300 * screenDensity;
            float mrecHeightPx = 250 * screenDensity;
           
            var canvas = Master.GetNearestCanvas(image);
            float referenceDpi = 160f;
            float scaleFactor = canvas.scaleFactor;

            float width = mrecWidthPx / scaleFactor;
            float height = mrecHeightPx / scaleFactor;
            return new Vector2(width, height);
        }

        public static void ResizeMREC(RectTransform image)
        {
            var size = GetSizeMrec(image);
            image.sizeDelta = new Vector2(size.x, size.y);
        }
    }
}