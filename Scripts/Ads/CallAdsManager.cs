using System;
using GoogleMobileAds.Api;

using _0.DucLib.Scripts.Common;
using BG_Library.Common;
using BG_Library.NET;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public static class CallAdsManager
    {
        public static string sceneName;
        public static Action rewardNotReadyAction;

        private static readonly IAdsPlatform _impl;

        static CallAdsManager()
        {
#if UNITY_ANDROID && USE_ANDROID_MEDIATION
            _impl = new AndroidAdsPlatform();
#elif UNITY_ANDROID
            _impl = new NoAdsPlatform();
#elif UNITY_IOS
            _impl = new IosAdsPlatform();
#endif
        }

        #region LoadAds

        public static void LoadBanner()
        {
            _impl.InitBanner();
        }

        public static void LoadMrec()
        {
            _impl.InitMREC();
        }

        public static void LoadInterByGroup(string group)
        {
            _impl.InitInter(group);
        }

        public static void LoadReward()
        {
            _impl.InitReward();
        }

        #endregion

        #region Call ADS
        public static void InitBannerLoading() => _impl.InitBannerLoading();
        public static void ShowBannerLoading() => _impl.ShowBannerLoading();
        public static void HideBannerLoading() => _impl.HideBannerLoading();
        public static void DestroyBannerLoading() => _impl.DestroyBannerLoading();
        public static bool BannerLoadingReady() => _impl.BannerLoadingReady();

        public static void ShowInter(string pos, Action complete = null) => _impl.ShowInter(pos, complete);

        public static bool RewardedIsReady() => _impl.RewardedIsReady();
        public static bool ShowRewardVideo(string pos, Action actionDone) => _impl.ShowRewardVideo(pos, actionDone);

        public static void InitONA(string group) => _impl.InitONA(group);
        public static void ShowONA(string pos) => _impl.ShowONA(pos);
        public static void ClearONA(string pos) => _impl.ClearONA(pos);
        public static void CloseONA(string pos) => _impl.CloseONA(pos);
        public static void ShowBanner() => _impl.ShowBanner();
        public static void HideBanner() => _impl.HideBanner();

        public static void ShowCollapseBanner() => _impl.ShowCollapseBanner();
        public static void HideCollapseBanner() => _impl.HideCollapseBanner();

        public static void ShowMREC(GameObject target, string pos)
        {
            sceneName = pos;
            _impl.ShowMREC(target);
        }

        public static void ShowMREC(GameObject target, Camera cam, string pos)
        {
            sceneName = pos;
            _impl.ShowMREC(target, cam);
        }

        public static void ShowMREC(AdPosition adPosition, string pos)
        {
            sceneName = pos;
            _impl.ShowMREC(adPosition);
        }

        public static void HideMREC() => _impl.HideMREC();

        public static bool CheckInternet() => Application.internetReachability != NetworkReachability.NotReachable;
        

        #endregion
       
    }

    internal interface IAdsPlatform
    {
        void InitBanner();

        void InitMREC();
        void InitInter(string group);
        void InitReward();
        void InitBannerLoading();
        void ShowBannerLoading();
        void HideBannerLoading();
        void DestroyBannerLoading();
        bool BannerLoadingReady();

        void ShowInter(string pos, Action complete);
        bool RewardedIsReady();
        bool ShowRewardVideo(string pos, Action actionDone);

        void InitONA(string group);
        void ShowONA(string pos);
        void ClearONA(string pos);

        void CloseONA(string pos);

        void ShowBanner();
        void HideBanner();

        void ShowCollapseBanner();
        void HideCollapseBanner();
        void ShowMREC(GameObject target);
        void ShowMREC(GameObject target, Camera cam);
        void ShowMREC(AdPosition adPosition);
        void HideMREC();
    }

    // ===================== IGNORE_ADS =====================


    // ===================== ANDROID (no editor) =====================
