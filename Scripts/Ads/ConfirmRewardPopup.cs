using System;
using System.Collections.Generic;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class ConfirmRewardPopup : MonoBehaviour
    {
        public List<Transform> content;
        public Action completeCallback;
        private Transform currentContent;
        public void Show(int id, Action callback)
        {
            gameObject.ShowObject();
            currentContent = content[id];
            currentContent.ShowObject();
            currentContent.ScaleInPopup();
            completeCallback = callback;
        }

        public void Confirm()
        {
            CallAdsManager.ShowRewardVideo("",() =>
            {
                currentContent.HideObject();
                completeCallback?.Invoke();
                completeCallback = null;
                currentContent = null;
                gameObject.HideObject();
            });
        }

        public void Cancel()
        {
            currentContent.HideObject();
            completeCallback = null;
            currentContent = null;
            gameObject.HideObject();
        }
    }
}