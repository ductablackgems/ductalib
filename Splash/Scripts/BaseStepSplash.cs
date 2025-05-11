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
            mrecPos.rectTransform().sizeDelta = CallAdsManager.GetMRECSize();
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