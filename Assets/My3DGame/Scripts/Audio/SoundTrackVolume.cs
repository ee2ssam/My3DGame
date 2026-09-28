using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 맵 특정지역에 배경음 트리거를 설치하여 
    /// 플레이어가 트리거에 들어오면 등록된 배경음 플레이
    /// 트리거에서 나가면 다시 기존 배경음 플레이
    /// </summary>
    public class SoundTrackVolume : MonoBehaviour
    {
        public LayerMask layers;            
        private SoundTrack soundTrack;      //배경음 플레이어

        private void OnEnable()
        {
            //참조
            soundTrack = GetComponentInParent<SoundTrack>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //레이어 마스트 체크 - 등록된 배경음 스택에 쌓고
            //기존 배경음을 페이드 시키고 등록된 배경음 플레이 한다
            if((layers.value & 1 << other.gameObject.layer) != 0)
            {
                soundTrack.PushTrack(this.name);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //레이어 마스트 체크 - 등록된 배경음 스택에서 꺼내고
            //등록된 배경음 페이드 시키고 기존의 배경음을 플레이 한다
            if ((layers.value & 1 << other.gameObject.layer) != 0)
            {
                soundTrack.PopTrack();
            }
        }
    }
}