#if UNITY_ANDROID && USE_ANDROID_MEDIATION
    internal sealed class AndroidAdsPlatform : IAdsPlatform
    {
        public void InitBanner()
        {
            AdsManager.InitBannerManually();
        }

        public void InitMREC()
        {
            AdsManager.InitMrecManually();
        }

        public void InitInter(string group)
        {
            LogHelper.CheckPoint($"load inter {group}");
            AdsManager.InitInterstitialManually(group);
        }

        public void InitReward()
        {
            AdsManager.InitRewardManually();
        }

        public void InitBannerLoading()
        {
            Game3DCore2.InitializeBNNA();
        }

        public void ShowBannerLoading()
        {
            Game3DCore2.ShowBNNA();
        }

        public void HideBannerLoading()
        {
            Game3DCore2.HideBNNA();
        }

        public void DestroyBannerLoading()
        {
            Game3DCore2.ClearBNNA();
        }

        public bool BannerLoadingReady() => Game3DCore2.IsBNNAReady();

        public void ShowInter(string pos, Action complete)
        {
            LogHelper.CheckPoint($"show inter {pos}");
            AdsManager.ShowInterstitial(pos);
            complete?.Invoke();
        }

        public bool RewardedIsReady() => AdsManager.IsRewardedReady;

        public bool ShowRewardVideo(string pos, Action actionDone)
        {
            if (!RewardedIsReady())
            {
                CallAdsManager.rewardNotReadyAction?.Invoke();
                return false;
            }

            return AdsManager.ShowRewardVideo(pos, actionDone);
        }

        public void InitONA(string group)
        {
            LogHelper.CheckPoint($"load ONA {group}");
            Game3DCore2.InitializeONA(group);
        }

        public void ShowONA(string pos)
        {
            Game3DCore2.ShowONA(pos, 0, 0);
        }

        public void ClearONA(string pos)
        {
            Game3DCore2.ClearONA(pos);
        }

        public void CloseONA(string pos)
        {
            Game3DCore2.CloseONA(pos);
        }

        public void ShowBanner()
        {
            AdsManager.ShowBanner();
        }

        public void HideBanner()
        {
            AdsManager.HideBanner();
        }

        public void ShowCollapseBanner()
        {
           Game3DCore2.ExpandBNNA();
        }

        public void HideCollapseBanner()
        {
            Debug.Log("Hide Collapse Banner");
        }

        public void ShowMREC(GameObject target)
        {
            AdsManager.ShowMrec(target);
            AdsManager.UpdatePos(target);
        }

        public void ShowMREC(GameObject target, Camera cam)
        {
            AdsManager.ShowMrec(target, cam);
            AdsManager.UpdatePos(target, cam);
        }

        public void ShowMREC(AdPosition adPosition)
        {
            AdsManager.ShowMrec((int)adPosition);
            AdsManager.UpdatePos((int)adPosition);
        }

        public void HideMREC()
        {
            AdsManager.HideMrec();
        }
    }
