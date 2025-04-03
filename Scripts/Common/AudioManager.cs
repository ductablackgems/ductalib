﻿using BG_Library.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _0.DucTALib.Scripts.Common
{
    [CreateAssetMenu(fileName = "AudioManager", menuName = "DucLib/AudioManager")]

    public class AudioManager : ResourceSOManager<AudioManager>
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
        #endregion
    }

}