using _0.Custom.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _0.DucTALib.Scripts.Common.ToggleBase
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleService : MonoBehaviour
    {
        [SerializeField] protected Toggle toggle;

        public UnityEvent isOn;
        public UnityEvent isOff;
        public Toggle Toggle => toggle;
        private bool isInitialized = false;
        private void Awake()
        {
            toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    IsOnItem();
                }
                else
                {
                    IsOffItem();
                }
            });
            isInitialized = true;
        }

        public void IsOnItem()
        {
            isOn?.Invoke();
            if(isInitialized) AudioManager.Instance.PlayClickSound();
        }

        protected void IsOffItem()
        {
            isOff?.Invoke();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (toggle == null) toggle = GetComponent<Toggle>();
        }
#endif
    }
}