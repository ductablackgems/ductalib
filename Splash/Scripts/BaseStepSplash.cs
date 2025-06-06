    using System;
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
            public SplashType splashType;
            public abstract void Enter();
            public abstract void Next();
            public abstract void ShowAds();
            protected abstract void GetCurrentButton();
            public List<ButtonCustom> buttons;

            public NativeUIManager native;
            public string nativePosition;
            [ReadOnly] public ButtonCustom currentButton;

            public MRECObject mrecObject;

            protected virtual void Awake()
            {
                gameObject.HideObject();
            }
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
                GameSplash.instance.NextStep();
            }

            protected void ShowMrec()
            {
                if (!GameSplash.instance.hasShowNative)
                {
                    mrecObject.ShowMREC(Camera.main);
                    GameSplash.instance.hasShowNative = true;
                }
                else
                    mrecObject.UpdateMREC(Camera.main);
            }

        }
    }