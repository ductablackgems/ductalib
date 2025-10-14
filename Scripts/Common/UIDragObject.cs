using UnityEngine;
using UnityEngine.EventSystems;

namespace _0.DucTALib.Scripts.Common
{
    public class UIDragObject : MonoBehaviour, IDragHandler
    {
        RectTransform rt;
        Vector2 off;
        Canvas canvas;

        void Awake()
        {
            rt = (RectTransform)transform;
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnDrag(PointerEventData e)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform, e.position, e.pressEventCamera, out var lp);
            rt.localPosition = lp;
        }
    }
}