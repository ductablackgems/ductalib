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
        public Image background;
        public TextMeshProUGUI text;

        public void CustomTxt(string value)
        {
            if(text == null) text = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            if(value != "none") text.text = value;
        }
        
        
        public void CustomTxtColor(string value)
        {
            if(text == null) text = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            if (value != "none") text.color = CommonHelper.HexToColor(value);
        }
        
        public void CustomButtonColor(string value)
        {
            if(type != ButtonType.WithImage) return;
            if(background == null) background = transform.GetComponent<Image>();
            if (value != "none") background.color = CommonHelper.HexToColor(value);
        }
    }
}