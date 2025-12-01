using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using _0.DucTALib.Splash;
using BG_Library.IAR;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.AppReview
{
    public class IARPopup : MonoBehaviour
    {
        public Transform content;
        public CanvasGroup startReview;
        public CanvasGroup badReview;
        public CanvasGroup goodReview;
        public List<GameObject> star;
        public Button submitButton;
        private CanvasGroup currentCvg;
        private int currentStar;


        public void Show()
        {
            if (GlobalData.Reviewed)
            {
                return;
            }

            gameObject.ShowObject();
            content.ScaleInPopup();
            currentStar = 0;
            currentCvg = startReview;
            submitButton.interactable = false;
            ShowDefault();
        }

        public void ShowDefault()
        {
            startReview.alpha = 1;
            badReview.alpha = 0;
            goodReview.alpha = 0;
            StarClick(0);
        }

        public void StarClick(int starRate)
        {
            currentStar = starRate;
            if (currentStar != 0)
            {
                AudioController.Instance?.PlayClickSound();
                if (currentStar < 4 && currentCvg != badReview)
                {
                    currentCvg.DOFade(0, 0.2f);
                    currentCvg = badReview;
                    currentCvg.DOFade(1, 0.2f);
                }
                else if (currentStar >= 4 && currentCvg != goodReview)
                {
                    currentCvg.DOFade(0, 0.2f);
                    currentCvg = goodReview;
                    currentCvg.DOFade(1, 0.2f);
                }
            }

            for (int i = 0; i < star.Count; i++)
            {
                var s = star[i];
                s.SetActive(currentStar >= i + 1);
            }

            submitButton.interactable = currentStar != 0;
        }

        public void Submit()
        {
            SplashTracking.Rating(currentStar);
            if (CommonRemoteConfig.instance.commonConfig.isProduct)
            {
                if (currentStar >= 4)
                {
                    GlobalData.Reviewed = true;
                    InAppReviewManager.ShowReview();
                }
            }


            Hide();
            gameObject.HideObject();
        }

        public void Hide()
        {
            AudioController.Instance?.PlayClickSound();
            gameObject.HideObject();
        }
    }
}
