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

        public static void InitBannerLoading()
        {
#if IGNORE_ADS
            return;
#endif
#if UNITY_ANDROID
            Game3DCore2.InitializeBNNA();
#elif UNITY_IOS
#endif
        }

        public static void ShowBannerLoading()
        {
#if IGNORE_ADS
            return;
#endif
#if UNITY_ANDROID
            Game3DCore2.ShowBNNA();
#elif UNITY_IOS
#endif
        }
        public static void HideBannerLoading()
        {
#if IGNORE_ADS
            return;
#endif
#if UNITY_ANDROID
            Game3DCore2.HideBNNA();
#elif UNITY_IOS
#endif
        }
        public static void DestroyBannerLoading()
        {
#if IGNORE_ADS
            return;
#endif
#if UNITY_ANDROID
            Game3DCore2.InitializeBNNA();
#elif UNITY_IOS
#endif
        }

        public static bool BannerLoadingReady()
        {
#if IGNORE_ADS
            return false;
#endif
#if UNITY_ANDROID
            return Game3DCore2.IsBNNAReady();
#elif UNITY_IOS
            return false;
#endif
        }

        public static void ShowInter(string pos, Action complete = null)
        {
            LogHelper.CheckPoint($"show inter {pos}");
#if IGNORE_ADS
complete?.Invoke();
            return;
#endif
            AdsManager.ShowInterstitial(pos.ToString());
            complete?.Invoke();
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
        public static void InitONA(string group)
        {
#if IGNORE_ADS
            return ;
#endif
#if UNITY_ANDROID
            Game3DCore2.InitializeONA(group);
#elif UNITY_IOS
            return ;
#endif
        }
        public static void ShowONA(string pos)
        {
#if IGNORE_ADS
            return ;
#endif
#if UNITY_ANDROID
            Game3DCore2.ShowONA(pos, 0,0);
#elif UNITY_IOS
            return ;
#endif
        }

        public static void ClearONA(string pos)
        {
#if IGNORE_ADS
            return ;
#endif
#if UNITY_ANDROID
            Game3DCore2.ClearONA(pos);
#elif UNITY_IOS
            return ;
#endif
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