using _0.DucLib.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.CustomButton
{
    public class ButtonCustom : MonoBehaviour
    {
        public enum ButtonType
        {
            ScreenButton,
            TextOnly,
            WithImage
        }

        public enum ButtonPosition
        {
            Center,
            TopRight,
            TopLeft,
            BottomRight,
            BottomLeft,
            Bottom
        }
        public ButtonType type;
        public ButtonPosition pos;
    }
}