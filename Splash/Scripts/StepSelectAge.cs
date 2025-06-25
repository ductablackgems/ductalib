using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using BG_Library.NET;
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

        [Header("UI Elements")] [SerializeField]
        private Toggle policyToggle;

        [SerializeField] private TextMeshProUGUI ageText;
        [SerializeField] private TextMeshProUGUI leftAgeText;
        [SerializeField] private TextMeshProUGUI rightAgeText;
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private CanvasGroup cvg;

        [Header("Config")] [SerializeField] private float durationShowButton;

        #endregion

        #region Private Fields

        private int currentAge = 2012;
        private float cd;
        private bool isClick = false;
        private bool autoClose = true;
        private bool hasAnyChange;
        private bool buttonShowed;

        #endregion

        #region Unity Lifecycle

        protected override IEnumerator InitNA()
        {
            yield return new WaitUntil(() => AdmobMediation.IsInitComplete);

            if (SplashRemoteConfig.CustomConfigValue.selectAgeConfig.adsType == AdFormatType.Native)
            {
                indexNative = 0;
                isFlowInProgress = false;
                List<string> positionList = SplashRemoteConfig.CustomConfigValue.selectAgeConfig.adsPosition;

                nativeObjects = nativeObjects
                    .Where(n => positionList.Contains(n.adsPosition))
                    .OrderBy(n => positionList.IndexOf(n.adsPosition))
                    .ToList();
                if (nativeObjects.Count > 0)
                {
#if USE_ADMOB_NATIVE
                     nativeObjects[0].native.Request(nativeObjects[0].adsPosition);
                    Debug.Log($"[Preload First] {nativeObjects[0].adsPosition}");
#endif
                }
            }

            gameObject.SetActive(false);
        }

        #endregion

        #region BaseStepSplash Overrides

        public override void Enter()
        {
            base.Enter();
            SplashTracking.PolicyShow();
            StartCoroutine(AutoCloseCountdown());
        }

        public override void Next()
        {
            cd = 0;
            autoClose = false;
        }

        public override void ShowAds()
        {
            if (isClick) return;
            if (SplashRemoteConfig.CustomConfigValue.selectAgeConfig.adsType == AdFormatType.Native)
            {
                ShowAdNative();
                mrecObject.HideObject();
            }
            else if (SplashRemoteConfig.CustomConfigValue.selectAgeConfig.adsType == AdFormatType.MREC)
            {
                ShowMrec();
            }
        }

        public override void HideAds()
        {
            if (isClick) return;
            if (SplashRemoteConfig.CustomConfigValue.selectAgeConfig.adsType == AdFormatType.Native)
            {
                OnAdClosed();
                mrecObject.HideObject();
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

        protected override void ShowNativeFull()
        {
        }

        protected override void HideNativeFull()
        {
        }

        public override void Complete()
        {
            base.Complete();
        }

        #endregion

        #region UI Event Handlers

        public void ToggleOnChange(bool isOn)
        {
            AudioController.Instance.PlayClickSound();
            cd = SplashRemoteConfig.CustomConfigValue.selectAgeConfig.nextTime;
            hasAnyChange = true;
            // HideAds();
            // ShowAds();
            isClick = true;
            if (SplashRemoteConfig.CustomConfigValue.selectAgeConfig.nextType == NextType.ClickPolicy)
            {
                if (policyToggle.isOn)
                {
                    if (!buttonShowed)
                    {
                        buttonShowed = true;
                        DOVirtual.DelayedCall(
                            SplashRemoteConfig.CustomConfigValue.selectAgeConfig.delayShowButtonTime,
                            () => { currentButton.ShowButtonTween(0.12f); });
                    }
                }
                else
                {
                    buttonShowed = false;
                    currentButton.HideObject();
                }
            }
            else if (!buttonShowed)
            {
                buttonShowed = true;
                DOVirtual.DelayedCall(
                    SplashRemoteConfig.CustomConfigValue.selectAgeConfig.delayShowButtonTime,
                    () => { currentButton.ShowButtonTween(0.12f); });
            }
        }

        public void ChangeAge(int ageDelta)
        {
            AudioController.Instance.PlayClickSound();
            hasAnyChange = true;
            // HideAds();
            // ShowAds();
            isClick = true;
            currentAge += ageDelta;
            ageText.text = currentAge.ToString();
            leftAgeText.text = (currentAge - 1).ToString();
            rightAgeText.text = (currentAge + 1).ToString();

            if (SplashRemoteConfig.CustomConfigValue.selectAgeConfig.nextType == NextType.ClickPolicy)
            {
                if (policyToggle.isOn && !currentButton.gameObject.activeSelf)
                    currentButton.ShowButtonTween(0.12f);
                else if (!policyToggle.isOn)
                    currentButton.HideObject();
            }
            else if (!buttonShowed)
            {
                buttonShowed = true;
                DOVirtual.DelayedCall(
                    SplashRemoteConfig.CustomConfigValue.selectAgeConfig.delayShowButtonTime,
                    () => { currentButton.ShowButtonTween(0.12f); });
            }

            cd = SplashRemoteConfig.CustomConfigValue.selectAgeConfig.nextTime;
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
            HideAds();
            Complete();
        }

        #endregion
    }
}