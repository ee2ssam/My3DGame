using UnityEngine;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 반환 값이 없고 매개 변수가 int형 함수 이벤트 채널 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "new IntEventChannel", menuName = "Events/Int Event Channel")]
    public class IntEventChannelSO : ScriptableObject
    {
        public UnityAction<int> OnEventRaised;

        public void RaiseEvent(int value)
        {
            if (OnEventRaised != null)
            {
                OnEventRaised.Invoke(value);
            }
        }
    }
}