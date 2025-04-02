using System;
using DG.Tweening;
using UnityEngine;

namespace _0.DucTALib.Splash
{
    public class FadeUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup cvg;
        [SerializeField] private float fadeDuration = 0.15f;
        public void Fade(Action onFadeComplete)
        {
            gameObject.SetActive(true);
            DOTween.Sequence()
                .Append(cvg.DOFade(1, fadeDuration))
                .AppendCallback(() =>
                {
                    onFadeComplete?.Invoke();
                })
                .Append( cvg.DOFade(0, fadeDuration))
                .AppendCallback(() =>
                {
                    gameObject.SetActive(false);
                })
                ;
        }

    }
}