using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class TestMonet : MonoBehaviour
    {
        public GameObject trans;

        public void ShowAds()
        {
            CallAdsManager.ShowONA("Test1");
        }

        public void HideAds()
        {
            CallAdsManager.CloseONA("Test1");
        }

        public void LoadAds()
        {
            CallAdsManager.InitONA("Test1");
        }
    }
}