using System.Collections;
using _0.DucLib.Scripts.Common;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace _0.DucTALib.Scripts.Common
{
    public class LocalizedManager : SingletonMono<LocalizedManager>
    {
        public enum Language
        {
            NONE = -1,
            Iraq = 0,
            English = 1,
            VietNam = 5,
            India = 4,
            Germany = 3,
            Philip = 2,
        }

        private bool active = false;
        
        protected override void Init()
        {
            base.Init();
            SetDefaultLanguage();
            _dontDestroyOnLoad = true;
        }

        public void ChangeLocal(Language language)
        {
            if (active) return;
            StartCoroutine(ChangeLanguage(language));
        }

        private IEnumerator ChangeLanguage(Language language)
        {
            active = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)language];
            GlobalData.Language = language;
            active = false;
        }

        private static void SetDefaultLanguage()
        {
            if( GlobalData.Language != Language.NONE) return;
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    GlobalData.Language = Language.English;
                    break;
                case SystemLanguage.Vietnamese:
                    GlobalData.Language = Language.VietNam;
                    break;
                case SystemLanguage.Arabic:
                    GlobalData.Language = Language.Iraq;
                    break;

                case SystemLanguage.German:
                    GlobalData.Language = Language.Germany;
                    break;
                case SystemLanguage.Hindi:
                    GlobalData.Language = Language.India;
                    break;
                default:
                    GlobalData.Language = Language.English;
                    break;
            }
        }
    }
}