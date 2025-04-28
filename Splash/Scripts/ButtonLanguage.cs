using _0.DucTALib.Scripts.Common;
using UnityEngine;

namespace _0.DucTALib.Splash.Scripts
{
    public class ButtonLanguage : MonoBehaviour
    {
        public LocalizedManager.Language language;
        public GameObject selected;
        public StepSelectLanguage controller;
        public void SelectLanguage()
        {
            controller.SelectLanguage(language);
        }
    }
}