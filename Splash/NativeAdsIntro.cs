using System.Collections;
using BG_Library.NET;

using UnityEngine;
#if USE_ADMOB_NATIVE
using BG_Library.NET.Native_custom;
#endif
namespace _0.DucTALib.Splash
{
    public class NativeAdsIntro : SingletonMono<NativeAdsIntro>
    {
        // public NativeUIManager naUI;
        //
        // private IEnumerator Start()
        // {
        //     yield return new WaitUntil(() => AdmobMediation.IsInitComplete);
        //     naUI.Request("intro");
        //     yield return new WaitUntil(()=> naUI.IsReady);
        //     Show();
        // }
        //
        // public void Show()
        // {
        //     var isShowSuccess = naUI.Show();
        //     Debug.Log("NA =========== show");
        // }
        //
        // public void Refresh()
        // {
        //     naUI.RefreshAd();
        // }
        //
        // public void FinishNA()
        // {
        //     naUI.FinishNative();
        // }
    }
}