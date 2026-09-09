using UnityEngine;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 반환 값이 없고 매개 변수가 float형 함수 이벤트 채널 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "new FloatEventChannel", menuName = "Events/Float Event Channel")]
    public class FloatEventChannelSO : ScriptableObject
    {
        public UnityAction<float> OnEventRaised;

        public void RaiseEvent(float value)
        {
            if (OnEventRaised != null)
            {
                OnEventRaised.Invoke(value);
            }
        }
    }
}