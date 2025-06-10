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
        #region Serialized Fields

        [Header("UI References")] public List<DelayButtonTxt> delayButtonTxts = new List<DelayButtonTxt>();
        public List<string> tips = new List<string>();
        public TextMeshProUGUI tipText;
        public CanvasGroup cvg;
        public NativeUIManager nativeFull;
        [Header("Ad & Content")] public ContentCarousel contentCarousel;
        public Image closeNativeFullImg;
        public Text closeNativeFullTimeTxt;
        public GameObject closeNativeFullButton;

        #endregion

        #region Private Fields

        private TextMeshProUGUI currentDelayButtonTxt;
        private Coroutine showButtonCoroutine;
        private int index = 0;
        private bool isClick = false;

        #endregion

        #region Unity Lifecycle

        protected override IEnumerator InitNA()
        {
            yield return new WaitUntil(() => AdmobMediation.IsInitComplete);
            if (SplashRemoteConfig.CustomConfigValue.introConfig.adsType == AdFormatType.Native)
            {
                LoadNative();
            }

            if (SplashRemoteConfig.CustomConfigValue.introConfig.isShowNativeFull)
            {
                nativeFull.Request("TutorialFull");
            }

            gameObject.SetActive(false);
        }

        public void HideNativeFull()
        {
            nativeFull.FinishNative();
            NextStep();
        }

        private void ShowNativeFull()
        {
            if (nativeFull.IsReady)
            {
                FinishCurrentNative();
                nativeFull.Show();
                StartCoroutine(DelayShowCloseNative());
                contentCarousel.HideObject();
                currentButton.HideObject();
                currentDelayButtonTxt.HideObject();
                mrecObject.HideObject();
            }
            else
            {
                NextStep();
            }
        }

        private IEnumerator DelayShowCloseNative()
        {
            float totalDelay = SplashRemoteConfig.CustomConfigValue.introConfig.delayShowClose;
            float elapsed = 0f;

            while (elapsed < totalDelay)
            {
                elapsed += Time.deltaTime;
                float remaining = Mathf.Max(0, totalDelay - elapsed);

                closeNativeFullTimeTxt.text = ((int)remaining).ToString();
                closeNativeFullImg.fillAmount = elapsed / totalDelay;

                yield return null;
            }

            closeNativeFullTimeTxt.HideObject();
            closeNativeFullImg.HideObject();
            closeNativeFullButton.ShowObject();
        }

        #endregion

        #region BaseStepSplash Overrides

        public override void Enter()
        {
            base.Enter();
            contentCarousel.ShowObject();
            SplashTracking.OnboardingShow(1);
            index = 0;
        }

        public override void Next()
        {
            // Intentionally left blank
        }

        public override void ShowAds()
        {
            if (SplashRemoteConfig.CustomConfigValue.introConfig.adsType == AdFormatType.Native)
            {
                ShowCurrentNative();
                mrecObject.HideObject();
            }
            else if (SplashRemoteConfig.CustomConfigValue.introConfig.adsType == AdFormatType.MREC)
            {
                ShowMrec();
            }
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

        #endregion

        #region Button Logic

        public void NextOnClick()
        {
            AudioController.Instance.PlayClickSound();
            NextStep();
            contentCarousel.MoveToNextPage();
        }

        private bool isShowNativeFull = false;

        private void NextStep()
        {
            if (!isShowNativeFull && index ==
                SplashRemoteConfig.CustomConfigValue.introConfig.tutorialCount - 2 && SplashRemoteConfig
                .CustomConfigValue.introConfig.isShowNativeFull)
            {
                isShowNativeFull = true;
                ShowNativeFull();
                SplashTracking.ShowNativeFull();
                return;
            }

            contentCarousel.ShowObject();
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
            RefreshAds();
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

        public override void Complete()
        {
            base.Complete();
            if (SplashRemoteConfig.CustomConfigValue.introConfig.adsType == AdFormatType.Native)
                FinishCurrentNative();
        }

        #endregion
    }
}