
using UnityEngine;
using UnityEngine.SceneManagement;

#if USE_ADMOB_NATIVE
using BG_Library.NET.Native_custom;
#endif
namespace _0.DucLib.Scripts.Ads
{
    public class NativeCameraUpdater : MonoBehaviour
    {
#if USE_ADMOB_NATIVE
 [SerializeField] private Canvas canvas;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            TryAssignCamera();
        }

        private void TryAssignCamera()
        {
            NativeCamera target = FindObjectOfType<NativeCamera>();
            if (target != null)
            {
                Camera targetCam = target.GetComponent<Camera>();
                if (targetCam != null)
                {
                    canvas.worldCamera = targetCam;
                    canvas.sortingLayerName = "UI";
                    Debug.Log($"Assigned native camera to canvas: {targetCam.name}");
                }
                else
                {
                    Debug.LogWarning("NativeCamera found, but no Camera component on the same GameObject.");
                }
            }
            else
            {
                Debug.LogWarning("No NativeCamera found in the scene.");
            }
        }
#endif
       
    }
}