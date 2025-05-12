using System;
using System.Collections.Generic;

namespace _0.DucTALib.CustomButton
{
    [Serializable]
    public class CustomButtonConfig
    {
        public int positionIndex;
        public bool customButton;
        public string backgroundColor;
        public string imageName;
        public string textValue;
        public string textColor;
    }

    [Serializable]
    public class CustomButtonGroupConfig
    {
        public string groupName;
        public CustomButtonConfig buttonConfig;
        
    }
    
    // [Serializable]
    // public class CustomButtonConfigResponsive
    // {
    //     public List<CustomButtonConfig> groupButtonCustomConfigs;
    // }
}