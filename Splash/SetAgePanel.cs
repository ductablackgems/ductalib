using _0.DucLib.Scripts.Common;
using _0.DucTALib.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.DucTALib.Splash
{
    public class SetAgePanel : MonoBehaviour
    {
        [SerializeField] private Transform buttonNext;
        [SerializeField] private float durationShowButton;
        [SerializeField] private Toggle policyToggle;
        [SerializeField] private TextMeshProUGUI ageText;
        [SerializeField] private TextMeshProUGUI leftAgeText;
        [SerializeField] private TextMeshProUGUI rightAgeText;
        public Transform bannerPos;
        private int currentAge = 2012;

        private void OnEnable()
        {
            // DOVirtual.DelayedCall(durationShowButton, () => { buttonNext.ShowButtonTween(); });
        }

        public void ToggleOnChange(bool isOn)
        {
            AudioManager.Instance.PlayClickSound();
            LoadSplash.instance.ResetCooldown(10);
            if (!policyToggle.isOn) buttonNext.HideObject();
            else if (policyToggle.isOn && !buttonNext.gameObject.activeSelf) buttonNext.ShowButtonTween();
        }

        public void ChangeAge(int age)
        {
            AudioManager.Instance.PlayClickSound();
            currentAge += age;
            ageText.text = currentAge.ToString();
            leftAgeText.text = (currentAge - 1).ToString();
            rightAgeText.text = (currentAge + 1).ToString();
            LoadSplash.instance.ResetCooldown(10);
            if (policyToggle.isOn && !buttonNext.gameObject.activeSelf) buttonNext.ShowButtonTween();
            else if (!policyToggle.isOn) buttonNext.HideObject();
        }

        public void OnConfirmAge()
        {
        }
    }
}