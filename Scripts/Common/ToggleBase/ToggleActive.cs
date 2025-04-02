using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using UnityEngine;

namespace _0.DucTALib.Scripts.Common.ToggleBase
{
    public class ToggleActive : MonoBehaviour
    {
        public List<GameObject> gameObjects;

        public void Show()
        {
            foreach (var a in gameObjects)
            {
                a.ShowObject();
            }
        }

        public void Hide()
        {
            foreach (var a in gameObjects)
            {
                a.HideObject();
            }
        }
    }
}