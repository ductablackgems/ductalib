using System;
using System.Collections.Generic;
using TMPro;

namespace _0.DucTALib.Scripts.Common.ToggleBase
{
    public class ToggleText
    {
        [Serializable]
        public class ChangeTxtToggle
        {
            public TextMeshProUGUI tmp;
            public string txtOn;
            public string txtOff;

            public void Show()
            {
                tmp.text = txtOn;
            }

            public void Hide()
            {
                tmp.text = txtOff;
            }
        }
        
        public List<ChangeTxtToggle> toggles = new List<ChangeTxtToggle>();
        public void Show()
        {
            foreach (var a in toggles)
            {
                a.Show();
            }

          
        }

        public void Hide()
        {
            foreach (var a in toggles)
            {
                a.Hide();
            }

        }
    }
}