using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 플레이어의 데미지를 관리하는 클래스
    /// </summary>
    public class PlayerDamageable : Damageable
    {
        #region Variables
        [Header("Broadcasting On Channels")]
        [SerializeField] private DamagedEventChannelSO _damageEventSO;
        [SerializeField] private VoidEventChannelSO _deathEventSO;
        [SerializeField] private VoidEventChannelSO _healEventSO;
        [SerializeField] private VoidEventChannelSO _updateHealthUIEventSO;

        [Header("Listening To Channels")]
        [SerializeField] private FloatEventChannelSO _restoreHealthEventSO;
        #endregion

        #region Unity Event Method        
        private void OnEnable()
        {
            //ScriptableObject 이벤트 채널 등록
            if (_restoreHealthEventSO != null)
                _restoreHealthEventSO.OnEventRaised += Cure;
        }

        private void OnDisable()
        {
            //ScriptableObject 이벤트 채널 제거
            if (_restoreHealthEventSO != null)
                _restoreHealthEventSO.OnEventRaised -= Cure;
        }
        #endregion

        #region Custom Method
        public override void TakeDamage(float damage, GameObject damageSource)
        {
            //무적 모드, 죽음 체크
            if (IsInvulnerable || IsDeath)
                return;

            IsInvulnerable = true;

            //체력 연산
            _currentHealthSO.InflictDamage(damage);
            Debug.Log($"{gameObject.name}s Health : {_currentHealthSO.CurrentHealth}");

            //데미지 처리 (VFX, SFX, 애니메이션, UI)
            //OnDamaged?.Invoke(damage, damageSource);            //UnityAction 이벤트 함수
            if (_damageEventSO != null)
                _damageEventSO.RaiseEvent(damage, damageSource);    //ScriptableObject 이벤트 채널

            if(_updateHealthUIEventSO != null)
                _updateHealthUIEventSO.RaiseEvent();

            //죽음 처리
            if (_currentHealthSO.CurrentHealth <= 0f && IsDeath == false)
            {
                Die();
            }
        }

        protected override void Die()
        {
            IsDeath = true;

            //데미지 처리 (VFX, SFX, 애니메이션)
            //OnDie?.Invoke();                //UnityAction 이벤트 함수
            if (_deathEventSO != null)
                _deathEventSO.RaiseEvent();     //ScriptableObject 이벤트 채널

        }
        //회복하기
        public override void Cure(float healthToAdd)
        {
            //죽음 체크
            if (IsDeath)
                return;

            _currentHealthSO.RestoreHealth(healthToAdd);

            //회복처리 (VFX, SFX, 애니메이션, UI)
            if (_healEventSO != null)
                _healEventSO.RaiseEvent();

            if (_updateHealthUIEventSO != null)
                _updateHealthUIEventSO.RaiseEvent();
        }

        public override void Revive()
        {
            base.Revive();

            if (_updateHealthUIEventSO != null)
                _updateHealthUIEventSO.RaiseEvent();
        }
        #endregion
    }
}