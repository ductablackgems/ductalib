using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;

using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.CustomButton;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Splash.Scripts;
using BG_Library.NET;
using BG_Library.NET.Native_custom;

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
        [Header("UI References")]
        public List<DelayButtonTxt> delayButtonTxts = new List<DelayButtonTxt>();
        public List<string> tips = new List<string>();
        public TextMeshProUGUI tipText;
        public CanvasGroup cvg;

        [Header("Ad & Content")]
        // public NativeUIManager native;
        public ContentCarousel contentCarousel;

        // Private state
        private TextMeshProUGUI currentDelayButtonTxt;
        private Coroutine showButtonCoroutine;
        private int index = 0;
        private bool isClick = false;

        // ======================= //
        //        LIFE CYCLE       //
        // ======================= //

        public override void Enter()
        {
            gameObject.ShowObject();
            StartCoroutine(LoadNative());

            GetCurrentButton();
            cvg.FadeInPopup();

            SplashTracking.OnboardingShow(1);
            index = 0;

            ShowMrec();
        }

        public override void Complete()
        {
            // native.FinishNative();
            base.Complete();
        }

        public override void Next()
        {
            // Intentionally left blank
        }

        // ======================= //
        //      TRACKING + ADS     //
        // ======================= //

        private IEnumerator LoadNative()
        {
            // yield return new WaitUntil(() => AdmobMediation.IsInitComplete);
            //
            // native.Request("intro");
            // yield return new WaitUntil(() => native.IsReady);
            //
            // native.Show();
            yield break;
        }

      
        // ======================= //
        //      BUTTON LOGIC       //
        // ======================= //

        protected override void GetCurrentButton()
        {
            if (currentButton != null) return;

            var config = SplashRemoteConfig.CustomConfigValue.introConfig;

            currentButton = buttons.Find(x => x.type == config.buttonType && x.pos == config.buttonPos);
            currentDelayButtonTxt = delayButtonTxts.Find(x => x.pos == config.buttonPos).tmp;

            currentButton.CustomButtonColor(config.buttonColor);
            currentButton.CustomTxt(config.textValue);
            currentButton.CustomTxtColor(config.textColor);

            StartDelayShowButton(config.delayShowButtonTime);
        }

        public void NextOnClick()
        {
            D_AudioManager.Instance.PlayClickSound();
            NextStep();
            contentCarousel.MoveToNextPage();
        }

        private void NextStep()
        {
            index++;
            SplashTracking.OnboardingNext(index);

            if (showButtonCoroutine != null)
            {
                StopCoroutine(showButtonCoroutine);
                showButtonCoroutine = null;
            }

            if (index >= SplashRemoteConfig.CustomConfigValue.introConfig.tutorialCount)
            {
                Complete();
                return;
            }
            tipText.text = tips[index];
            SplashTracking.OnboardingShow(index + 1);
            ShowMrec();

            StartDelayShowButton(SplashRemoteConfig.CustomConfigValue.introConfig.nextTime);
        }

        private void StartDelayShowButton(int time)
        {
            if (showButtonCoroutine != null)
            {
                StopCoroutine(showButtonCoroutine);
                showButtonCoroutine = null;
            }

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

            currentDelayButtonTxt.HideObject();

            if (SplashRemoteConfig.CustomConfigValue.introConfig.isAutoNext)
            {
                NextStep();
            }
            else
            {
                currentButton.ShowObject();
            }
        }
    }
}
