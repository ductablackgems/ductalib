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
    public class StepIntro : BaseStepSplash
    {
        [SerializeField] private List<Sprite> sprites = new List<Sprite>();
        [SerializeField] private List<string> tips = new List<string>();
        [SerializeField] private Image screenshotImage;
        [SerializeField] private Image fadeImg;
        [SerializeField] private TextMeshProUGUI tipText;
        [SerializeField] private float showButtonDuration = 3.5f;
        public ButtonCustomGroup nextGroup;
        public CanvasGroup cvg;
        private Coroutine skipCoroutine;
        private int index = 0;
        private int countNext = 0;

        public override void Enter()
        {
            gameObject.ShowObject();
            cvg.FadeInPopup();
            SplashTracking.TrackingIntro("show_intro");
            index = 0;
            SetImage();
            skipCoroutine = StartCoroutine(DelayShowButton());
            ShowMrec();
        }

        public override void Next()
        {
        }

        public void NextOnClick()
        {
            AudioManager.Instance.PlayClickSound();
            index++;
            SplashTracking.TrackingIntro($"next_intro_{index}");
            if (skipCoroutine != null)
            {
                StopCoroutine(skipCoroutine);
            }

            skipCoroutine = null;
            if (index >= sprites.Count)
            {
                Complete();
                return;
            }

            ShowMrec();

            SetImage();
            skipCoroutine = StartCoroutine(DelayShowButton());
        }

        private IEnumerator DelayShowButton()
        {
            nextGroup.CurrentButton.HideObject();
            showButtonDuration = 2f;
            while (showButtonDuration > 0)
            {
                showButtonDuration -= Time.deltaTime;
                yield return null;
            }

            nextGroup.ShowObject();
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
    }
}