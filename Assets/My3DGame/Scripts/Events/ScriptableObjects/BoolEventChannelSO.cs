using UnityEngine;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 반환 값이 없고 매개 변수가 bool형 함수 이벤트 채널 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "new BoolEventChannel", menuName = "Events/Bool Event Channel")]
    public class BoolEventChannelSO : ScriptableObject
    {
        public UnityAction<bool> OnEventRaised;

        public void RaiseEvent(bool value)
        {
            if (OnEventRaised != null)
            {
                OnEventRaised.Invoke(value);
            }
        }
    }
}