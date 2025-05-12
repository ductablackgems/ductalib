using System.Collections.Generic;
using _0.DucTALib.Scripts.Common;
using UnityEngine;

namespace _0.DucTALib.CustomButton
{
    public class ButtonCustomGroup : MonoBehaviour
    {
        public string groupName;
        public List<ButtonCustom> buttons;
        // public ButtonCustom this[int index] => buttons[index];
        private ButtonCustom currentButton;
        public ButtonCustom CurrentButton
        {
            get
            {
                if (currentButton == null)
                {
                    var config = SplashRemoteConfig.CustomConfigValue.groupButtonCustomConfigs.Find(x =>
                        x.groupName == groupName).buttonConfig;
                    currentButton = buttons[config.positionIndex];
                    currentButton.SetUpButton(config);
                }
                return currentButton;
            }
        }
    }
}