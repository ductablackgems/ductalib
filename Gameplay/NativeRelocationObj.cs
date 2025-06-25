using System.Collections;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Splash;
using BG_Library.NET.Native_custom;
using UnityEngine;

namespace _0.DucTALib.Gameplay
{
    public class NativeRelocationObj : MonoBehaviour
    {
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
#if USE_ADMOB_NATIVE
            nativeUI.Request(pos);
            yield return new WaitUntil(() => nativeUI.IsReady);
            nativeUI.Show();
            LogHelper.CheckPoint($"Relocation native {pos}");
#endif
            yield return null;
        }

        private void OnDestroy()
        {
#if USE_ADMOB_NATIVE
            nativeUI.FinishNative();
#endif
            
            if (loadNativeIE != null) StopCoroutine(loadNativeIE);
        }
    }
}