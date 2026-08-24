using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 이펙트 데이터 활용 예제 클래스
    /// </summary>
    public class EffectTest : MonoBehaviour
    {
        [Header("Broadcasting on Channels")]
        [SerializeField] private EffectDataChannelSO _effectOneShot;

        private void Start()
        {
            PlayEffect();
        }

        public void PlayEffect()
        {
            Vector3 position = new Vector3( 0, 0, 0 );
            //EffectManager.Instance.EffectOneShot(EffectList.SphereEffect, position);
            _effectOneShot.RaiseEvent(EffectList.SphereEffect, position);
        }

    }
}