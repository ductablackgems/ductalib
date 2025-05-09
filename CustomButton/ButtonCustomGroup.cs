using System.Collections.Generic;
using UnityEngine;

namespace _0.DucTALib.CustomButton
{
    public class ButtonCustomGroup : MonoBehaviour
    {
        public string groupName;
        public List<ButtonCustom> buttons;
        // public ButtonCustom this[int index] => buttons[index];
        public ButtonCustom currentButton;
    }
}