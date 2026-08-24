using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 이펙트 데이터를 가져다가 이펙트 플레이 시키는 이벤트 예제
    /// </summary>
    [CreateAssetMenu(fileName = "new EffectDataChannel", menuName = "Events/Effect Data Channel")]
    public class EffectDataChannelSO : ScriptableObject
    {
        public EffectOneShotAction OnEffectOneShotRaised;

        public GameObject RaiseEvent(EffectList effectList, Vector3 position)
        {
            GameObject effectGo = null;

            if(OnEffectOneShotRaised != null)
            {
                effectGo = OnEffectOneShotRaised.Invoke(effectList, position);
            }

            return effectGo;
        }
    }

    public delegate GameObject EffectOneShotAction(EffectList effectList, Vector3 position);

}