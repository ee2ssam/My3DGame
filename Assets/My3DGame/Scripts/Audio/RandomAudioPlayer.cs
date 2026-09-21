using UnityEngine;
using System;

namespace My3DGame
{
    /// <summary>
    /// 사운드 뱅크(오디오클립 배열)을 만들고 그 중에 하나를 랜덤하게 선택하여 플레이 한다
    /// </summary>
    public class RandomAudioPlayer : MonoBehaviour
    {
        /// <summary>
        /// 사운드 뱅크 : 오디오클립 배열
        /// </summary>
        [Serializable]
        public class SoundBank
        {
            public string name;         //사운드 뱅크 이름
            public AudioClip[] clips;   //오디오클립 배열
        }

        #region Variables
        public SoundBank defaultBank = new SoundBank();     //기본 사운드 뱅크
        #endregion
    }
}