#elif UNITY_ANDROID
    internal sealed class NoAdsPlatform : IAdsPlatform
    {
        public void InitBanner()
        {
            Log("InitBanner");
        }

        public void InitMREC()
        {
            Log("InitMREC");
        }

        public void InitInter(string group)
        {
            Log($"InitInter {group}");
        }

        public void InitReward()
        {
            Log("InitReward");
        }

        private static void Log(string m) => Debug.Log($"[Ads/Editor] {m}");

        public void InitBannerLoading()
        {
            Log("InitBannerLoading");
        }

        public void ShowBannerLoading()
        {
            Log("ShowBannerLoading");
        }

        public void HideBannerLoading()
        {
            Log("HideBannerLoading");
        }

        public void DestroyBannerLoading()
        {
            Log("DestroyBannerLoading");
        }

        public bool BannerLoadingReady()
        {
            Log("BannerLoadingReady=false");
            return false;
        }

        public void ShowInter(string pos, Action complete)
        {
            Log($"ShowInter {pos}");
            complete?.Invoke();
        }

        public bool RewardedIsReady()
        {
            Log("RewardedIsReady=true");
            return true;
        }

        public bool ShowRewardVideo(string pos, Action actionDone)
        {
            Log($"ShowRewardVideo {pos}");
            actionDone?.Invoke();
            return true;
        }

        public void InitONA(string group)
        {
            Log($"InitONA {group}");
        }

        public void ShowONA(string pos)
        {
            Log($"ShowONA {pos}");
        }

        public void ClearONA(string pos)
        {
            Log($"ClearONA {pos}");
        }
 public void CloseONA(string pos){}
        public void ShowBanner()
        {
            Log("ShowBanner");
        }

        public void HideBanner()
        {
            Log("HideBanner");
        }

        public void ShowCollapseBanner()
        {
            Debug.Log("Show Collapse Banner");
        }

        public void HideCollapseBanner()
        {
            Debug.Log("Hide Collapse Banner");
        }

        public void ShowMREC(GameObject target)
        {
            Log("ShowMREC overlay");
        }

        public void ShowMREC(GameObject target, Camera cam)
        {
            Log($"ShowMREC screen ");
        }
 public void ShowMREC(AdPosition adPosition)
        {
            Log($"ShowMREC screen ");
        }

        public void HideMREC()
        {
            Log("HideMREC");
        }
    }
    // ===================== iOS (no editor) =====================
#elif UNITY_IOS
    internal sealed class IosAdsPlatform : IAdsPlatform
    {
  public void InitBanner()
        {
           AdsManager.InitBannerManually();
        }

        public void InitMREC()
        {
            AdsManager.InitMrecManually();
        }

        public void InitInter(string group)
        {
            AdsManager.InitInterstitialManually(group);
        }

        public void InitReward()
        {
           AdsManager.InitRewardManually();
        }

        // iOS không có Game3DCore2 – chỉ dùng AdsManager
        public void InitBannerLoading()
        {
        }

        public void ShowBannerLoading()
        {
        }

        public void HideBannerLoading()
        {
        }

        public void DestroyBannerLoading()
        {
        }

        public bool BannerLoadingReady() => false;

        public void ShowInter(string pos, Action complete)
        {
            LogHelper.CheckPoint($"show inter (iOS) {pos}");
            AdsManager.ShowInterstitial(pos);
            complete?.Invoke();
        }

        public bool RewardedIsReady() => AdsManager.IsRewardedReady;

        public bool ShowRewardVideo(string pos, Action actionDone)
        {
            if (!RewardedIsReady())
            {
                CallAdsManager.rewardNotReadyAction?.Invoke();
                return false;
            }

            return AdsManager.ShowRewardVideo(pos, actionDone);
        }
 public void CloseONA(string pos){}
        public void InitONA(string group)
        {
            /* iOS ONA nếu có SDK riêng thì thêm sau */
        }

        public void ShowONA(string pos)
        {
        }

        public void ClearONA(string pos)
        {
        }

        public void ShowBanner()
        {
            AdsManager.ShowBanner();
        }

        public void HideBanner()
        {
            AdsManager.HideBanner();
        }

        public void ShowCollapseBanner()
        {
            LogHelper.CheckPoint();
        }

        public void HideCollapseBanner()
        {
            LogHelper.CheckPoint();
        }

          public void ShowMREC(GameObject target)
        {
            AdsManager.ShowMrec(target);
            AdsManager.UpdatePos(target);
        }

        public void ShowMREC(GameObject target, Camera cam)
        {
            AdsManager.ShowMrec(target, cam);
            AdsManager.UpdatePos(target, cam);
        }

        public void ShowMREC(AdPosition adPosition)
        {
            AdsManager.ShowMrec((int)adPosition);
            AdsManager.UpdatePos((int)adPosition);
        }
        public void HideMREC()
        {
            AdsManager.HideMrec();
        }
    }
#endif
}