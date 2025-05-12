using _0.DucLib.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.CustomButton
{
    
    public class ButtonCustom : MonoBehaviour
    {
        public Image buttonImg;
        public TextMeshProUGUI tmpText;
        public void SetUpButton(CustomButtonConfig config)
        {
            if(!config.customButton) return;

            if (config.backgroundColor != "none")
            {
                buttonImg.color = CommonHelper.HexToColor(config.backgroundColor);
            }
            
            if (config.imageName != "none")
            {
                // buttonImg.color = CommonHelper.HexToColor(config.backgroundColor);
            }

            if (config.textValue != "none")
            {
                tmpText.text = config.textValue;
            }
            
            if (config.textColor != "none")
            {
                tmpText.color = CommonHelper.HexToColor(config.textColor);
            }
        }
    }
}