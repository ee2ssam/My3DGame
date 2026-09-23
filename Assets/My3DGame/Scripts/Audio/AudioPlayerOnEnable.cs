using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 활성화시 랜덤 오디오 플레이어 플레이
    /// 비활성화시 랜덤 오디오 플레이어 정지
    /// </summary>
    public class AudioPlayerOnEnable : MonoBehaviour
    {
        public RandomAudioPlayer player;
        public bool isStopOnDiable = false; //비활성화시 정지 여부

        private void OnEnable()
        {
            if(player != null)
            {
                player.PlayRandomClip();
            }
        }

        private void OnDisable()
        {
            if (player != null && isStopOnDiable == true)
            {
                player.audioSource.Stop();
            }
        }
    }
}