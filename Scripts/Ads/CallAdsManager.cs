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

        public static void ShowInter(string pos)
        {
            LogHelper.CheckPoint($"show inter {pos}");
            LogHelper.CheckPoint($"show inter {pos}");
#if IGNORE_ADS
            return;
#endif
            AdsManager.ShowInterstitial(pos.ToString());
        }
        public  static void LoadInterByGroup(string group)
        {
            LogHelper.CheckPoint($"load inter group {group}");
            AdsManager.InitInterstitialManually(group);
        }
        public static bool RewardedIsReady()
        {
            return AdsManager.IsRewardedReady;
        }

        public static bool ShowRewardVideo(string pos, Action actionDone)
        {
#if IGNORE_ADS
                actionDone?.Invoke();
                return true;
#endif
            if (!RewardedIsReady())
            {
                rewardNotReadyAction?.Invoke();
                return false;
            }

            return AdsManager.ShowRewardVideo(pos, actionDone);
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
            AdsManager.UpdatePos(target, cam);
        }
        public static void UpdateMRECPosition(GameObject target)
        {
            AdsManager.Ins.AdsCoreIns.UpdateMrecPos(target);
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