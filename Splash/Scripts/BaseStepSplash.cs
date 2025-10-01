using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.CustomButton;

using Sirenix.OdinInspector;
using UnityEngine;
#if USE_ADMOB_NATIVE
using BG_Library.NET.Native_custom;
#endif
namespace _0.DucTALib.Splash.Scripts
{
    public abstract class BaseStepSplash : MonoBehaviour
    {
#if USE_ADMOB_NATIVE
    public CanvasGroup canvasGroup;
        public SplashType splashType;
        public ButtonCustom currentButton;
        public List<NativeObject> nativeObjects;



        [ReadOnly] public NativeObject currentNativeObject;
        protected int indexNative = 0;



        private void Awake()
        {
            StartCoroutine(InitNA());
        }

        protected abstract IEnumerator InitNA();



        public virtual void Enter()
        {
            canvasGroup.alpha = 1;
            gameObject.ShowObject();
            ShowAds();
        }

        public virtual void Complete()
        {
            gameObject.HideObject();
            CallAdsManager.HideMREC();
            GameSplash.instance.NextStep();
        }

        public abstract void Next();
        public abstract void ShowAds();




        protected bool isFlowInProgress = false;

        public void ShowAdNative()
        {
            if (isFlowInProgress || indexNative >= nativeObjects.Count)
                return;

            var ad = nativeObjects[indexNative];

            // Show ad

            ad.native.Show();
            if(ad.isNativeFull)  ShowNativeFull();
            LogHelper.CheckPoint($"show native {ad.adsPosition}");


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

              ad.native.FinishNative();   
            if(ad.isNativeFull)  HideNativeFull();
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



                LogHelper.CheckPoint($"load native {nextAd.adsPosition}");
            }
        }
#endif
    



    }
}