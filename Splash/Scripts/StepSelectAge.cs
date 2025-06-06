using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using BG_Library.NET.Native_custom;

namespace _0.DucTALib.Splash.Scripts
{
    public class StepSelectAge : BaseStepSplash
    {
        public enum NextType
        {
            AnyClick,
            ClickPolicy
        }

        #region Serialized Fields

        [Header("UI Elements")]
        [SerializeField] private Toggle policyToggle;
        [SerializeField] private TextMeshProUGUI ageText;
        [SerializeField] private TextMeshProUGUI leftAgeText;
        [SerializeField] private TextMeshProUGUI rightAgeText;
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private CanvasGroup cvg;

        [Header("Config")]
        [SerializeField] private float durationShowButton;

        #endregion

        #region Private Fields

        private int currentAge = 2012;
        private float cd;
        private bool isClick = false;
        private bool autoClose = true;
        private bool hasAnyChange;

        #endregion

        #region Unity Lifecycle

        protected override void Awake()
        {
            if (SplashRemoteConfig.CustomConfigValue.selectAgeConfig.adsType == AdFormatType.Native)
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
            cvg.FadeInPopup();
            SplashTracking.PolicyShow();
            GetCurrentButton();
            StartCoroutine(AutoCloseCountdown());
            ShowAds();
        }

        public override void Next()
        {
            cd = 0;
            autoClose = false;
        }

        public override void ShowAds()
        {
            if (SplashRemoteConfig.CustomConfigValue.selectAgeConfig.adsType == AdFormatType.Native)
            {
                StartCoroutine(ShowNative());
            }
            else if (SplashRemoteConfig.CustomConfigValue.selectAgeConfig.adsType == AdFormatType.MREC)
            {
                ShowMrec();
            }
        }

        protected override void GetCurrentButton()
        {
            var config = SplashRemoteConfig.CustomConfigValue.selectAgeConfig;

            currentButton = buttons.Find(x => x.type == config.buttonType && x.pos == config.buttonPos);
            currentButton.CustomButtonColor(config.buttonColor);
            currentButton.CustomTxt(config.textValue);
            currentButton.CustomTxtColor(config.textColor);
        }

        #endregion

        #region UI Event Handlers

        public void ToggleOnChange(bool isOn)
        {
            AudioController.Instance.PlayClickSound();
            cd = SplashRemoteConfig.CustomConfigValue.selectAgeConfig.nextTime;
            hasAnyChange = true;

            if (SplashRemoteConfig.CustomConfigValue.selectAgeConfig.nextType == NextType.ClickPolicy)
            {
                if (policyToggle.isOn)
                {
                    if (!currentButton.gameObject.activeSelf)
                        currentButton.ShowButtonTween();
                }
                else
                {
                    currentButton.HideObject();
                }
            }
            else if (hasAnyChange)
            {
                if (!currentButton.gameObject.activeSelf)
                    currentButton.ShowButtonTween();
            }

            RefreshAds();
        }

        public void ChangeAge(int ageDelta)
        {
            AudioController.Instance.PlayClickSound();
            hasAnyChange = true;

            currentAge += ageDelta;
            ageText.text = currentAge.ToString();
            leftAgeText.text = (currentAge - 1).ToString();
            rightAgeText.text = (currentAge + 1).ToString();

            if (SplashRemoteConfig.CustomConfigValue.selectAgeConfig.nextType == NextType.ClickPolicy)
            {
                if (policyToggle.isOn && !currentButton.gameObject.activeSelf)
                    currentButton.ShowButtonTween();
                else if (!policyToggle.isOn)
                    currentButton.HideObject();
            }
            else if (hasAnyChange)
            {
                if (!currentButton.gameObject.activeSelf)
                    currentButton.ShowButtonTween();
            }

            cd = SplashRemoteConfig.CustomConfigValue.selectAgeConfig.nextTime;
            RefreshAds();
        }

        #endregion

        #region Internal Logic

        private IEnumerator AutoCloseCountdown()
        {
            cd = SplashRemoteConfig.CustomConfigValue.selectAgeConfig.nextTime;

            while (cd > 0)
            {
                loadingText.text = $"AUTO CLOSE LATER {(int)cd}S";
                cd -= Time.deltaTime;
                yield return null;
            }

            loadingText.text = "";
            SplashTracking.PolicyEnd(autoClose);
            Complete();
        }

        private void RefreshAds()
        {
            if (!isClick)
            {
                isClick = true;
                // native.RefreshAd();
            }
        }

        #endregion
    }
}
