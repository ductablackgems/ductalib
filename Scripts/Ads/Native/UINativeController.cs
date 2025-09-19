using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using _0.DucLib.Scripts.Common;
#if USE_ADMOB_NATIVE
using BG_Library.NET.Native_custom;
#endif
namespace _0.DucLib.Scripts.Ads.Native
{
    public class UINativeController : MonoBehaviour
    {
#if USE_ADMOB_NATIVE
 public NativeUIManager nativeManager;

        public GameObject nativePanel;
        public Button closeButton;
        public float autoCloseDelay = 3f;
        public Image loadingImage;

        private Queue<string> queue = new();
        private bool isFirst = true;
        private Coroutine routine;

        public Action onStartAll;
        public Action onEndAll;

        public void SetQueue(List<string> list, int startIndex)
        {
            queue.Clear();
            isFirst = true;

            for (int i = startIndex + 1; i < list.Count; i++)
                queue.Enqueue(list[i]);

            LogHelper.CheckPoint($"[UINativeController] SetQueue → {queue.Count} item(s) in queue");
        }

        public void ShowCurrent()
        {
            if (isFirst)
            {
                LogHelper.CheckPoint("[UINativeController] First native → invoke onStartAll");
                onStartAll?.Invoke();
                isFirst = false;
            }

            LogHelper.CheckPoint("[UINativeController] ShowCurrent");

            nativePanel.SetActive(true);
            closeButton.gameObject.SetActive(false);

            if (!nativeManager.IsReady)
            {
                CloseAll();
                return;
            }
            nativeManager.Show();
            
            if (queue.Count > 0)
            {
                LogHelper.CheckPoint("[UINativeController] Start auto-close timer");
                routine = StartCoroutine(AutoCloseAndNext());
            }
            else
            {
                LogHelper.CheckPoint("[UINativeController] Last native → show close button");
                routine = StartCoroutine(AutoCloseAndNext());
               
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(() =>
                {
                    LogHelper.CheckPoint("[UINativeController] Close button clicked");
                    CloseAll();
                });
            }
        }

        private IEnumerator AutoCloseAndNext()
        {
            float timer = 0f;
            loadingImage.ShowObject();
            loadingImage.fillAmount = 0;
            closeButton.HideObject();
            LogHelper.CheckPoint("[UINativeController] AutoCloseAndNext started");

            while (timer < autoCloseDelay)
            {
                timer += Time.unscaledDeltaTime;

                
                loadingImage.fillAmount = Mathf.Clamp01(timer / autoCloseDelay);

                yield return null;
            }

            LogHelper.CheckPoint("[UINativeController] Auto-close time reached → closing");
            closeButton.ShowObject();
            loadingImage.HideObject();
            // nativeManager.FinishNative();
            // nativePanel.SetActive(false);

            // if (queue.Count > 0)
            // {
            //     // var next = queue.Dequeue();
            //     // LogHelper.CheckPoint($"[UINativeController] Request next native → {next}");
            //     // nativeManager.Request(next);
            //     // ShowCurrent();
            // }
            // else
            // {
            //     LogHelper.CheckPoint("[UINativeController] Queue empty → end sequence");
            //     CloseAll();
            // }
        }

        private void CloseAll()
        {
            LogHelper.CheckPoint("[UINativeController] CloseAll called");

            if (routine != null)
            {
                StopCoroutine(routine);
                routine = null;
            }

            nativeManager.FinishNative();
            nativePanel.SetActive(false);
            onEndAll?.Invoke();
        }

        public void ForceClose()
        {
            LogHelper.CheckPoint("[UINativeController] ForceClose called");

            if (routine != null)
            {
                StopCoroutine(routine);
                routine = null;
            }

            nativeManager?.FinishNative();
            nativePanel?.SetActive(false);
        }
#endif
       
    }
}