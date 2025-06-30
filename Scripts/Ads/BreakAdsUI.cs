using System;
using System.Collections;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using BG_Library.Common;
using BG_Library.NET;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucLib.Scripts.Ads
{
    public class BreakAdsUI : MonoBehaviour
    {
        public GameObject content;
        [SerializeField] private Image _fillCountDown;
        [SerializeField] private TMP_Text _timeTxt;
        
        private Coroutine _countDownCoroutine;

        private void Awake()
        {
            BreakAdsInterstitial.Ins.IsPause = false;
            
            content.ShowObject();
            BreakAdsInterstitial.OnBreakAdsFAComing += OnBreakAdsFAComing;
            BreakAdsInterstitial.OnStartCountdown += OnBreakAdsReset;
            BreakAdsInterstitial.OnPause += OnBreakAdsReset;
            BreakAdsInterstitial.OnResume += OnBreakAdsResume;
        }

        private void OnDestroy()
        {
            BreakAdsInterstitial.OnBreakAdsFAComing -= OnBreakAdsFAComing;
            BreakAdsInterstitial.OnStartCountdown -= OnBreakAdsReset;
            BreakAdsInterstitial.OnPause -= OnBreakAdsReset;
            BreakAdsInterstitial.OnResume -= OnBreakAdsResume;
            
            BreakAdsInterstitial.Ins.IsPause = true;
        }
        
        private void OnBreakAdsFAComing(int countdown)
        {
            if(!AdsManager.IAP_RemoveAds) DisplayTime(countdown);
        }
        
        private void OnBreakAdsResume(bool isResume, float time)
        {
            if(isResume && !AdsManager.IAP_RemoveAds) DisplayTime(time);
        }
        
        private void OnBreakAdsReset()
        {
            StopDisplay();
        }

        private void DisplayTime(float time)
        {
            if(_countDownCoroutine != null)
                StopCoroutine(_countDownCoroutine);
            _countDownCoroutine = StartCoroutine(IERunCountDown(time));
        }

        private void StopDisplay()
        {
            if(_countDownCoroutine != null)
                StopCoroutine(_countDownCoroutine);
            content.HideObject();
        }

        private IEnumerator IERunCountDown(float time)
        {
            content.ShowObject();
            float endTime = Time.realtimeSinceStartup + time;
            while (Time.realtimeSinceStartup < endTime)
            {
                float currentTime = endTime - Time.realtimeSinceStartup;
                _fillCountDown.fillAmount = currentTime / time;
                _timeTxt.text = $"{(int)currentTime}";
                yield return null;
            }
            gameObject.HideObject();
        }
    }
}