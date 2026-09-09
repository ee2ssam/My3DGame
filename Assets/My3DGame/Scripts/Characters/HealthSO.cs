using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 캐릭터의 체력을 관리하는 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "new Health", menuName = "Entity/Health")]
    public class HealthSO : ScriptableObject
    {
        #region Variables 
        [SerializeField] private float _maxHealth;       //최대 체력
        [SerializeField] private float _currentHealth;   //현재 체력
        #endregion

        #region Property
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        public float HealthRatio => _currentHealth / _maxHealth;
        #endregion

        #region Custom Method
        //MaxHealth 초기화
        public void SetMaxHealth(float value)
        {
            _maxHealth = value;
        }

        //CurrentHealth 초기화
        public void SetCurrentHealth(float value)
        {
            _currentHealth = value;
        }

        //데미지 연산
        public void InflictDamage(float value)
        {
            _currentHealth -= value;
            if (_currentHealth < 0f)
                _currentHealth = 0f;
        }

        //회복 연산
        public void RestoreHealth(float value)
        {
            _currentHealth += value;
            if(_currentHealth > _maxHealth)
                _currentHealth = _maxHealth;
        }
        #endregion
    }
}