using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _0.DucTALib.Splash
{
    public class CheckInternetScene : MonoBehaviour
    {
        [SerializeField] private Button retryButton;
        [SerializeField] private GameObject popupRetry;
       
        private void Start()
        {
            retryButton.onClick.AddListener(OnRetryClick);
            OnStartCheck();
        }

        private void OnRetryClick()
        {
            SplashTracking.IsRetryTurnOnInternet = true;
            Check();
        }
        private void OnStartCheck()
        {
            Check();
        }
        private void Check()
        {
            popupRetry.SetActive(false);

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                NoInternet();
                return;
            }

            // Có mạng vật lý, check thử kết nối thật
            StartCoroutine(PingServer());
        }

        private IEnumerator PingServer()
        {
            using (var req = new UnityEngine.Networking.UnityWebRequest("https://clients3.google.com/generate_204"))
            {
                req.timeout = 3;
                yield return req.SendWebRequest();

                if (req.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
                    NoInternet();
                else
                {
                    SceneManager.LoadScene(1);
                }
            }
        }

        private void NoInternet()
        {
            popupRetry.SetActive(true);
        }
    }
}