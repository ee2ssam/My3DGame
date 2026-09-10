using UnityEngine;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 캐릭터의 데미지를 관리하는 클래스
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        #region Variables
        [Header("Health")]
        [SerializeField] private HealthConfigSO _healthCongfigSO;
        [SerializeField] private HealthSO _currentHealthSO;

        //무적모드 타이머
        [SerializeField] private float _invulnerableTimer = 0.5f;
        private float m_CountDown = 0f;

        //이벤트 함수
        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction OnDie;

        #endregion

        #region Property        
        public bool IsDeath { get; private set; }   //죽음 체크
        public bool IsInvulnerable { get; private set; }    //무적 모드

        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //_currentHealthSO null 체크해서 
            if (_currentHealthSO == null)
            {
                //null이면 체력 스크립터블 오브젝트 생성 후 체력 초기화
                _currentHealthSO = ScriptableObject.CreateInstance<HealthSO>();
                _currentHealthSO.SetMaxHealth(_healthCongfigSO.InitalHealth);
                _currentHealthSO.SetCurrentHealth(_healthCongfigSO.InitalHealth);
            }
        }

        private void Update()
        {
            //무적 모드 발동시 무적 타이머 작동
            if (IsInvulnerable)
            {
                m_CountDown += Time.deltaTime;
                if(m_CountDown >= _invulnerableTimer)
                {
                    IsInvulnerable = false;

                    //타이머 초기화
                    m_CountDown = 0f;
                }
            }     
            
        }
        #endregion

        #region Custom Method
        public void TakeDamage(float damage, GameObject damageSource)
        {
            //무적 모드, 죽음 체크
            if (IsInvulnerable || IsDeath)
                return;

            IsInvulnerable = true;

            //체력 연산
            _currentHealthSO.InflictDamage(damage);
            Debug.Log($"{gameObject.name}s Health : {_currentHealthSO.CurrentHealth}");

            //데미지 처리 (VFX, SFX, 애니메이션)
            OnDamaged?.Invoke(damage, damageSource);

            //죽음 처리
            if (_currentHealthSO.CurrentHealth <= 0f && IsDeath == false)
            {
                Die();
            }
        }

        private void Die()
        {
            IsDeath = true;

            //데미지 처리 (VFX, SFX, 애니메이션)
            OnDie?.Invoke();

        }
        #endregion
    }
}