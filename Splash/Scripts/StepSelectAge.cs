using System.Collections;
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
        public ButtonCustomGroup selectAgeNext;
        public CanvasGroup cvg;
        private int currentAge = 2012;

        private float cd;

        public override void Enter()
        {
            gameObject.ShowObject();
            cvg.FadeInPopup();
            SplashTracking.TrackingIntro("show_select_age");
            // CallAdsManager.ShowMRECApplovin(bannerPos.gameObject, Camera.main);
            StartCoroutine(LoadIE());
            ShowMrec();
        }

        private IEnumerator LoadIE()
        {
            cd = 10;
            while (cd > 0)
            {
                cd -= Time.deltaTime;
                GameSplash.instance.loadingText.text = $"AUTO CLOSE LATER {(int)cd}S";
                yield return null;
            }

            GameSplash.instance.loadingText.text = "";
            HideMrec();
            Complete();
        }

        public void ToggleOnChange(bool isOn)
        {
            AudioManager.Instance.PlayClickSound();
            cd = 10;
            if (!policyToggle.isOn) selectAgeNext.CurrentButton.HideObject();
            else if (policyToggle.isOn && !selectAgeNext.CurrentButton.gameObject.activeSelf) selectAgeNext.CurrentButton.ShowButtonTween();
        }

        public void ChangeAge(int age)
        {
            AudioManager.Instance.PlayClickSound();
            currentAge += age;
            ageText.text = currentAge.ToString();
            leftAgeText.text = (currentAge - 1).ToString();
            rightAgeText.text = (currentAge + 1).ToString();
            if (policyToggle.isOn && !selectAgeNext.CurrentButton.gameObject.activeSelf) selectAgeNext.CurrentButton.ShowButtonTween();
            else if (!policyToggle.isOn) selectAgeNext.CurrentButton.HideObject();
            cd = 10;
        }

        public override void Next()
        {
            cd = 0;
        }
    }
}