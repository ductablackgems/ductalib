using BG_Library.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _0.DucTALib.Scripts.Common
{
    [CreateAssetMenu(fileName = "AudioController", menuName = "DucLib/AudioController")]

    public class AudioController : ResourceSOManager<AudioController>
    {
        [FoldoutGroup("Music")] [SerializeField]
        private PlayAudioChannelSO BGM;
        [FoldoutGroup("Music")] [SerializeField]
        private PlayAudioChannelSO BGMMenu;
        [FoldoutGroup("Single")] [SerializeField]
        private PlayAudioChannelSO clickAud;
        
        [FoldoutGroup("Single")] [SerializeField]
        private PlayAudioChannelSO pop;

        [FoldoutGroup("Single")] [SerializeField]
        private PlayAudioChannelSO coin;
        
        [FoldoutGroup("Single")] [SerializeField]
        private PlayAudioChannelSO noMoneySound;
        public void PlayBGM()
        {
            BGM?.Play();
        }
        
        public void StopBGM()
        {
            BGM?.Stop();
        }
        public void PlayBGMMenu()
        {
            BGMMenu?.Play();
        }
        
        public void StopBGMMenu()
        {
            BGMMenu?.Stop();
        }
       
        public void PlayClickSound()
        {
            clickAud?.Play();
        }
       
        #region Custom

        public void PlayConveyor()
        {
            
        }
        public void PlayPop()
        {
            pop?.Play();
        }

        public void PlayCoin()
        {
            coin?.Play();
        }

        public void NoMoneySound()
        {
            noMoneySound?.Play();
        }
        #endregion
    }

}