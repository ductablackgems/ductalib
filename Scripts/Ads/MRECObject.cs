using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucLib.Scripts.Ads
{
    public class MRECObject : MonoBehaviour
    {
        public int spaceTop;
        public int spaceRight;
        public RectTransform content;
        public RectTransform parent;
        public List<RectTransform> resizedObjects = new List<RectTransform>();

        public string pos;
        // [Button]
        private void SetDefault()
        {
            parent.anchoredPosition = new Vector2(spaceRight, spaceTop);
            content.sizeDelta = new Vector2(540, 450);
            content.anchoredPosition = new Vector2(-(content.sizeDelta.x / 2), content.anchoredPosition.y);
            ResizeObjects();
        }

        private void ResizeObjects()
        {
            foreach (var rect in resizedObjects)
            {
                var delta = rect.rectTransform().sizeDelta;
                delta.x = content.sizeDelta.x;
                rect.rectTransform().sizeDelta = delta;
            }
        }
        public void ShowMREC(Camera camera, bool ignoreDpiLimit = true)
        {
#if UNITY_EDITOR
            SetDefault();
            return;
#endif
            if (!ignoreDpiLimit && CallAdsManager.GetDPIDevice() > 2.6f)
            {
                SetDefault();
                HideMREC();
                return;
            }
            CallAdsManager.ResizeMREC(content.rectTransform());
            parent.anchoredPosition = new Vector2(spaceRight, spaceTop);
            content.anchoredPosition = new Vector2(-(content.sizeDelta.x / 2), content.anchoredPosition.y);
            ResizeObjects();
            HideMREC();
            CallAdsManager.ShowMRECApplovin(content.gameObject, camera, pos);
            // StartCoroutine(WaitFrame());
        }


     

        public void HideMREC()
        {
            CallAdsManager.HideMRECApplovin();
        }
    }
}