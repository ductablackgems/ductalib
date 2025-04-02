using System.Collections;
using System.Collections.Generic;
using _0.Custom.Scripts;
using _0.DucLib.Scripts.Ads;
using _0.DucTALib.Scripts.Common;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.Splash
{
    public class IntroPanel : MonoBehaviour
    {
        [SerializeField] private List<Sprite> sprites = new List<Sprite>();
        [SerializeField] private List<string> tips = new List<string>();
        [SerializeField] private Image screenshotImage;
        [SerializeField] private Image fadeImg;
        [SerializeField] private Button skipButton;
        [SerializeField] private TextMeshProUGUI textSkip;
        [SerializeField] private TextMeshProUGUI tipText;
        [SerializeField] private float showButtonDuration = 3.5f;
        public Transform bannerPos;
        private Coroutine skipCoroutine;
        private int index = 0;
        private int countNext = 0;

        private void OnEnable()
        {
            // CalculateSize();
            index = 0;
            SetImage();
            skipCoroutine = StartCoroutine(DelayShowButton());
        }

        private void OnDisable()
        {
        }

        private void CalculateSize()
        {
            // leftPanel.sizeDelta = new Vector2(Screen.width / 2, Screen.height);
            // leftPanel.anchoredPosition = new Vector2(leftPanel.anchoredPosition.x/2,0 );
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
                LoadSplash.instance.StopCRLoading();
                return;
            }

            CallAdsManager.DestroyMRECApplovin();
            CallAdsManager.ShowMRECApplovin(bannerPos.gameObject, RenderMode.ScreenSpaceCamera);

            SetImage();
            // Todo load ads
            skipCoroutine = StartCoroutine(DelayShowButton());
        }

        private IEnumerator DelayShowButton()
        {
            skipButton.interactable = false;
            showButtonDuration = 2f;
            while (showButtonDuration > 0)
            {
                showButtonDuration -= Time.deltaTime;
                textSkip.text = $"";
                yield return null;
            }

            textSkip.text = $"NEXT";
            skipButton.interactable = true;
            // float delayTime = 3f;
            // while (delayTime > 0)
            // {
            //     delayTime -= Time.deltaTime;
            //     yield return null;
            // }
            // NextOnClick();
        }

        private void SetImage()
        {
            // toggles[index].isOn = true;
            tipText.text = tips[index];
            fadeImg.DOFade(1.0f, 0.12f).OnComplete(() =>
            {
                screenshotImage.sprite = sprites[index];
                fadeImg.DOFade(0, 0.12f);
            });
        }
    }
}