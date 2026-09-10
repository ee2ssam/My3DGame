using UnityEngine;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 반환 값이 없고 매개 변수가 float, GameObejt 함수 이벤트 채널 스크립터블 오브젝트
    /// 데미지 처리하는 이벤트 채널
    /// </summary>
    [CreateAssetMenu(fileName = "new DamagedEventChannel", menuName = "Events/Health/Damaged Event Channel")]
    public class DamagedEventChannelSO : ScriptableObject
    {
        public UnityAction<float, GameObject> OnEventRaised;

        public void RaiseEvent(float value1, GameObject value2)
        {
            if (OnEventRaised != null)
            {
                OnEventRaised.Invoke(value1, value2);
            }
        }
    }
}