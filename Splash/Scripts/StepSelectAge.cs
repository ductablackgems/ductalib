using System.Collections;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.Splash.Scripts
{
    public class StepSelectAge : BaseStepSplash
    {
        [SerializeField] private Transform buttonNext;
        [SerializeField] private float durationShowButton;
        [SerializeField] private Toggle policyToggle;
        [SerializeField] private TextMeshProUGUI ageText;
        [SerializeField] private TextMeshProUGUI leftAgeText;
        [SerializeField] private TextMeshProUGUI rightAgeText;
        public CanvasGroup cvg;
        public Transform bannerPos;
        private int currentAge = 2012;

        private float cd;
        public override void Enter()
        {
            gameObject.ShowObject();
            cvg.FadeInPopup();
            SplashTracking.TrackingIntro("show_select_age");
            CallAdsManager.ShowMRECApplovin(bannerPos.gameObject, Camera.main);
            StartCoroutine(LoadIE());
        }

        private IEnumerator LoadIE()
        {
            cd = 10;
            float time = 0;
            while (cd > 0)
            {
                cd -= Time.deltaTime;
                time += Time.deltaTime;
                GameSplash.instance.loadingText.text = $"AUTO CLOSE LATER {(int)cd}S";
                if (!buttonNext.gameObject.activeSelf && time >=4) buttonNext.ShowButtonTween();
                yield return null;
            }

            GameSplash.instance.loadingText.text = "";
            CallAdsManager.DestroyMRECApplovin();
            Complete();
        }
        public void ToggleOnChange(bool isOn)
        {
            AudioManager.Instance.PlayClickSound();
            cd = 10;
        }

        public void ChangeAge(int age)
        {
            AudioManager.Instance.PlayClickSound();
            currentAge += age;
            ageText.text = currentAge.ToString();
            leftAgeText.text = (currentAge - 1).ToString();
            rightAgeText.text = (currentAge + 1).ToString();
            cd = 10;
        }

        public override void Next()
        {
            cd = 0;
        }
    }
}