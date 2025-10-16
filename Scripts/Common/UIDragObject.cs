using System;
using _0.DucLib.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _0.DucTALib.Scripts.Common
{
    public class UIDragObject : MonoBehaviour, IDragHandler
    {
        RectTransform rt;
        Vector2 off;

        Canvas canvas;
        public RectTransform rect;
        public TextMeshProUGUI tmp;

        void Awake()
        {
            rt = (RectTransform)transform;
            canvas = GetComponentInParent<Canvas>();
        }

        private void Update()
        {
            var vv = CommonHelper.ObjectToOverlayPos(rect);
            tmp.text = $"{vv.x:F2},{vv.y:F2}";
        }

       

        public void OnDrag(PointerEventData e)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform, e.position, e.pressEventCamera, out var lp);
            rt.localPosition = lp;
        }
    }
}