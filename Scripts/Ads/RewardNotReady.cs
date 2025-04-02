using System;
using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class RewardNotReady : MonoBehaviour
    {
        public Transform content;
        private void OnEnable()
        {
            content.ScaleInPopup();
        }

        public void Hide()
        {
            AudioManager.Instance.PlayClickSound();
            gameObject.HideObject();
        }
    }
}