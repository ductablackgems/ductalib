using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using BG_Library.Common;
using BG_Library.NET.Native_custom;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.Splash
{
    public class SetupNative : MonoBehaviour
    {
#if UNITY_EDITOR

        [FoldoutGroup("NA")] public Sprite btnSp;
        [FoldoutGroup("NA")] public List<NativeUIManager> na;
        [FoldoutGroup("NA")] public List<Texture> bgSp;
        [FoldoutGroup("NA")] public Texture iconSp;

        [Button]
        public void Setup()
        {
            foreach (var a in na)
            {
                Master.GetChildByName(a.gameObject, "CallToAction").GetComponent<Image>().sprite = btnSp;
                var bg = Master.GetChildByName(a.gameObject, "AdImage Size").transform.GetChild(0)
                    .GetComponent<RawImage>();
                var spBg = bgSp[Random.Range(0, bgSp.Count)];
                LogHelper.CheckPoint("0");
                LogHelper.CheckPoint($"{bg.texture == null}");
                LogHelper.CheckPoint($"{spBg == null}");
                bg.texture = spBg;

                var icon = Master.GetChildByName(a.gameObject, "AdIcon").GetComponent<RawImage>();
                icon.texture = iconSp;
#if USE_ADMOB_NATIVE
                 a.defaultIcon = iconSp;
                a.defaultImage = spBg;
#endif
            }
        }

        [FoldoutGroup("View")] public List<Image> content;
        [FoldoutGroup("View")] public List<Sprite> contentBg;


        [Button]
        public void SetupView()
        {
            foreach (var a in content)
            {
                var sp = contentBg[Random.Range(0, contentBg.Count)];
                a.sprite = sp;
            }
        }
#endif
    }
}