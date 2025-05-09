using System.Collections;
using BG_Library.NET;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class LoadAdsManualy : SingletonMono<LoadAdsManualy>
    {
        protected override void Init()
        {
            base.Init();
            StartCoroutine(LoadAds());
        }

        private IEnumerator LoadAds()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => AdsManager.IsMrecReady);
            Debug.Log("mrec ready");
        }
    }
}