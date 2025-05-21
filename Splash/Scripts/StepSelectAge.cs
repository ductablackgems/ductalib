using System.Collections;
using _0.Custom.Scripts;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.CustomButton;
using _0.DucTALib.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.Splash.Scripts
{
    public class StepSelectAge : BaseStepSplash
    {
        [SerializeField] private float durationShowButton;
        [SerializeField] private Toggle policyToggle;
        [SerializeField] private TextMeshProUGUI ageText;
        [SerializeField] private TextMeshProUGUI leftAgeText;
        [SerializeField] private TextMeshProUGUI rightAgeText;
        public TextMeshProUGUI loadingText;
        public CanvasGroup cvg;
        private int currentAge = 2012;
        private bool autoClose = true;
        private float cd;

        public override void Enter()
        {
            gameObject.ShowObject();
            cvg.FadeInPopup();
            SplashTracking.PolicyShow();
            GetCurrentButton();
            StartCoroutine(LoadIE());
            ShowMrec();
        }

        private IEnumerator LoadIE()
        {
            cd = 10;
            while (cd > 0)
            {
                cd -= Time.deltaTime;
                loadingText.text = $"AUTO CLOSE LATER {(int)cd}S";
                yield return null;
            }

            loadingText.text = "";
            HideMrec();
            SplashTracking.PolicyEnd(autoClose);
            Complete();
        }

        public void ToggleOnChange(bool isOn)
        {
            AudioManager.Instance.PlayClickSound();
            cd = 10;
            if (!policyToggle.isOn) currentButton.HideObject();
            else if (policyToggle.isOn && !currentButton.gameObject.activeSelf) currentButton.ShowButtonTween();
        }

        public void ChangeAge(int age)
        {
            AudioManager.Instance.PlayClickSound();
            currentAge += age;
            ageText.text = currentAge.ToString();
            leftAgeText.text = (currentAge - 1).ToString();
            rightAgeText.text = (currentAge + 1).ToString();
            if (policyToggle.isOn && !currentButton.gameObject.activeSelf) currentButton.ShowButtonTween();
            else if (!policyToggle.isOn) currentButton.HideObject();
            cd = 10;
        }

        public override void Next()
        {
            cd = 0;
            autoClose = false;
        }

        protected override void GetCurrentButton()
        {
            var config = SplashRemoteConfig.CustomConfigValue.selectAgeConfig;
            var button = buttons.Find(x => x.type == config.buttonType && x.pos == config.buttonPos);
            currentButton = button;
        }
    }
}