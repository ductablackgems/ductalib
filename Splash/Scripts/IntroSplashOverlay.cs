using System;
using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Scripts.Loading;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.Splash.Scripts
{
    
    public class IntroSplashOverlay : MonoBehaviour
    {
        [BoxGroup("Intro")] public ContentCarousel contentImage;
        [BoxGroup("Intro")] public TextMeshProUGUI tipText;
        [BoxGroup("Intro")] public TextMeshProUGUI cdTimeText;
        [BoxGroup("Intro")] public List<Image> introImg;
        [BoxGroup("Intro")] public List<Sprite> introSprites;
        [BoxGroup("Intro")] public RectTransform adPos;
        [BoxGroup("Intro")] public GameObject btnNext;

        [BoxGroup("EndIntro")] public GameObject endIntroAdObject;
        [BoxGroup("EndIntro")] public Image endIntroImage;
        [BoxGroup("EndIntro")] public List<Sprite> endIntroBackgroundSprite;
        [BoxGroup("EndIntro")] public Text endIntroTextTime;
        [BoxGroup("EndIntro")] public Image endIntroAdCdFill;
        [BoxGroup("EndIntro")] public Button endIntroBtn;

        private int currentStep;
        private int endIntroIndex;
        private Coroutine nextStepCoroutine;
        private Coroutine endIntroStepCoroutine;
        private List<string> adPositions;
        private List<string> endIntroAdPositions;
        private void SetUp()
        {
            this.adPositions = SimpleSplashOverlay.instance.splashConfig.adPositions;
            this.endIntroAdPositions = SimpleSplashOverlay.instance.splashConfig.endIntroAdPositions;
            CallAdsManager.LoadInterByGroup("intro_step");
        }

        public void StartIntro()
        {
            SetUp();
            contentImage.ShowObject();
            StartStep();
        }

        private void StartStep()
        {
            if (adPositions.Count <= 0 || currentStep >= adPositions.Count)
            {
                // finish intro
                FinishAll();
                return;
            }


            CallAdsManager.ShowONA($"{adPositions[currentStep]}", adPos);
            if (currentStep + 1 == adPositions.Count)
            {
                CallAdsManager.InitONA($"{endIntroAdPositions[0]}");
            }
            else
            {
                CallAdsManager.InitONA($"{adPositions[currentStep + 1]}");
            }

            introImg[currentStep].sprite = introSprites[currentStep];
            tipText.text = GetTip();
            StartCDNextStep();
        }

        private void StartCDNextStep()
        {
            btnNext.HideObject();

            if (nextStepCoroutine != null)
            {
                StopCoroutine(nextStepCoroutine);
                nextStepCoroutine = null;
            }

            nextStepCoroutine = StartCoroutine(NextStepCD());
        }

        private IEnumerator NextStepCD()
        {
            float nextTime = SimpleSplashOverlay.instance.splashConfig.introStepTime;
            cdTimeText.text = $"Next in {(int)nextTime}s...";
            cdTimeText.ShowObject();
            while (nextTime > 0)
            {
                cdTimeText.text = $"Next in {(int)nextTime}s...";
                yield return null;
                nextTime -= Time.deltaTime;
            }

            cdTimeText.HideObject();
            btnNext.ShowObject();
        }

        public void NextClick()
        {
            if (CallAdsManager.ONAReady($"{adPositions[currentStep]}"))
            {
                CallAdsManager.CloseONA($"{adPositions[currentStep]}");
            }

            contentImage.MoveToNextPage();
            CallAdsManager.ShowInter($"intro_step_{currentStep}");
            currentStep += 1;


            if (currentStep >= SimpleSplashOverlay.instance.splashConfig.stepCount)
            {
                LogHelper.LogPurple("Kết thúc intro, sang màn end ads");

                StartEndIntroStep();
            }
            else
            {
                StartStep();
            }
        }

        private string GetTip()
        {
            if (SimpleSplashOverlay.instance.splashConfig == null ||
                SimpleSplashOverlay.instance.splashConfig.tipText == null ||
                SimpleSplashOverlay.instance.splashConfig.tipText.Count == 0)
            {
                return "";
            }

            return SimpleSplashOverlay.instance.splashConfig.tipText[currentStep];
        }


        private void StartEndIntroStep()
        {
            if (endIntroAdPositions.Count <= 0 || endIntroIndex >= endIntroAdPositions.Count)
            {
                return;
            }

            endIntroBtn.HideObject();
            endIntroAdCdFill.fillAmount = 1;
            endIntroAdCdFill.ShowObject();

            endIntroImage.sprite = endIntroBackgroundSprite[endIntroIndex];
            endIntroAdObject.ShowObject();

            CallAdsManager.ShowONA($"{endIntroAdPositions[endIntroIndex]}", adPos);
            if (endIntroIndex + 1 != endIntroAdPositions.Count)
            {
                CallAdsManager.InitONA($"{endIntroAdPositions[endIntroIndex + 1]}");
            }

            StartCoroutine(EndIntroCD());
        }


        private IEnumerator EndIntroCD()
        {
            float maxTime = SimpleSplashOverlay.instance.splashConfig.introStepTime;
            float currentTime0 = 0;
            float currentTime1 = SimpleSplashOverlay.instance.splashConfig.introStepTime;

            endIntroTextTime.text = $"{(int)currentTime1}";
            endIntroTextTime.ShowObject();
            while (currentTime1 > 0)
            {
                currentTime1 -= Time.deltaTime;
                currentTime0 += Time.deltaTime;
                yield return null;
                endIntroTextTime.text = $"{(int)currentTime1}";
                endIntroAdCdFill.fillAmount = currentTime0 / maxTime;
            }

            endIntroBtn.ShowObject();
            endIntroTextTime.HideObject();
            endIntroAdCdFill.HideObject();
        }

        public void CloseEndIntroAd()
        {
            if (CallAdsManager.ONAReady($"{endIntroAdPositions[endIntroIndex]}"))
            {
                CallAdsManager.CloseONA($"{endIntroAdPositions[endIntroIndex]}");
            }

            endIntroIndex += 1;

            if (endIntroIndex >= endIntroAdPositions.Count)
            {
                LogHelper.LogPurple("Kết thúc toàn bộ, load ad và chuyển scene");

                FinishAll();
            }
            else
            {
                StartEndIntroStep();
            }
        }

        private void FinishAll()
        {
            CallAdsManager.LoadBanner();
            CallAdsManager.LoadInterByGroup("gameplay");
            CallAdsManager.LoadInterByGroup("break");
            CallAdsManager.LoadReward();

            LoadingScene.instance.LoadMenu();
        }
    }
}