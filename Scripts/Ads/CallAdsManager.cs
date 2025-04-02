using System;
using _0.DucLib.Scripts.Common;
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
            return AdsManager.IsRewardedReady();
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

        public static void ShowMRECApplovin(GameObject target,RenderMode renderMode)
        {
            LogHelper.CheckPoint("Call MREC");
#if IGNORE_ADS
            return;
#endif
            // AdsManager.ShowMrec(target, renderMode);
        }
        public static void HideMRECApplovin()
        {
#if IGNORE_ADS
            return;
#endif
            AdsManager.HideMrec();
        }

        public static void DestroyMRECApplovin()
        {
#if IGNORE_ADS
            return;
#endif
            // AdsManager.DesTroyMrec();
        }

        public static void LogEventFirebase(string str)
        {
            Debug.Log("Push Firebase Event: " + str);
        }

        public static bool CheckInternet()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        public static void ShowNativeOverlay()
        {
            //AdsManager.Ins.nativeOverlayManager.ShowAd();
        }

        public static void ShowNativeOverlay(Transform pos)
        {
            // AdsManager.Ins.nativeOverlayManager.RenderAd(pos);
            // AdsManager.Ins.nativeOverlayManager.ShowAd();
        }

        public static void HideNativeOverlay()
        {
           // AdsManager.Ins.nativeOverlayManager.HideAd();
        }
    }
}