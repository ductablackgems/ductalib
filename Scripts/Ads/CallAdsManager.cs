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

        public static void ShowInter(string pos, Action actionDone = null)
        {
            LogHelper.CheckPoint($"Show FA {pos}");
#if IGNORE_ADS
            actionDone?.Invoke();
            return;
#endif
            AdsManager.ShowInterstitial(pos.ToString());
            actionDone?.Invoke();
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
        }

        public static void HideBanner()
        {
        }

        public static void ShowMRECAdmob(Transform pos)
        {
        }

        public static void HideMRECAdmob()
        {
        }

        public static void DestroyMRECAdmob()
        {
        }

        public static void ShowMRECApplovin(GameObject target)
        {
            LogHelper.CheckPoint("Call MREC overlay");
#if IGNORE_ADS
            return;
#endif
            AdsManager.ShowMrec(target);
        }

        public static void ShowMRECApplovin(GameObject target, Camera cam, string pos)
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