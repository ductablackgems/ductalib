using System;
using System.Collections.Generic;
using BG_Library.Common;
using BG_Library.NET.Native_custom;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _0.DucTALib.Splash.Scripts
{
    public class SplashNativeSetup : MonoBehaviour
    {
        [FoldoutGroup("Static")][BoxGroup("Static/All Native")] public List<NativeUIManager> native;
        [FoldoutGroup("Static")][BoxGroup("Static/Native Complete Intro")] public List<Image> NC_BGImage;
        [FoldoutGroup("Static")][BoxGroup("Static/Native Complete Intro")] public List<Image> NC_Button;
        [FoldoutGroup("Static")][BoxGroup("Static/Tutorial")] public List<Image> T_Button;
        [FoldoutGroup("Static")][BoxGroup("Static/Tutorial")] public List<Image> content;

        [FoldoutGroup("Custom")][BoxGroup("Custom/All Native")] public Texture mainIcon;
        [FoldoutGroup("Custom")][BoxGroup("Custom/All Native")] public List<Texture> backgroundNativeDefault;
        [FoldoutGroup("Custom")][BoxGroup("Custom/Native Complete Intro")] public Sprite NC_BGSprite;
        [FoldoutGroup("Custom")][BoxGroup("Custom/Native Complete Intro")] public List<Sprite> NC_ButtonSprite;
        [FoldoutGroup("Custom")][BoxGroup("Custom/Tutorial")] public Sprite T_ButtonSprite;
        [FoldoutGroup("Custom")][BoxGroup("Custom/Tutorial")] public List<Sprite> contentBg;


        private void Awake()
        {
            SetUpDefault();
        }
        [Button]
        private void SetUpDefault()
        {
            foreach (var a in native)
            {
                var bg = Master.GetChildByName(a.gameObject, "AdImage Size").transform.GetChild(0)
                    .GetComponent<RawImage>();
                var spBg = backgroundNativeDefault[Random.Range(0, backgroundNativeDefault.Count)];
                bg.texture = spBg;
                var icon = Master.GetChildByName(a.gameObject, "AdIcon").GetComponent<RawImage>();
                icon.texture = mainIcon;

#if USE_ADMOB_NATIVE
                a.defaultIcon = mainIcon;
                a.defaultImage = spBg;
#endif
            }

            foreach (var a in NC_BGImage)
            {
                a.sprite = NC_BGSprite;
            }

            for (int i = 0; i < NC_Button.Count; i++)
            {
                var btn = NC_Button[i];
                btn.sprite = NC_ButtonSprite[i];
            }

            for (int i = 0; i < T_Button.Count; i++)
            {
                var btn = T_Button[i];
                btn.sprite = T_ButtonSprite;
            }
        }

        [Button]
        public void SetupTutorialView()
        {
            foreach (var a in content)
            {
                if (contentBg == null) return;
                var sp = contentBg[Random.Range(0, contentBg.Count)];
                a.sprite = sp;
            }
        }
    }
}