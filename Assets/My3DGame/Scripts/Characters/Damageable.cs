using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 캐릭터의 데미지를 관리하는 추상 클래스
    /// </summary>
    public abstract class Damageable : MonoBehaviour
    {
        #region abstract
        public abstract void TakeDamage(float damage, GameObject damageSource);
        protected abstract void Die();
        public abstract void Cure(float healthToAdd);
        #endregion

        #region Variables
        [Header("Health")]
        [SerializeField] protected HealthConfigSO _healthCongfigSO;
        protected HealthSO _currentHealthSO;

        //무적모드 타이머
        [SerializeField] protected float _invulnerableTimer = 0.5f;
        private float m_CountDown = 0f;

        //이벤트 함수
        //public UnityAction<float, GameObject> OnDamaged;
        //public UnityAction OnDie;
        #endregion

        #region Property        
        public bool IsDeath { get; protected set; }   //죽음 체크
        public bool IsInvulnerable { get; protected set; }    //무적 모드

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
            //죽음 체크
            if (IsDeath)
                return;

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
        //원샷원킬
        public virtual void Kill()
        {
            TakeDamage(_currentHealthSO.CurrentHealth, null);
        }

        //부활하기
        public virtual void Revive()
        {
            //체력 초기화
            _currentHealthSO.SetMaxHealth(_healthCongfigSO.InitalHealth);

            IsDeath = false;
        }
        #endregion
    }
}