using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Ads;
using UnityEngine;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using BG_Library.NET;

namespace _0.DucTALib.Splash.Scripts
{
    public class StepSelectLanguage : BaseStepSplash
    {
        #region Serialized Fields

        [SerializeField] private List<ButtonLanguage> languageButtons = new List<ButtonLanguage>();
        public CanvasGroup anim;

        #endregion

        #region Unity Lifecycle

        
        
        
        protected override IEnumerator InitNA()
        {
            yield return new WaitUntil(() => AdmobMediation.IsInitComplete);
            if (SplashRemoteConfig.CustomConfigValue.selectLanguageConfig.adsType == AdFormatType.Native)
            {
                LoadNative();
            }

            gameObject.SetActive(false);
        }

        public override void RefreshAds()
        {
            if (SplashRemoteConfig.CustomConfigValue.introConfig.adsType == AdFormatType.Native)
            {
                ShowCurrentNative();
            }
            else
            {
                ShowMrec();
            }
        }

        #endregion

        #region BaseStepSplash Overrides

        public override void Enter()
        {
            base.Enter();
            SelectLanguage(GlobalData.Language);
            ShowNext();
            anim.FadeInPopup();
        }

        public override void Next()
        {
            GameSplash.instance.NextStep();
        }

        public override void ShowAds()
        {
            if (SplashRemoteConfig.CustomConfigValue.selectLanguageConfig.adsType == AdFormatType.Native)
            {
                ShowCurrentNative();
                mrecObject.HideObject();
            }
            else if (SplashRemoteConfig.CustomConfigValue.selectLanguageConfig.adsType == AdFormatType.MREC)
            {
                ShowMrec();
            }
        }

        protected override void GetCurrentButton()
        {
            if (currentButton != null) return;

            var config = SplashRemoteConfig.CustomConfigValue.selectLanguageConfig;
            currentButton = buttons.Find(x => x.type == config.buttonType && x.pos == config.buttonPos);
        }

        #endregion

        #region Public Methods

        public void SelectLanguage(LocalizedManager.Language language)
        {
            AudioController.Instance.PlayClickSound();
            LocalizedManager.instance.ChangeLocal(language);
            RefreshButton(language);
        }

        #endregion

        #region Private Methods

        private void RefreshButton(LocalizedManager.Language language)
        {
            foreach (var a in languageButtons)
            {
                a.selected.SetActive(a.language == language);
            }
        }

        private void ShowNext()
        {
            currentButton.ShowObject();
        }
        public override void Complete()
        {
            base.Complete();
            if (SplashRemoteConfig.CustomConfigValue.selectLanguageConfig.adsType == AdFormatType.Native)
                FinishCurrentNative();
        }
        #endregion
    }
}