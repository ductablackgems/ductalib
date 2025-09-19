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

#if USE_ADMOB_NATIVE
using BG_Library.NET.Native_custom;
#endif
namespace _0.DucTALib.Splash.Scripts
{
    public class StepSelectAge : BaseStepSplash
    {
        public enum NextType
        {
            AnyClick,
            ClickPolicy
        }

#if USE_ADMOB_NATIVE
[Header("UI Elements")] [SerializeField]
        private Toggle policyToggle;

        [SerializeField] private TextMeshProUGUI ageText;
        [SerializeField] private TextMeshProUGUI leftAgeText;
        [SerializeField] private TextMeshProUGUI rightAgeText;
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private CanvasGroup cvg;

        [Header("Config")] [SerializeField] private float durationShowButton;



        private int currentAge = 2012;
        private float cd;
        private bool isClick = false;
        private bool autoClose = true;
        private bool hasAnyChange;
        private bool buttonShowed;



        protected override IEnumerator InitNA()
        {
            yield return new WaitUntil(() => CommonRemoteConfig.instance.fetchComplete);

            indexNative = 0;
            isFlowInProgress = false;
            List<string> positionList = CommonRemoteConfig.instance.splashConfig.selectAgeConfig.adsPosition;

            nativeObjects = nativeObjects
                .Where(n => positionList.Contains(n.adsPosition))
                .OrderBy(n => positionList.IndexOf(n.adsPosition))
                .ToList();
            if (nativeObjects.Count > 0)
            {
                nativeObjects[0].native.Request(nativeObjects[0].adsPosition);
                Debug.Log($"[Preload First] {nativeObjects[0].adsPosition}");

            }

            gameObject.SetActive(false);
        }



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
            ShowAdNative();
        }

        public override void HideAds()
        {
            if (isClick) return;
            OnAdClosed();
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



        public void ToggleOnChange(bool isOn)
        {
            AudioController.Instance.PlayClickSound();
            cd = CommonRemoteConfig.instance.splashConfig.selectAgeConfig.nextTime;
            hasAnyChange = true;
            // HideAds();
            // ShowAds();
            isClick = true;
            if (CommonRemoteConfig.instance.splashConfig.selectAgeConfig.nextType == NextType.ClickPolicy)
            {
                if (policyToggle.isOn)
                {
                    if (!buttonShowed)
                    {
                        buttonShowed = true;
                        DOVirtual.DelayedCall(
                            CommonRemoteConfig.instance.splashConfig.selectAgeConfig.delayShowButtonTime,
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
                    CommonRemoteConfig.instance.splashConfig.selectAgeConfig.delayShowButtonTime,
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

            if (CommonRemoteConfig.instance.splashConfig.selectAgeConfig.nextType == NextType.ClickPolicy)
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
                    CommonRemoteConfig.instance.splashConfig.selectAgeConfig.delayShowButtonTime,
                    () => { currentButton.ShowButtonTween(0.12f); });
            }

            cd = CommonRemoteConfig.instance.splashConfig.selectAgeConfig.nextTime;
        }



        private IEnumerator AutoCloseCountdown()
        {
            cd = CommonRemoteConfig.instance.splashConfig.selectAgeConfig.nextTime;
            LogHelper.CheckPoint($"next time {cd}");
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
#endif
        

    }
}