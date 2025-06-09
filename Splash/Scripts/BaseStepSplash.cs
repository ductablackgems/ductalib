    using System;
    using System.Collections;
    using System.Collections.Generic;
    using _0.DucLib.Scripts.Ads;
    using _0.DucLib.Scripts.Common;
    using _0.DucTALib.CustomButton;
    using _0.DucTALib.Scripts.Common;
    using BG_Library.NET.Native_custom;
    using Sirenix.OdinInspector;
    using UnityEngine;

    namespace _0.DucTALib.Splash.Scripts
    {
        public abstract class BaseStepSplash : MonoBehaviour
        {
            public CanvasGroup canvasGroup;
            public SplashType splashType;
            public abstract void Next();
            public abstract void ShowAds();
            
            public abstract void RefreshAds();
            protected abstract void GetCurrentButton();
            public List<ButtonCustom> buttons;

            public NativeUIManager native;
            public string nativePosition;
            [ReadOnly] public ButtonCustom currentButton;

            public MRECObject mrecObject;
            public virtual void Enter()
            {
                canvasGroup.alpha = 1;
                gameObject.ShowObject();
                ShowAds();
                GetCurrentButton();
            }
            
            private void Awake()
            {
                StartCoroutine(InitNA());
            }

            protected abstract IEnumerator InitNA();
            protected void LoadNative()
            {
                native.Request(nativePosition);
            }

            protected IEnumerator ShowNative()
            {
                yield return new WaitUntil(() => native.IsReady);
                native.Show();
            }
            public virtual void Complete()
            {
                gameObject.HideObject();
                CallAdsManager.HideMRECApplovin();
                GameSplash.instance.NextStep();
            }

            protected void ShowMrec()
            {
                mrecObject.ShowMREC(Camera.main);
            }

        }
    }