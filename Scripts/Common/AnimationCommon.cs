using DG.Tweening;
using UnityEngine;

namespace _0.DucTALib.Scripts.Common
{
    public static class AnimationCommon
    {
        public static void ScaleInPopup(this Transform obj)
        {
            obj.transform.localScale = Vector3.one * 0.5f;
            obj.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);
        }

        public static void ScaleOutPopup()
        {
            
        }

        public static void FadeInPopup(this CanvasGroup obj, float time = 0.3f)
        {
            obj.transform.localScale = Vector3.one * 1.05f;
            obj.alpha = 0;
            obj.DOFade(1, time).SetUpdate(true);
            obj.transform.DOScale(1, time).SetUpdate(true);
        }

        public static Tween FadeOutPopup(this CanvasGroup obj, float time = 0.3f)
        {
            obj.transform.localScale = Vector3.one;
            return DOTween.Sequence()
                    .Append(obj.DOFade(0, time))
                    .Join(obj.transform.DOScale(0.9f, time))
                ;
        }
    }
}