using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace My3DGame
{
    /// <summary>
    /// 사운드 뱅크(오디오클립 배열)을 만들고 그 중에 하나를 랜덤하게 선택하여 플레이 한다
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioPlayer : MonoBehaviour
    {
        /// <summary>
        /// 등록된 매터리얼에 플레이되는 사운드 목록
        /// </summary>
        [Serializable]
        public class MaterialAudioOverride
        {
            public Material[] materials;
            public SoundBank[] banks;
        }

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
        public MaterialAudioOverride[] overrides;

        public bool randomizePitch = true;                  //재생 속도 랜덤 여부
        public float pitchRandomRange = 0.2f;               //랜덤 속도 +- 범위
        public float palyDelay = 0f;                        //딜레이 플레이 시간

        [HideInInspector] public bool playing;
        [HideInInspector] public bool canPlay;

        //참조
        protected AudioSource m_AudioSource;
        protected Dictionary<Material, SoundBank[]> m_Lookup = new Dictionary<Material, SoundBank[]>();
        #endregion

        #region Property
        public AudioSource audioSource => m_AudioSource;
        public AudioClip clip { get; private set; }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            m_AudioSource = GetComponent<AudioSource>();
            //등록된 메터리얼을 딕셔너리 key로 하여 플레이될 뱅크를 value로 저장
            for (int i = 0; i < overrides.Length; i++)
            {
                foreach(var material in overrides[i].materials)
                {
                    m_Lookup[material] = overrides[i].banks;
                }
            }
        }
        #endregion

        #region Custom Method
        //디폴트 뱅크 클립 랜덤 플레이
        public void PlayRandomClip()
        {
            clip = InternalPlayRandomClip(null, bankId:0);
        }

        //매개변수로 들어온 매터리얼로 등록된 뱅크 클립 랜덤 플레이
        public AudioClip PlayRandomClip(Material overrideMaterial, int bankId = 0)
        {
            if (overrideMaterial == null) return null;

            return InternalPlayRandomClip(overrideMaterial, bankId);
        }


        //매개변수로 메터리얼을 받아 등록된 지정된(bankId) 뱅크를 플레이 시킨다
        //메터리얼이 null 이면 디폴트 뱅크를 플레이 시킨다
        private AudioClip InternalPlayRandomClip(Material overrideMaterial, int bankId)
        {
            SoundBank[] banks = null;
            SoundBank bank = defaultBank;

            //overrideMaterial null 체크
            if (overrideMaterial != null)
            {
                if(m_Lookup.TryGetValue(overrideMaterial, out banks))
                {
                    //bankId 범위 체크
                    if (bankId < banks.Length)
                        bank = banks[bankId];
                }
            }

            //bank.clips 체크
            if (bank.clips == null || bank.clips.Length == 0)
                return null;

            var clip = bank.clips[Random.Range(0, bank.clips.Length)];
            if (clip == null)
                return null;

            m_AudioSource.pitch = randomizePitch ?
                Random.Range(1.0f - pitchRandomRange, 1.0f + pitchRandomRange) : 1.0f;
            m_AudioSource.clip = clip;
            m_AudioSource.PlayDelayed(palyDelay);

            return clip;
        }
        #endregion
    }
}