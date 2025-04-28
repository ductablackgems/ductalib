using _0.Game.Scripts;
using UnityEngine;

namespace _0.DucTALib.Splash.Scripts
{
    public abstract class BaseStepSplash : MonoBehaviour
    {
        public SplashType splashType;
        public abstract void Enter();

        public abstract void Next();
        public void Complete()
        {
            gameObject.HideObject();
            GameSplash.instance.NextStep();
        }
    }
}