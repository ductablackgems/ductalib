using System;
using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.CustomButton;
using _0.DucTALib.Scripts.Common;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _0.DucTALib.Splash.Scripts
{
    [Serializable]
    public class DelayButtonTxt
    {
        public TextMeshProUGUI tmp;
        public ButtonCustom.ButtonPosition pos;
    }

    public class StepIntro : BaseStepSplash
    {
        public List<DelayButtonTxt> delayButtonTxts = new List<DelayButtonTxt>();
        [SerializeField] private List<string> tips = new List<string>();
        [SerializeField] private TextMeshProUGUI tipText;
        public ContentCarousel contentCarousel;
        private TextMeshProUGUI currentDelayButtonTxt;
        public CanvasGroup cvg;
        private Coroutine showButtonCoroutine;
        private int index = 0;
        private int countNext = 0;

        public override void Enter()
        {
            gameObject.ShowObject();
            GetCurrentButton();
            cvg.FadeInPopup();
            SplashTracking.OnboardingShow(1);
            index = 0;
            ShowMrec();
        }

        protected override void GetCurrentButton()
        {
            if (currentButton != null) return;
            var config = SplashRemoteConfig.CustomConfigValue.introConfig;
            var button = buttons.Find(x => x.type == config.buttonType && x.pos == config.buttonPos);
            currentDelayButtonTxt = delayButtonTxts.Find(x => x.pos == config.buttonPos).tmp;
            currentButton = button;
            currentButton.CustomButtonColor(config.buttonColor);
            currentButton.CustomTxt(config.textValue);
            currentButton.CustomTxtColor(config.textColor);
            StartDelayShowButton(SplashRemoteConfig.CustomConfigValue.introConfig.delayShowButtonTime);
        }

        public void NextOnClick()
        {
            AudioManager.Instance.PlayClickSound();
            NextStep();
            contentCarousel.MoveToNextPage();
        }

        private void NextStep()
        {
            index++;
            SplashTracking.OnboardingNext(index);
            if (showButtonCoroutine != null) StopCoroutine(showButtonCoroutine);
            showButtonCoroutine = null;

            if (index >= SplashRemoteConfig.CustomConfigValue.introConfig.tutorialCount)
            {
                Complete();
                return;
            }

            SplashTracking.OnboardingShow(index + 1);
            ShowMrec();
            StartDelayShowButton(SplashRemoteConfig.CustomConfigValue.introConfig.nextTime);
        }

        private void StartDelayShowButton(int time)
        {
            if (showButtonCoroutine != null) StopCoroutine(showButtonCoroutine);
            showButtonCoroutine = null;
            showButtonCoroutine = StartCoroutine(DelayShowButton(time));
        }

        private IEnumerator DelayShowButton(int time)
        {
            currentButton.HideObject();
            currentDelayButtonTxt.ShowObject();

            for (int i = time; i > 0; i--)
            {
                currentDelayButtonTxt.text = $"Next in {i}s...";
                yield return new WaitForSeconds(1f);
            }

            if (SplashRemoteConfig.CustomConfigValue.introConfig.isAutoNext)
            {
                currentDelayButtonTxt.HideObject();
                NextStep();
            }
            else
            {
                currentDelayButtonTxt.HideObject();
                currentButton.ShowObject();
            }
        }

        // private void SetImage()
        // {
        //     var count = SplashRemoteConfig.CustomConfigValue.introConfig.tutorialCount;
        //     var cindex = index;
        //     if (sprites.Count <= count) cindex = Random.Range(0, sprites.Count);
        //     tipText.text = tips[cindex];
        //     fadeImg.DOFade(1.0f, 0.12f).OnComplete(() =>
        //     {
        //         screenshotImage.sprite = sprites[cindex];
        //         fadeImg.DOFade(0, 0.12f);
        //     });
        // }

        public override void Next()
        {
        }
    }
}