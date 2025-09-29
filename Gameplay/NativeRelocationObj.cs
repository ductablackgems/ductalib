using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Splash;

using UnityEngine;
#if USE_ADMOB_NATIVE
using BG_Library.NET.Native_custom;
#endif
namespace _0.DucTALib.Gameplay
{
    public class NativeRelocationObj : MonoBehaviour
    {
#if USE_ADMOB_NATIVE
        public int id;
        public NativeUIManager nativeUI;
        private Coroutine loadNativeIE;
        public bool active;

        public void Show(string pos)
        {
            if (loadNativeIE != null) StopCoroutine(loadNativeIE);
            loadNativeIE = StartCoroutine(LoadNative(pos));
        }

        private IEnumerator LoadNative(string pos)
        {
            

            nativeUI.Request(pos);
            yield return new WaitUntil(() => nativeUI.IsReady);
            nativeUI.Show();
            LogHelper.CheckPoint($"Relocation native {pos}");

            yield return null;
        }

        private void OnDestroy()
        {
#if IGNORE_ADS
            return;
#endif


            
            if (loadNativeIE != null) StopCoroutine(loadNativeIE);
        }
#endif
        
    }
}