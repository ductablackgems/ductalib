using System;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class RewardNotReady : MonoBehaviour
    {
        public Transform content;
        public Transform parent;

        private void Awake()
        {
            CallAdsManager.rewardNotReadyAction += Show;
        }

        private void OnDestroy()
        {
            CallAdsManager.rewardNotReadyAction -= Show;
        }

        private void Show()
        {
            parent.ShowObject();
            content.ScaleInPopup();
        }

        public void Hide()
        {
            AudioController.Instance.PlayClickSound();
            parent.HideObject();
        }
    }
}