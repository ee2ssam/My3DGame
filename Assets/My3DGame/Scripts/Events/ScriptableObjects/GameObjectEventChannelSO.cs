using UnityEngine;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 반환 값이 없고 매개 변수가 GameObject인 함수 이벤트 채널 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "new GameObjectEventChannel", menuName = "Events/GameObject Event Channel")]
    public class GameObjectEventChannelSO : ScriptableObject
    {
        public UnityAction<GameObject> OnEventRaised;

        public void RaiseEvent(GameObject value)
        {
            if (OnEventRaised != null)
            {
                OnEventRaised.Invoke(value);
            }
        }
    }
}