using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 캐릭터의 체력 초기값을 저장하는 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "new HealthConfig", menuName = "Entity/Config/Health Config")]
    public class HealthConfigSO : ScriptableObject
    {
        [SerializeField] private float _initalHealth;        //체력 초기값

        public float InitalHealth => _initalHealth;         //읽기 전용
    }
}