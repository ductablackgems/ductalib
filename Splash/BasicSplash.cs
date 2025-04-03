using System;
using System.Collections;
using _0.Custom.Scripts;
using _0.DucLib.Scripts.Ads;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _0.DucTALib.Splash
{
    public class BasicSplash : MonoBehaviour
    {
        [ReadOnly] public string[] loadingTxt = new string[]
        {
            "Checking network connection...",
            "Loading user profile data...",
            "Initializing game assets...",
            "Fetching remote configurations...",
            "Initializing Firebase services...",
            "Loading ads configuration...",
            "Preparing game environment...",
            "Syncing player data...",
            "Setting up audio manager...",
            "Loading game resources...",
            "Initializing leaderboard...",
            "Establishing server connection...",
            "Loading in-game tutorials...",
            "Setting up user preferences...",
            "Finalizing game setup..."
        };

        public Image loadingBar;
        public TextMeshProUGUI loadingText;
        public TextMeshProUGUI currentProgressTxt;
        private float loadingDuration = 12f;
        private float[] speedStages = { 2f, 1f, 0.5f, 1.5f, 2.5f };
        public float loadDuration = 12f;
        private int currentMessageIndex = 0;
        private float currentTime = 0f;

        private float currentProgress = 0f;
        private float targetProgress = 0f;

        private float stopDuration = 1f;
        private float stopTimer = 0f;
        private float smoothSpeed = 0.02f;
        private float cooldown = 0;

        private void Start()
        {
            StartCoroutine(WaitToLoadScene());
        }

        IEnumerator WaitToLoadScene()
        {
            loadingBar.fillAmount = 0;
            currentProgressTxt.text = $"{(int)(loadingBar.fillAmount * 100)}%";
            while (currentTime < loadDuration)
            {
                var canStop = Random.Range(0, 4);
                if (stopTimer <= 0f && canStop == 3)
                {
                    float speedModifier = Random.Range(0.001f, 0.01f);
                    currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, speedModifier);
                    if (Random.Range(0f, 1f) > 0.9f)
                    {
                        stopTimer = Random.Range(0.35f, 0.85f);
                    }
                }
                else
                {
                    stopTimer -= Time.deltaTime;
                }

                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, currentProgress, smoothSpeed);
                currentProgressTxt.text = $"{(int)(loadingBar.fillAmount * 100)}%";

                if (currentProgress >= (float)(currentMessageIndex + 1) / loadingTxt.Length)
                {
                    currentMessageIndex++;
                    loadingText.text = loadingTxt[currentMessageIndex];
                }

                currentTime += Time.deltaTime;
                targetProgress = Mathf.Clamp01(currentTime / loadDuration);
                yield return null;
            }

            loadingText.text = "Starting game";
            loadingBar.DOFillAmount(1, 0.2f);
            currentProgressTxt.text = $"{100}%";
            yield return new WaitForSeconds(0.2f);
            CompleteLoad();

        }

        protected virtual void CompleteLoad()
        {
            
        }

        IEnumerator ShowLoadingText()
        {
            float loadingDuration = 5f;
            float dotInterval = 0.5f;
            float elapsedTime = 0f;
            int dotCount = 0;
            string baseText = "Loading";

            while (elapsedTime < loadingDuration)
            {
                loadingText.text = baseText + new string('.', dotCount);

                dotCount = (dotCount + 1) % 4;

                yield return new WaitForSeconds(dotInterval);
                elapsedTime += dotInterval;
            }
        }
    }
}