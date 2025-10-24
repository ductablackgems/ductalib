using System;
using System.Collections;
using GoogleMobileAds.Api;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Splash;
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
        public static Action<string> OnOverlayCloseEvent;
        private Coroutine expandBannerCR;

        private static IAdsPlatform _impl
        {
            get
            {
                if (adsPlatform == null) Setup();
                return adsPlatform;
            }
        }

        private static IAdsPlatform adsPlatform;
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

        public void FinishSplash()
        {
            switch (CommonRemoteConfig.instance.commonConfig.bannerType)
            {
                case BannerType.Admob:
                    StopReloadBNNA();
                    LoadBanner();
                    HideBannerNA();
                    ClearBannerNA();
                    break;
                case BannerType.Mix:
                    LoadBanner();
                    HideBannerNA();
                    break;
                case BannerType.Android:
                    break;
            }

            StopReloadFS("launch");
            LoadInterByGroup("gameplay");
            LoadInterByGroup("break");
            LoadReward();
        }

        #region Setup

        private static void Setup()
        {
#if IGNORE_ADS
            adsPlatform = new NoAdsPlatform();
#elif UNITY_ANDROID && USE_ANDROID_MEDIATION
            adsPlatform = new AndroidAdsPlatform();
#else
            adsPlatform = new IosAdsPlatform();
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

        private void OnDestroy()
        {
            BG_Event.AdmobMediation.Mrec.OnAdLoaded -= MRECLoadDone;
#if USE_ANDROID_MEDIATION
            AndroidMediationEvent.BannerNative.OnBannerCollapsed -= OnBannerCollapsed;
#endif
        }

        private void MRECLoadDone(string a, ResponseInfo info)
        {
            LogHelper.CheckPoint();
            if (sceneName == "")
                HideMREC();
        }

        #endregion

        #region Banner & Collapse

        public void ShowBannerGameplay()
        {
#if UNITY_IOS
            return;
#endif
            switch (CommonRemoteConfig.instance.commonConfig.bannerType)
            {
                case BannerType.Admob:
                    break;
                case BannerType.Mix:
                    HideBanner();
                    ShowBannerNA();
                    ShowBannerCollapsibleNA();
                    break;
                case BannerType.Android:
                    break;
            }

            LogHelper.CheckPoint();
        }

        public void ShowBannerMenu()
        {
#if UNITY_IOS
            return;
#endif
            switch (CommonRemoteConfig.instance.commonConfig.bannerType)
            {
                case BannerType.Admob:
                    break;
                case BannerType.Mix:
                    StopAutoExpandBanner();
                    ShowBanner();
                    HideBannerNA();
                    break;
                case BannerType.Android:
                    break;
            }

            LogHelper.CheckPoint();
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

            ShowBannerCollapsibleNA();
        }

        private void StopAutoExpandBanner()
        {
            if (expandBannerCR != null) StopCoroutine(expandBannerCR);
        }

        #endregion

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

        public static void InitBannerNA() => _impl.InitBannerNA();
        public static void ShowBannerNA() => _impl.ShowBannerNA();
        public static void HideBannerNA() => _impl.HideBannerNA();
        public static void ClearBannerNA() => _impl.DestroyBannerNA();
        public static bool BannerNAReady() => _impl.BannerNAReady();

        public static bool BannerReady() => _impl.BannerReady();
        public static void ShowInter(string pos, Action complete = null) => _impl.ShowInter(pos, complete);

        public static bool IsInterPosReady(string pos) => _impl.IsInterPosReady(pos);

        public static void StopReloadFS(string group) => _impl.StopReloadFS(group);
        public static bool RewardedIsReady() => _impl.RewardedIsReady();
        public static bool ShowRewardVideo(string pos, Action actionDone) => _impl.ShowRewardVideo(pos, actionDone);

        public static void InitONA(string group) => _impl.InitONA(group);
        public static void ShowONA(string pos) => _impl.ShowONA(pos);
        public static void ShowONA(string pos, RectTransform objectPos) => _impl.ShowONA(pos, objectPos);
        public static void ClearONA(string pos) => _impl.ClearONA(pos);
        public static void CloseONA(string pos) => _impl.CloseONA(pos);
        public static bool ONAReady(string pos) => _impl.ONAReady(pos);

        public static void StopReloadONA(string group) => _impl.StopReloadONA(group);


        public static void ShowBanner() => _impl.ShowBanner();

        public static void ShowBannerCollapsible() => _impl.ShowBannerCollapsible();
        public static void HideBanner() => _impl.HideBanner();

        public static void ShowBannerCollapsibleNA() => _impl.ShowCollapseBannerNA();
        public static void HideBannerCollapsibleNA() => _impl.HideCollapseBannerNA();

        public static void StopReloadBNNA() => _impl.StopReloadBNNA();

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

        public static void HideMREC()
        {
            sceneName = "";
            _impl.HideMREC();
        }

        public static bool CheckInternet() => Application.internetReachability != NetworkReachability.NotReachable;
#if USE_IMMERSIVE_ADMOB
        public static void InitImmersive(string pos) => _impl.InitImmersive(pos);
        public static void ShowImmersive(string pos, GameObject adParent) => _impl.ShowImmersive(pos, adParent);
        public static bool ImmersiveIsReady(string pos) => _impl.ImmersiveReady(pos);
        public static void DestroyImmersive(string pos) => _impl.DestroyImmersive(pos);
#endif

        #endregion

        internal interface IAdsPlatform
        {
            public string sceneName { get; set; }

            #region INTER

            void InitInter(string group);
            void ShowInter(string pos, Action complete);
            bool IsInterPosReady(string pos);
            void StopReloadFS(string group);

            #endregion

            #region BANNER

            void InitBannerNA();
            void InitBanner();
            void ShowBanner();
            void ShowBannerCollapsible();
            void HideBanner();
            void ShowCollapseBannerNA();
            void HideCollapseBannerNA();
            void ShowBannerNA();
            void HideBannerNA();
            void DestroyBannerNA();
            bool BannerNAReady();
            void StopReloadBNNA();
            bool BNNAIsShowing();
            bool BannerReady();

            #endregion

            #region REWARD

            void InitReward();

            bool RewardedIsReady();
            bool ShowRewardVideo(string pos, Action actionDone);

            #endregion

            #region MREC

            void InitMREC();
            void ShowMREC(GameObject target);
            void ShowMREC(GameObject target, Camera cam);
            void ShowMREC(AdPosition adPosition);
            void HideMREC();

            #endregion

            #region OVERLAY

            void InitONA(string group);
            void ShowONA(string pos);
            void ShowONA(string pos, RectTransform objectPos);
            void ClearONA(string pos);
            void CloseONA(string pos);
            bool ONAReady(string pos);
            void StopReloadONA(string group);

            #endregion

#if USE_IMMERSIVE_ADMOB

            #region Immersive

            void InitImmersive(string pos);
            void ShowImmersive(string pos, GameObject adParent);
            void DestroyImmersive(string pos);
            bool ImmersiveReady(string pos);

            #endregion

#endif
        }


        internal sealed class NoAdsPlatform : IAdsPlatform
        {
            public string sceneName { get; set; }

            #region INTER

            public void InitInter(string group) => LogHelper.CheckPoint($"InitInter {group}");

            public void ShowInter(string pos, Action complete)
            {
                LogHelper.CheckPoint($"ShowInter {pos}");
                complete?.Invoke();
            }

            public bool IsInterPosReady(string group) => false;

            public void StopReloadFS(string group) => LogHelper.CheckPoint($"StopReloadFS {group}");

            #endregion

            #region BANNER

            public void InitBannerNA() => LogHelper.CheckPoint();
            public void InitBanner() => LogHelper.CheckPoint();
            public void ShowBanner() => LogHelper.CheckPoint();
            public void ShowBannerCollapsible() => LogHelper.CheckPoint();
            public void HideBanner() => LogHelper.CheckPoint();
            public void ShowCollapseBannerNA() => LogHelper.CheckPoint();
            public void HideCollapseBannerNA() => LogHelper.CheckPoint();
            public void ShowBannerNA() => LogHelper.CheckPoint();
            public void HideBannerNA() => LogHelper.CheckPoint();
            public void DestroyBannerNA() => LogHelper.CheckPoint();
            public bool BNNAIsShowing() => false;

            public bool BannerNAReady()
            {
                LogHelper.CheckPoint();
                return false;
            }

            public void StopReloadBNNA() => LogHelper.CheckPoint();
            public bool BannerReady() => AdsManager.Ins.AdsCoreIns.IsBNReady();

            #endregion

            #region REWARD

            public void InitReward() => LogHelper.CheckPoint();

            public bool RewardedIsReady()
            {
                LogHelper.CheckPoint();
                return false;
            }

            public bool ShowRewardVideo(string pos, Action actionDone)
            {
                LogHelper.CheckPoint();
                actionDone?.Invoke();
                return false;
            }

            #endregion

            #region MREC

            public void InitMREC() => LogHelper.CheckPoint();
            public void ShowMREC(GameObject target) => LogHelper.CheckPoint();
            public void ShowMREC(GameObject target, Camera cam) => LogHelper.CheckPoint();
            public void ShowMREC(AdPosition adPosition) => LogHelper.CheckPoint($"Show MREC {adPosition.ToString()}");
            public void HideMREC() => LogHelper.CheckPoint();

            #endregion

            #region OVERLAY

            public void InitONA(string group) => LogHelper.CheckPoint($"Init OnA {group}");
            public void ShowONA(string pos) => LogHelper.CheckPoint($"ShowONA {pos}");
            public void ShowONA(string pos, RectTransform objectPos) => LogHelper.CheckPoint($"ShowONA XY {pos}");
            public void ClearONA(string pos) => LogHelper.CheckPoint($"ClearONA OnA {pos}");
            public void CloseONA(string pos) => LogHelper.CheckPoint($"CloseONA OnA {pos}");

            public bool ONAReady(string pos)
            {
                return false;
            }

            public void StopReloadONA(string group) => LogHelper.CheckPoint($"StopReloadONA {group}");

            #endregion

#if USE_IMMERSIVE_ADMOB

            #region Immersive

            public void InitImmersive(string pos) => LogHelper.CheckPoint($"InitImmersive {pos}");
            public void ShowImmersive(string pos, GameObject adParent) => LogHelper.CheckPoint($"Show Immersive {pos}");

            public void DestroyImmersive(string pos) => LogHelper.CheckPoint($"Destroy immersive {pos}");

            public bool ImmersiveReady(string pos) => false;

            #endregion

#endif
        }
#if UNITY_ANDROID && USE_ANDROID_MEDIATION
        internal sealed class AndroidAdsPlatform : IAdsPlatform
        {
            public string sceneName { get; set; }

            #region INTER

            public void InitInter(string group)
            {
                LogHelper.CheckPoint($"Load inter {group}");
                AdsManager.InitInterstitialManually(group);
            }

            public void ShowInter(string pos, Action complete)
            {
                LogHelper.CheckPoint($"show inter {pos}");
                AdsManager.ShowInterstitial(pos);
                complete?.Invoke();
            }

            public bool IsInterPosReady(string pos) => AdsManager.IsInterstitialReady(pos);

            public void StopReloadFS(string group)
            {
                LogHelper.CheckPoint($"Stop  reload inter {group}");
                Game3DCore2.StopReloadFA(group);
            }

            #endregion

            #region BANNER

            public void InitBannerNA()
            {
                LogHelper.CheckPoint();
                Game3DCore2.InitializeBNNA();
            }

            public void InitBanner()
            {
                LogHelper.CheckPoint();
                AdsManager.InitBannerManually();
            }

            public void ShowBanner()
            {
                LogHelper.CheckPoint();
                AdsManager.ShowBanner();
            }

            public void ShowBannerCollapsible()
            {
                LogHelper.CheckPoint();
                AdsManager.ShowBannerCollapsible();
            }

            public void HideBanner()
            {
                LogHelper.CheckPoint();
                AdsManager.HideBanner();
            }

            public void ShowCollapseBannerNA()
            {
                LogHelper.CheckPoint();
                Game3DCore2.ExpandBNNA();
            }

            public void HideCollapseBannerNA()
            {
                Debug.Log("Hide Collapse Banner");
            }

            public void ShowBannerNA()
            {
                LogHelper.CheckPoint();
                Game3DCore2.ShowBNNA();
            }

            public void HideBannerNA()
            {
                LogHelper.CheckPoint();
                Game3DCore2.HideBNNA();
            }

            public void DestroyBannerNA()
            {
                LogHelper.CheckPoint();
                Game3DCore2.ClearBNNA();
            }

            public bool BannerNAReady() => Game3DCore2.IsBNNAReady();

            public void StopReloadBNNA()
            {
                LogHelper.CheckPoint();
                Game3DCore2.StopReloadBNNA();
            }

            public bool BNNAIsShowing() => Game3DCore2.BNNAIsShowing();

            public bool BannerReady() => AdsManager.Ins.AdsCoreIns.IsBNReady();

            #endregion

            #region REWARD

            public void InitReward()
            {
                LogHelper.CheckPoint();
                AdsManager.InitRewardManually();
            }

            public bool RewardedIsReady() => AdsManager.IsRewardedReady;

            public bool ShowRewardVideo(string pos, Action actionDone)
            {
                LogHelper.CheckPoint();
                if (!RewardedIsReady())
                {
                    CallAdsManager.rewardNotReadyAction?.Invoke();
                    return false;
                }

                return AdsManager.ShowRewardVideo(pos, actionDone);
            }

            #endregion

            #region MREC

            public void InitMREC()
            {
                LogHelper.CheckPoint();
                AdsManager.InitMrecManually();
            }

            public void ShowMREC(GameObject target)
            {
                LogHelper.CheckPoint();
                AdsManager.ShowMrec(target);
                AdsManager.UpdatePos(target);
            }

            public void ShowMREC(GameObject target, Camera cam)
            {
                LogHelper.CheckPoint();
                AdsManager.ShowMrec(target, cam);
                AdsManager.UpdatePos(target, cam);
            }

            public void ShowMREC(AdPosition adPosition)
            {
                LogHelper.CheckPoint();
                AdsManager.ShowMrec((int)adPosition);
                AdsManager.UpdatePos((int)adPosition);
            }

            public void HideMREC()
            {
                LogHelper.CheckPoint();
                AdsManager.DestroyMrec();
            }

            #endregion

            #region OVERLAY

            public void InitONA(string group)
            {
                LogHelper.CheckPoint($"load ONA {group}");
                Game3DCore2.InitializeONA(group);
            }

            public void ShowONA(string pos)
            {
                LogHelper.CheckPoint();
                Game3DCore2.ShowONA(pos);
            }

            public void ShowONA(string pos, RectTransform objectPos)
            {
                LogHelper.CheckPoint();
                var position = CommonHelper.ObjectToOverlayPos(objectPos);
                Game3DCore2.ShowONA(pos, position.x, position.y);
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

            public bool ONAReady(string pos)
            {
                LogHelper.CheckPoint();
                return Game3DCore2.IsONAReady(pos);
            }

            public void StopReloadONA(string group)
            {
                LogHelper.CheckPoint($"Stop Reload  ONA {group}");
                Game3DCore2.StopReloadONA(group);
            }

            #endregion

#if USE_IMMERSIVE_ADMOB

            #region Immersive

            public void InitImmersive(string pos)
            {
                LogHelper.CheckPoint($"InitImmersive {pos}");
                AdsManager.InitImmersive(pos);
            }

            public void ShowImmersive(string pos, GameObject adParent)
            {
                LogHelper.CheckPoint($"ShowImmersive {pos}");
                AdsManager.ShowImmersive(pos, adParent);
            }

            public void DestroyImmersive(string pos)
            {
                LogHelper.CheckPoint($"DestroyImmersive {pos}");
                AdsManager.DestroyImmersive(pos);
            }

            public bool ImmersiveReady(string pos)
            {
                return AdsManager.ImmersiveIsReady(pos);
            }

            #endregion

#endif
        }
#endif
        internal sealed class IosAdsPlatform : IAdsPlatform
        {
            public string sceneName { get; set; }

            #region INTER

            public void InitInter(string group)
            {
                LogHelper.CheckPoint($"Init inter {group}");
                AdsManager.InitInterstitialManually(group);
            }

            public void ShowInter(string pos, Action complete)
            {
                LogHelper.CheckPoint($"show inter (iOS) {pos}");
                AdsManager.ShowInterstitial(pos);
                complete?.Invoke();
            }

            public bool IsInterPosReady(string pos) => AdsManager.IsInterstitialReady(pos);

            public void StopReloadFS(string group) => LogHelper.CheckPoint();

            #endregion

            #region BANNER

            public void InitBannerNA() => LogHelper.CheckPoint();
            public void InitBanner() => AdsManager.InitBannerManually();
            public void ShowBanner() => AdsManager.ShowBanner();

            public void ShowBannerCollapsible()
            {
                AdsManager.ShowBannerCollapsible();
            }

            public void HideBanner() => AdsManager.HideBanner();
            public void ShowCollapseBannerNA() => LogHelper.CheckPoint("ShowCollapseBannerNA (iOS) noop");
            public void HideCollapseBannerNA() => LogHelper.CheckPoint("HideCollapseBannerNA (iOS) noop");
            public void ShowBannerNA() => LogHelper.CheckPoint();
            public void HideBannerNA() => LogHelper.CheckPoint();
            public void DestroyBannerNA() => LogHelper.CheckPoint();
            public bool BannerNAReady() => false;
            public void StopReloadBNNA() => LogHelper.CheckPoint();
            public bool BannerReady() => AdsManager.Ins.AdsCoreIns.IsBNReady();
            public bool BNNAIsShowing() => false;

            #endregion

            #region REWARD

            public void InitReward() => AdsManager.InitRewardManually();
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

            #endregion

            #region MREC

            public void InitMREC()
            {
                LogHelper.CheckPoint();
                AdsManager.InitMrecManually();
            }

            public void ShowMREC(GameObject target)
            {
                LogHelper.CheckPoint();
                AdsManager.ShowMrec(target);
                AdsManager.UpdatePos(target);
            }

            public void ShowMREC(GameObject target, Camera cam)
            {
                LogHelper.CheckPoint();
                AdsManager.ShowMrec(target, cam);
                AdsManager.UpdatePos(target, cam);
            }

            public void ShowMREC(AdPosition adPosition)
            {
                LogHelper.CheckPoint();
                AdsManager.ShowMrec((int)adPosition);
                AdsManager.UpdatePos((int)adPosition);
            }

            public void HideMREC()
            {
                LogHelper.CheckPoint();
                AdsManager.HideMrec();
            }

            #endregion

            #region OVERLAY

            public void InitONA(string group) => LogHelper.CheckPoint();
            public void ShowONA(string pos) => LogHelper.CheckPoint();
            public void ShowONA(string pos, RectTransform objectPoss) => LogHelper.CheckPoint("Show ONA (iOS-ignoe)");

            public void ClearONA(string pos) => LogHelper.CheckPoint();
            public void CloseONA(string pos) => LogHelper.CheckPoint();

            public bool ONAReady(string pos)
            {
                LogHelper.CheckPoint("ONAReady (iOS) noop");
                return false;
            }

            public void StopReloadONA(string group) => LogHelper.CheckPoint();

            #endregion

#if USE_IMMERSIVE_ADMOB

            #region Immersive

            public void InitImmersive(string pos)
            {
                LogHelper.CheckPoint($"InitImmersive {pos}");
                AdsManager.InitImmersive(pos);
            }

            public void ShowImmersive(string pos, GameObject adParent)
            {
                LogHelper.CheckPoint($"ShowImmersive {pos}");
                AdsManager.ShowImmersive(pos, adParent);
            }

            public void DestroyImmersive(string pos)
            {
                LogHelper.CheckPoint($"DestroyImmersive {pos}");
                AdsManager.DestroyImmersive(pos);
            }

            public bool ImmersiveReady(string pos)
            {
                return AdsManager.ImmersiveIsReady(pos);
            }

            #endregion

#endif
        }
    }
}