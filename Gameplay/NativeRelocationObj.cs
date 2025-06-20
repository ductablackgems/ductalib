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
        }

        private void OnDestroy()
        {
            nativeUI.FinishNative();
            if (loadNativeIE != null) StopCoroutine(loadNativeIE);
        }
    }
}