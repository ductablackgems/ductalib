using System.Collections.Generic;
using _0.DucLib.Scripts.Ads;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.CustomButton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _0.DucTALib.Splash.Scripts
{
    public abstract class BaseStepSplash : MonoBehaviour
    {
        public SplashType splashType;
        public abstract void Enter();
        public abstract void Next();

        protected abstract void GetCurrentButton();
        public List<ButtonCustom> buttons;
        [ReadOnly] public ButtonCustom currentButton;
        
        public MRECObject mrecObject;
        public virtual void Complete()
        {
            gameObject.HideObject();
            GameSplash.instance.NextStep();
        }

        protected void ShowMrec()
        {
            mrecObject.ShowMREC(Camera.main);
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