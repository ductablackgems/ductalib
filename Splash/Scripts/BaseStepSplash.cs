using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.CustomButton;
using BG_Library.NET.Native_custom;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _0.DucTALib.Splash.Scripts
{
    public abstract class BaseStepSplash : MonoBehaviour
    {
        #region Serialized Fields

        public CanvasGroup canvasGroup;
        public SplashType splashType;
        public List<ButtonCustom> buttons;
        [ReadOnly] public ButtonCustom currentButton;
        public List<NativeObject> nativeObjects;
        public MRECObject mrecObject;

        #endregion

        #region Private Fields

        [ReadOnly] public NativeObject currentNativeObject;
        protected int indexNative = 0;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            StartCoroutine(InitNA());
        }

        protected abstract IEnumerator InitNA();

        #endregion

        #region Public Methods

        public virtual void Enter()
        {
            canvasGroup.alpha = 1;
            gameObject.ShowObject();
            ShowAds();
            GetCurrentButton();
        }

        public virtual void Complete()
        {
            gameObject.HideObject();
            CallAdsManager.HideMRECApplovin();
            GameSplash.instance.NextStep();
        }

        public abstract void Next();
        public abstract void ShowAds();

        #endregion

        #region Protected Methods

        protected abstract void GetCurrentButton();

        protected bool isFlowInProgress = false;

        public void ShowAdNative()
        {
            if (isFlowInProgress || indexNative >= nativeObjects.Count)
                return;

            var ad = nativeObjects[indexNative];

            // Show ad
#if USE_ADMOB_NATIVE
            ad.native.Show();
            if(ad.isNativeFull)  ShowNativeFull();
            LogHelper.CheckPoint($"show native {ad.adsPosition}");
#endif


            isFlowInProgress = true;

            PreloadNextAd();
        }

        public abstract void HideAds();
        protected abstract void ShowNativeFull();
        protected abstract void HideNativeFull();

        public void OnAdClosed()
        {
            if (indexNative >= nativeObjects.Count)
                return;

            var ad = nativeObjects[indexNative];
#if USE_ADMOB_NATIVE
              ad.native.FinishNative();   
            if(ad.isNativeFull)  HideNativeFull();
#endif

            LogHelper.CheckPoint($"finish native {ad.adsPosition}");

            indexNative++;
            isFlowInProgress = false;
        }

        private void PreloadNextAd()
        {
            int next = indexNative + 1;
            if (next < nativeObjects.Count)
            {
                var nextAd = nativeObjects[next];
#if USE_ADMOB_NATIVE
                nextAd.native.Request(nextAd.adsPosition);
#endif

                LogHelper.CheckPoint($"load native {nextAd.adsPosition}");
            }
        }


        protected void ShowMrec()
        {
            mrecObject.ShowObject();
            mrecObject.ShowMREC(Camera.main);
        }

        #endregion
    }
}