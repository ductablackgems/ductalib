using BG_Library.NET;
using TMPro;
using UnityEngine;

namespace _0.DucLib.Scripts.Ads
{
    public class TestMonet : MonoBehaviour
    {
        public DucTALib.Scripts.Common.UIDragObject trans;

        public void ShowAds()
        {
            CallAdsManager.ShowONA("Test1", trans.rect);
        }

        public void HideAds()
        {
            CallAdsManager.CloseONA("Test1");
        }

        public void LoadAds()
        {
            CallAdsManager.InitONA("Test1");
        }

        public void InitBN()
        {
            CallAdsManager.LoadBanner();
        }

        public void ShowBanner()
        {
            CallAdsManager.ShowBanner();
        }

        public void ShowCollapse()
        {
            CallAdsManager.ShowBannerCollapsible();
        }
        
    }
}