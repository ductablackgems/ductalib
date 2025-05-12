using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using UnityEngine;

namespace _0.DucTALib.Splash.Scripts
{
    public abstract class BaseStepSplash : MonoBehaviour
    {
        public SplashType splashType;
        public abstract void Enter();
        public Transform mrecPos;
        public abstract void Next();
        public void Complete()
        {
            gameObject.HideObject();
            GameSplash.instance.NextStep();
        }

        protected void ShowMrec()
        {
            CallAdsManager.ResizeMREC(mrecPos.rectTransform());
            Debug.Log($"position : {mrecPos.transform.localPosition} _ {transform.name}");
            HideMrec();
            CallAdsManager.ShowMRECApplovin(mrecPos.gameObject, Camera.main);
        }

        protected void HideMrec()
        {
            CallAdsManager.HideMRECApplovin();
        }

        protected void DestroyMrec()
        {
            CallAdsManager.DestroyMRECApplovin();
        }
    }
}