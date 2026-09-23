using UnityEngine;
using System.Collections.Generic;

namespace My3DGame
{
    /// <summary>
    /// 배경음 플레이어, 스택 구조를 이용하여 배경음 플레이 리스트를 만든다
    /// </summary>
    public class SoundTrack : MonoBehaviour
    {
        #region Variables
        public AudioSource[] audioSources;

        public float soundTrackVolume = 1f;
        public float initialVolume = 1f;

        public float volumeRampSpeed = 5f;      //볼륨 페이드 속도
        public bool playOnStart = true;

        private AudioSource activeAudio;        //현재 플레이되고 있는 오디오
        private AudioSource fadeAudio;          //앞선 오디오, 사라지는 오디오

        private float volumeVelocity, fadeVelocity; //페이드 볼륨 속도
        private float volume;                       //볼륨

        private Stack<string> trackStack = new Stack<string>();
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //사운드 트랙 초기화
            trackStack.Clear();

            //사운드 트랙 만들기
            if (audioSources.Length > 0)
            {
                //기본 배경음 플레이
                activeAudio = audioSources[0];
                foreach (AudioSource source in audioSources)
                    source.volume = 0f;

                trackStack.Push(audioSources[0].name);
                if(playOnStart ==  true)
                {
                    Play();
                }
            }
            //볼륨 초기화
            volume = initialVolume;
        }

        private void Update()
        {
            if(activeAudio != null)
            {
                activeAudio.volume = Mathf.SmoothDamp(activeAudio.volume,
                    volume * soundTrackVolume, ref volumeVelocity, volumeRampSpeed, 1);
            }

            if(fadeAudio != null)
            {
                fadeAudio.volume = Mathf.SmoothDamp(fadeAudio.volume,
                    0, ref fadeVelocity, volumeRampSpeed, 1);

                if(Mathf.Approximately(fadeAudio.volume, 0f))
                {
                    fadeAudio.Stop();
                    fadeAudio = null;
                }
            }
        }
        #endregion

        #region Custom Method
        //스택 push
        public void PushTrack(string name)
        {
            trackStack.Push(name);
            //트랙에 추가한 오디오 플레이
            Enqueue(name);
        }

        //스택 pop, 맨 바닥에 있는 트랙은 꺼내지 않는다
        public void PopTrack()
        {
            if(trackStack.Count > 1)
            {
                trackStack.Pop();
            }
            //꺼낸 오디오 바로 아래에 있는 오디오를 플레이
            //스택의 맨 위에 있는 오디오 플레이
            Enqueue(trackStack.Peek());
        }

        //매개변수로 들어온 오디오 플레이
        public void Enqueue(string name)
        {
            foreach (AudioSource source in audioSources)
            {
                if(source.name == name)
                {
                    //현재 플레이 되고 있는 오디오를 페이드 전환
                    fadeAudio = activeAudio;
                    //현재 오디오 셋팅후 플레이
                    activeAudio = source;
                    if(!activeAudio.isPlaying)
                        activeAudio.Play();

                    break;
                }
            }
        }

        //현재 오디오 플레이
        public void Play()
        {
            if(activeAudio != null)
                activeAudio.Play();
        }

        //오디오 플레이 정지
        public void Stop()
        {
            //모든 오디오 정지
            foreach (AudioSource source in audioSources)
                source.Stop();
        }

        //볼륨 값 설정
        public void SetVolume(float volume)
        {
            this.volume = volume;
        }
        #endregion
    }
}