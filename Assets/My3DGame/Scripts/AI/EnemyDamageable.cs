using UnityEngine;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 적 캐릭터의 데미지를 관리하는 클래스
    /// </summary>
    public class EnemyDamageable : Damageable
    {
        #region Variables
        //이벤트 함수
        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction OnDie;
        public UnityAction OnHeal;
        #endregion

        #region abstract Method
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
            OnDamaged?.Invoke(damage, damageSource);            //UnityAction 이벤트 함수

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
            OnDie?.Invoke();                //UnityAction 이벤트 함수
        }

        public override void Cure(float healthToAdd)
        {
            //죽음 체크
            if (IsDeath)
                return;

            _currentHealthSO.RestoreHealth(healthToAdd);

            //회복처리 (VFX, SFX, 애니메이션, UI)
            OnHeal?.Invoke();
        }
        #endregion
    }
}