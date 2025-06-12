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
        public string nativePosition;
        [ReadOnly] public ButtonCustom currentButton;
        public List<NativeUIManager> nativeUIManagers;
        public MRECObject mrecObject;

        #endregion

        #region Private Fields

        [ReadOnly] private NativeUIManager currentNativeUIManager;
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
        public abstract void RefreshAds();

        #endregion

        #region Protected Methods

        protected abstract void GetCurrentButton();

        protected virtual void LoadNative()
        {
            var pos = $"{nativePosition}_{indexNative}";
            nativeUIManagers[indexNative].Request(pos);
        }

        protected void ShowCurrentNative()
        {
            StartCoroutine(ShowCurrentNativeIE());
        }

        private IEnumerator ShowCurrentNativeIE()
        {
            if(currentNativeUIManager != null) FinishCurrentNative();
            currentNativeUIManager = nativeUIManagers[indexNative];
            yield return new WaitUntil(() => currentNativeUIManager.IsReady);
            var pos = $"{nativePosition}_{indexNative}";
            LogHelper.LogPurple($"Show Native : {pos}");
            currentNativeUIManager.Show();
            LoadNextNative();
        }

        private void LoadNextNative()
        {
            indexNative++;
            if (indexNative < nativeUIManagers.Count)
            {
                LoadNative();
            }
        }

        protected void FinishCurrentNative()
        {
            if(currentNativeUIManager != null)
                currentNativeUIManager.FinishNative();
        }

        protected void ShowMrec()
        {
            mrecObject.ShowObject();
            mrecObject.ShowMREC(Camera.main);
        }

        #endregion
    }
}
