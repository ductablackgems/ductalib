using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;

namespace _0.DucTALib.Splash.Scripts
{
    public class StepSelectAge : BaseStepSplash
    {
        [Header("UI Elements")]
        [SerializeField] private Toggle policyToggle;
        [SerializeField] private TextMeshProUGUI ageText;
        [SerializeField] private TextMeshProUGUI leftAgeText;
        [SerializeField] private TextMeshProUGUI rightAgeText;
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private CanvasGroup cvg;

        [Header("Ads")]
        // public NativeUIManager native;

        [Header("Config")]
        [SerializeField] private float durationShowButton;

        // Internal
        private int currentAge = 2012;
        private float cd;
        private bool isClick = false;
        private bool autoClose = true;

        //============================//
        //         Lifecycle          //
        //============================//

        public override void Enter()
        {
            gameObject.ShowObject();
            cvg.FadeInPopup();

            SplashTracking.PolicyShow();
            GetCurrentButton();

            StartCoroutine(LoadNative());
            StartCoroutine(AutoCloseCountdown());

            // native.Show();
            ShowMrec();
        }

        public override void Complete()
        {
            // native.FinishNative();
            base.Complete();
        }

        public override void Next()
        {
            cd = 0;
            autoClose = false;
        }

        //============================//
        //          Logic             //
        //============================//

        private IEnumerator LoadNative()
        {
            // yield return new WaitUntil(() => AdmobMediation.IsInitComplete);
            //
            // native.Request("select_age");
            // yield return new WaitUntil(() => native.IsReady);
            //
            // native.Show();
            yield break;
        }

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

        //============================//
        //         UI Events          //
        //============================//

        public void ToggleOnChange(bool isOn)
        {
            D_AudioManager.Instance.PlayClickSound();
            cd = SplashRemoteConfig.CustomConfigValue.selectAgeConfig.nextTime;

            if (policyToggle.isOn)
            {
                if (!currentButton.gameObject.activeSelf)
                    currentButton.ShowButtonTween();
            }
            else
            {
                currentButton.HideObject();
            }

            RefreshAds();
        }

        public void ChangeAge(int ageDelta)
        {
            D_AudioManager.Instance.PlayClickSound();

            currentAge += ageDelta;
            ageText.text = currentAge.ToString();
            leftAgeText.text = (currentAge - 1).ToString();
            rightAgeText.text = (currentAge + 1).ToString();

            if (policyToggle.isOn && !currentButton.gameObject.activeSelf)
                currentButton.ShowButtonTween();
            else if (!policyToggle.isOn)
                currentButton.HideObject();

            cd = SplashRemoteConfig.CustomConfigValue.selectAgeConfig.nextTime;
            RefreshAds();
        }

        //============================//
        //        Setup Button        //
        //============================//

        protected override void GetCurrentButton()
        {
            var config = SplashRemoteConfig.CustomConfigValue.selectAgeConfig;

            currentButton = buttons.Find(x => x.type == config.buttonType && x.pos == config.buttonPos);
            currentButton.CustomButtonColor(config.buttonColor);
            currentButton.CustomTxt(config.textValue);
            currentButton.CustomTxtColor(config.textColor);
        }
    }
}
