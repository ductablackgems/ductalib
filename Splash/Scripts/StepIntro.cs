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
        [SerializeField] private List<Sprite> sprites = new List<Sprite>();
        [SerializeField] private List<string> tips = new List<string>();
        [SerializeField] private Image screenshotImage;
        [SerializeField] private Image fadeImg;
        [SerializeField] private TextMeshProUGUI tipText;
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
            SplashTracking.TrackingIntro("show_intro");
            index = 0;
            SetImage();
            ShowMrec();
        }

        protected override void GetCurrentButton()
        {
            if (currentButton != null) return;
            var config = SplashRemoteConfig.CustomConfigValue.introConfig;
            var button = buttons.Find(x => x.type == config.buttonType && x.pos == config.buttonPos);
            currentDelayButtonTxt = delayButtonTxts.Find(x => x.pos == config.buttonPos).tmp;
            currentButton = button;
            StartDelayShowButton(SplashRemoteConfig.CustomConfigValue.introConfig.delayShowButtonTime);
        }

        public void NextOnClick()
        {
            AudioManager.Instance.PlayClickSound();
            NextStep();
        }

        private void NextStep()
        {
            index++;
            SplashTracking.TrackingIntro($"next_intro_{index}");
            if (showButtonCoroutine != null) StopCoroutine(showButtonCoroutine);
            showButtonCoroutine = null;
            if (index >= sprites.Count)
            {
                Complete();
                return;
            }

            ShowMrec();
            SetImage();
            StartDelayShowButton(SplashRemoteConfig.CustomConfigValue.introConfig.nextTIme);
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
                currentDelayButtonTxt.text = $"Next in {i}s";
                yield return new WaitForSeconds(1f);
            }

            if (SplashRemoteConfig.CustomConfigValue.introConfig.isAutoNext)
            {
                NextStep();
            }
            else
            {
                currentDelayButtonTxt.HideObject();
                currentButton.ShowObject();
            }
        }

        private void SetImage()
        {
            tipText.text = tips[index];
            fadeImg.DOFade(1.0f, 0.12f).OnComplete(() =>
            {
                screenshotImage.sprite = sprites[index];
                fadeImg.DOFade(0, 0.12f);
            });
        }

        public override void Next()
        {
        }
    }
}