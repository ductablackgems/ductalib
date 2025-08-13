// using System;
// using System.Collections.Generic;
// using _0.DucLib.Scripts.Common;
// using BG_Library.Common;
// using BG_Library.NET.Native_custom;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using UnityEngine.UI;
// using Random = UnityEngine.Random;
//
// namespace _0.DucTALib.Splash
// {
//     public class SetupNative : MonoBehaviour
//     {
//         [FoldoutGroup("NA")] public Sprite btnSp;
//         [FoldoutGroup("NA")] public List<NativeUIManager> na;
//         [FoldoutGroup("NA")] public List<Texture> bgSp;
//
//         [Button]
//         public void Setup()
//         {
//             foreach (var a in na)
//             {
//                 Master.GetChildByName(a.gameObject, "CallToAction").GetComponent<Image>().sprite = btnSp;
//             }
//         }
//
//         [FoldoutGroup("View")] public List<Image> content;
//         [FoldoutGroup("View")] public List<Sprite> contentBg;
//
//
//         
//     }
// }