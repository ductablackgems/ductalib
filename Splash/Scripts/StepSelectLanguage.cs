using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.CustomButton;
using _0.DucTALib.Scripts.Common;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.Splash.Scripts
{
    public class StepSelectLanguage : BaseStepSplash
    {
        [SerializeField] private List<ButtonLanguage> languageButtons = new List<ButtonLanguage>();
        [SerializeField] private CanvasGroup canvasGroup;

        public void SelectLanguage(LocalizedManager.Language language)
        {
            D_AudioManager.Instance.PlayClickSound();
            LocalizedManager.instance.ChangeLocal(language);
            RefreshButton(language);
        }

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

        public override void Enter()
        {
            gameObject.ShowObject();
            GetCurrentButton();
            canvasGroup.FadeInPopup();
            SelectLanguage(GlobalData.Language);
            ShowMrec();
            ShowNext();
        }

        public override void Next()
        {
            GameSplash.instance.NextStep();
        }

        protected override void GetCurrentButton()
        {
            if (currentButton != null) return;
            var config = SplashRemoteConfig.CustomConfigValue.selectLanguageConfig;
            var button = buttons.Find(x => x.type == config.buttonType && x.pos == config.buttonPos);
            currentButton = button;
        }
    }
}