using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.Splash.Scripts
{
    public class StepSelectLanguage : BaseStepSplash
    {
        public override void Enter()
        {
            gameObject.ShowObject();
            SplashTracking.TrackingIntro("show_select_age");
            canvasGroup.FadeInPopup();
            SelectLanguage(GlobalData.Language);
            ShowMrec();
            StartCoroutine(ShowNext());
        }

        public override void Next()
        {
            GameSplash.instance.NextStep();
        }

        [SerializeField] private List<ButtonLanguage> languageButtons = new List<ButtonLanguage>();
        [SerializeField] private CanvasGroup canvasGroup;
        public GameObject buttonNext;

        public void SelectLanguage(LocalizedManager.Language language)
        {
            AudioManager.Instance.PlayClickSound();
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

        private IEnumerator ShowNext()
        {
            yield return new WaitForSeconds(3.5f);
            buttonNext.ShowObject();
        }
    }
}