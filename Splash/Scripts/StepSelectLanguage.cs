using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;

namespace _0.DucTALib.Splash.Scripts
{
    public class StepSelectLanguage : BaseStepSplash
    {
        #region Serialized Fields

        [SerializeField] private List<ButtonLanguage> languageButtons = new List<ButtonLanguage>();
        [SerializeField] private CanvasGroup canvasGroup;

        #endregion

        #region Unity Lifecycle

        protected override void Awake()
        {
            if (SplashRemoteConfig.CustomConfigValue.selectLanguageConfig.adsType == AdFormatType.Native)
            {
                LoadNative();
            }
            base.Awake();
        }

        #endregion

        #region BaseStepSplash Overrides

        public override void Enter()
        {
            gameObject.ShowObject();
            ShowAds();
            GetCurrentButton();
            canvasGroup.FadeInPopup();
            SelectLanguage(GlobalData.Language);
            ShowNext();
        }

        public override void Next()
        {
            GameSplash.instance.NextStep();
        }

        public override void ShowAds()
        {
            if (SplashRemoteConfig.CustomConfigValue.selectLanguageConfig.adsType == AdFormatType.Native)
            {
                StartCoroutine(ShowNative());
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

        #endregion
    }
}
