using System.Collections;
using BG_Library.NET;
using BG_Library.NET.Native_custom;
using UnityEngine;

namespace _0.DucTALib.Splash
{
    public class NativeAdsIntro : SingletonMono<NativeAdsIntro>
    {
        public NativeUIManager naUI;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => AdmobMediation.IsInitComplete);
            naUI.Request("intro");
            Debug.Log("NA =========== request");
            yield return new WaitUntil(()=> naUI.IsReady);
            Debug.Log("NA =========== ready");
            Show();
        }

        public void Show()
        {
            var isShowSuccess = naUI.Show();
            Debug.Log("NA =========== show");
        }

        public void Refresh()
        {
            naUI.RefreshAd();
        }

        public void FinishNA()
        {
            naUI.FinishNative();
        }
    }
}