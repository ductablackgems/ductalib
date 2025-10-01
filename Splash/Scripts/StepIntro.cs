using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

using Sirenix.OdinInspector;
#if USE_ADMOB_NATIVE
using BG_Library.NET.Native_custom;
#endif
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
#if USE_ADMOB_NATIVE
public TextMeshProUGUI tipText;
        public CanvasGroup cvg;
        public NativeUIManager nativeFull;
        [Header("Ad & Content")] public ContentCarousel contentCarousel;
        public Image closeNativeFullImg;
        public Text closeNativeFullTimeTxt;
        public GameObject closeNativeFullButton;
        public List<string> listTips = new List<string>();



        public TextMeshProUGUI currentDelayButtonTxt;
        private Coroutine showButtonCoroutine;
        private int index = 0;
        private bool isClick = false;



        protected override IEnumerator InitNA()
        {
            yield return new WaitUntil(() => CommonRemoteConfig.instance.fetchComplete);
            indexNative = 0;
            isFlowInProgress = false;
            List<string> positionList = CommonRemoteConfig.instance.splashConfig.introConfig.adsPosition;

            nativeObjects = nativeObjects
                .Where(n => positionList.Contains(n.adsPosition))
                .OrderBy(n => positionList.IndexOf(n.adsPosition))
                .ToList();
#if USE_ADMOB_NATIVE
            if (CommonRemoteConfig.instance.splashConfig.loadIntro && nativeObjects.Count > 0 && CommonRemoteConfig.instance.splashConfig.splashConfigs.Contains(SplashType.Intro))
            {
                nativeObjects[0].native.Request(nativeObjects[0].adsPosition);
                LogHelper.CheckPoint($"[Preload First] {nativeObjects[0].adsPosition}");
            }
#endif

            gameObject.SetActive(false);
            
        }


        protected override void ShowNativeFull()
        {
#if USE_ADMOB_NATIVE
            if (nativeFull.IsReady)
            {
                
                StartCoroutine(DelayShowCloseNative());
                contentCarousel.HideObject();
                currentButton.HideObject();
                currentDelayButtonTxt.HideObject();
               
                CallAdsManager.HideMREC();
            }
#endif
        }

        protected override void HideNativeFull()
        {
        }

        private IEnumerator DelayShowCloseNative()
        {
            float totalDelay = CommonRemoteConfig.instance.splashConfig.introConfig.delayShowClose;
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



        public override void Enter()
        {
            base.Enter();
            
            contentCarousel.ShowObject();
            SplashTracking.OnboardingShow(1);
            index = 0;
            GetTip(index);
            var config = CommonRemoteConfig.instance.splashConfig.introConfig;
            StartDelayShowButton(config.delayShowButtonTime);
        }

        public override void Next()
        {
            // Intentionally left blank
        }

        public override void ShowAds()
        {
            ShowAdNative();
        }
        public void NextOnClick()
        {
            AudioController.Instance.PlayClickSound();
            NextStep();
            contentCarousel.MoveToNextPage();
        }

        private bool isShowNativeFull = false;

        public override void HideAds()
        {
            OnAdClosed();

        }

        public void NextStep()
        {
            HideAds();
            ShowAds();
            if (!isShowNativeFull && indexNative < nativeObjects.Count && nativeObjects[indexNative].isNativeFull)
            {
                isShowNativeFull = true;
                return;
            }
            CallAdsManager.ShowInter($"next_step_intro_{index}");
            
            contentCarousel.ShowObject();
            index++;
            SplashTracking.OnboardingNext(index);

            if (showButtonCoroutine != null)
            {
                StopCoroutine(showButtonCoroutine);
                showButtonCoroutine = null;
            }

            if (index >= CommonRemoteConfig.instance.splashConfig.introConfig.tutorialCount)
            {
                Complete();
                return;
            }

            SplashTracking.OnboardingShow(index + 1);
            tipText.text = GetTip(index);

            StartDelayShowButton(CommonRemoteConfig.instance.splashConfig.introConfig.nextTime);
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

            if (CommonRemoteConfig.instance.splashConfig.introConfig.isAutoNext)
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
        }

        private string GetTip(int index)
        {
            if (listTips == null || listTips.Count == 0)
                listTips = CommonRemoteConfig.instance.splashConfig.introConfig.tipText;

            return listTips[index];
        }
#endif
        

    }
}