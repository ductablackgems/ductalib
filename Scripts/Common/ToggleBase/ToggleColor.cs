using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.Scripts.Common.ToggleBase
{
    public class ToggleColor : MonoBehaviour
    {
        public List<TextMeshProUGUI> texts;
        public List<Image> images;
        public Color colorOn;
        public Color colorOff;
        
        public void Show()
        {
            foreach (var a in texts)
            {
                a.color = colorOn;
            }

            foreach (var a in images)
            {
                a.color = colorOn;
            }
        }

        public void Hide()
        {
            foreach (var a in texts)
            {
                a.color = colorOff;
            }

            foreach (var a in images)
            {
                a.color = colorOff;
            }
        }
    }
}