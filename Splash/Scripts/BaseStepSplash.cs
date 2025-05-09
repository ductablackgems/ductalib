using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using BG_Library.NET;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.Splash.Scripts
{
    public abstract class BaseStepSplash : MonoBehaviour
    {
        public SplashType splashType;
        public abstract void Enter();
        public GameObject mrecPos;
        public Image tempImage;

        public abstract void Next();
        public void Complete()
        {
            gameObject.HideObject();
            GameSplash.instance.NextStep();
        }

        protected void ShowMrec()
        {
            HideMrec();
            CallAdsManager.ShowMRECApplovin(mrecPos, Camera.main);
        }
        protected  void HideMrec()
        {
            CallAdsManager.HideMRECApplovin();
        }
    }
}