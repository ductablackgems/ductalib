using System;
using System.Collections;
using GoogleMobileAds.Api;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using BG_Library.Common;
using BG_Library.NET;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class CallAdsManager : MonoBehaviour
    {
        public static CallAdsManager instance;
        public static string sceneName;
        public static Action rewardNotReadyAction;
        private Coroutine expandBannerCR;
        private static IAdsPlatform _impl;
        private bool eventAdded = false;
        private int currentInter = 0;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            Setup();
        }

        #region Setup

        private static void Setup()
        {
#if UNITY_ANDROID && USE_ANDROID_MEDIATION && !IGNORE_ADS
            _impl = new AndroidAdsPlatform();
#elif UNITY_ANDROID && IGNORE_ADS
            _impl = new NoAdsPlatform();
#elif UNITY_IOS
            _impl = new IosAdsPlatform();
#endif
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            BG_Event.AdmobMediation.Mrec.OnAdLoaded += MRECLoadDone;
#if USE_ANDROID_MEDIATION
            AndroidMediationEvent.BannerNative.OnBannerCollapsed += OnBannerCollapsed;
#endif
        }

        public void AddEndCardEvent()
        {
#if USE_ANDROID_MEDIATION
            if (eventAdded) return;
            eventAdded = true;
            AndroidMediationEvent.FullScreenNative.OnAdFullScreenContentClosed += CallEndCard;
            InitONA("endcard");
#endif
        }

        private void OnDestroy()
        {
            BG_Event.AdmobMediation.Mrec.OnAdLoaded -= MRECLoadDone;
#if USE_ANDROID_MEDIATION
            AndroidMediationEvent.FullScreenNative.OnAdFullScreenContentClosed -= CallEndCard;
            AndroidMediationEvent.BannerNative.OnBannerCollapsed -= OnBannerCollapsed;
#endif
        }


        private void CallEndCard(string groupName)
        {
#if USE_ANDROID_MEDIATION
            currentInter += 1;
            if (currentInter >= CommonRemoteConfig.instance.commonConfig.interstitialsBeforeMRECCount)
            {
                currentInter = 0;
                StartCoroutine(CallEndCardFullScreen());
            }
            else LogHelper.CheckPoint($"Interstitial count not reached MREC threshold : {currentInter}");
#endif
        }

        private IEnumerator CallEndCardFullScreen()
        {
            yield return new WaitForSecondsRealtime(15f);
            CallAdsManager.ShowONA("endcard");
        }

        private void MRECLoadDone(string a, ResponseInfo info)
        {
            LogHelper.CheckPoint();
            if (CallAdsManager.sceneName == "")
                AdsManager.HideMrec();
        }

        #endregion

        #region LoadAds

        public void FinishSplash()
        {
            LoadBanner();
#if USE_ANDROID_MEDIATION
            if (Game3DCore2.BNNAIsShowing())
            {
                HideBannerNA();
                ClearBannerNA();
            }
#endif
            LoadInterByGroup("gameplay");
            LoadInterByGroup("break");
            LoadReward();
            AddEndCardEvent();
        }

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

        #region Banner & Collapse

        public void ShowBannerGameplay()
        {
            LogHelper.CheckPoint();
            HideBanner();
            if (!BannerNAReady()) InitBannerNA();
            ShowBannerNA();
            ShowCollapseBanner();
        }

        public void ShowBannerMenu()
        {
            LogHelper.CheckPoint();
            StopAutoExpandBanner();
            ShowBanner();
            HideBannerNA();
            if (BannerNAReady()) ClearBannerNA();
        }

        private void OnBannerCollapsed()
        {
            LogHelper.CheckPoint("Close collapsed banner, start countdown");
            StartCDExpandBanner();
        }

        private void StartCDExpandBanner()
        {
            if (!CommonRemoteConfig.instance.commonConfig.expandBanner) return;
            if (expandBannerCR != null) StopCoroutine(expandBannerCR);
            expandBannerCR = StartCoroutine(AutoExpandBannerIE());
        }

        private IEnumerator AutoExpandBannerIE()
        {
            var time = CommonRemoteConfig.instance.commonConfig.timeExpandBanner;
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }

            ShowCollapseBanner();
        }

        private void StopAutoExpandBanner()
        {
            if (expandBannerCR != null) StopCoroutine(expandBannerCR);
        }

        #endregion


        #region Call ADS

        public static void InitBannerNA() => _impl.InitBannerNA();
        public static void ShowBannerNA() => _impl.ShowBannerNA();
        public static void HideBannerNA() => _impl.HideBannerNA();
        public static void ClearBannerNA() => _impl.DestroyBannerNA();
        public static bool BannerNAReady() => _impl.BannerNAReady();

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

        internal interface IAdsPlatform
        {
            void InitBanner();

            void InitMREC();
            void InitInter(string group);
            void InitReward();
            void InitBannerNA();
            void ShowBannerNA();
            void HideBannerNA();
            void DestroyBannerNA();
            bool BannerNAReady();
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
#if UNITY_ANDROID && USE_ANDROID_MEDIATION && !IGNORE_ADS
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
                LogHelper.CheckPoint($"Load inter {group}");
                AdsManager.InitInterstitialManually(group);
            }

            public void InitReward()
            {
                AdsManager.InitRewardManually();
            }

            public void InitBannerNA()
            {
                LogHelper.CheckPoint();
                Game3DCore2.InitializeBNNA();
            }

            public void ShowBannerNA()
            {
                Game3DCore2.ShowBNNA();
            }

            public void HideBannerNA()
            {
                Game3DCore2.HideBNNA();
            }

            public void DestroyBannerNA()
            {
                Game3DCore2.ClearBNNA();
            }

            public bool BannerNAReady() => Game3DCore2.IsBNNAReady();

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
                LogHelper.CheckPoint();
                Game3DCore2.ShowONA(pos, 0, 0);
            }

            public void ClearONA(string pos)
            {
                LogHelper.CheckPoint();
                Game3DCore2.ClearONA(pos);
            }

            public void CloseONA(string pos)
            {
                LogHelper.CheckPoint();
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
#elif UNITY_ANDROID && IGNORE_ADS
        internal sealed class NoAdsPlatform : IAdsPlatform
        {
            public void InitBanner()
            {
                LogHelper.CheckPoint();
            }

            public void InitMREC()
            {
                LogHelper.CheckPoint();
            }

            public void InitInter(string group)
            {
                LogHelper.CheckPoint($"InitInter {group}");
            }

            public void InitReward()
            {
                LogHelper.CheckPoint();
            }


            public void InitBannerNA()
            {
                LogHelper.CheckPoint();
            }

            public void ShowBannerNA()
            {
                LogHelper.CheckPoint();
            }

            public void HideBannerNA()
            {
                LogHelper.CheckPoint();
            }

            public void DestroyBannerNA()
            {
                LogHelper.CheckPoint();
            }

            public bool BannerNAReady()
            {
                LogHelper.CheckPoint();
                return true;
            }

            public void ShowInter(string pos, Action complete)
            {
                LogHelper.CheckPoint();
                complete?.Invoke();
            }

            public bool RewardedIsReady()
            {
                LogHelper.CheckPoint();
                return true;
            }

            public bool ShowRewardVideo(string pos, Action actionDone)
            {
                LogHelper.CheckPoint($"ShowRewardVideo {pos}");
                actionDone?.Invoke();
                return true;
            }

            public void InitONA(string group)
            {
                LogHelper.CheckPoint();
            }

            public void ShowONA(string pos)
            {
                LogHelper.CheckPoint();
            }

            public void ClearONA(string pos)
            {
                LogHelper.CheckPoint();
            }

            public void CloseONA(string pos) => LogHelper.CheckPoint();

            public void ShowBanner()
            {
                LogHelper.CheckPoint();
            }

            public void HideBanner()
            {
                LogHelper.CheckPoint();
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
                LogHelper.CheckPoint();
            }

            public void ShowMREC(GameObject target, Camera cam)
            {
                LogHelper.CheckPoint();
            }

            public void ShowMREC(AdPosition adPosition)
            {
                LogHelper.CheckPoint();
            }

            public void HideMREC()
            {
                LogHelper.CheckPoint();
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
        public void InitBannerNA()
        {
        }

        public void ShowBannerNA()
        {
        }

        public void HideBannerNA()
        {
        }

        public void DestroyBannerNA()
        {
        }

        public bool BannerNAReady() => false;

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
}