using System;
using System.Collections;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using BG_Library.Common;
using BG_Library.NET;
using TMPro;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class BreakAdsUI : MonoBehaviour
    {
        public CanvasGroup cvg;
        public TextMeshProUGUI tmpNoti;
        private Coroutine cr;
        private void Awake()
        {
            transform.SetParent(DDOL.Instance.transform);
            BreakAdsInterstitial.OnBreakAdsFAComing += Show;
        }

        private void OnDestroy()
        {
            BreakAdsInterstitial.OnBreakAdsFAComing -= Show;
        }

        private void Show(int time)
        {
            if (cr != null) StopCoroutine(cr);
            cr = StartCoroutine(ShowBreakAdsUI(time));
        }

        private IEnumerator ShowBreakAdsUI(int time)
        {
            cvg.ShowObject();
            cvg.FadeInPopup();
            while (time > 0)
            {
                time -= 1;
                yield return new WaitForSecondsRealtime(1);
                tmpNoti.text = $"Get ready! An ad is coming in {time} seconds.";
            }
            cvg.HideObject();
        }
    }
}