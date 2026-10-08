using UnityEngine;
using System.Collections;

namespace My3DGame
{
    /// <summary>
    /// 적을 관리하는 클래스, 모든 적의 부모 클래스
    /// 속성: 상태머신 
    /// 기능: 적의 기본 기능
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        #region Variables
        //참조
        protected EnemyDamageable m_Damageable;
        protected DetectionModule m_DetectionModule;
        protected Animator m_Animator;
        protected CharacterController m_CharacterController;

        public MeleeWeapon meleeWeapon;

        protected StateMachine m_StateMachine;  //상태를 관리하는 상태머신

        //공격
        [SerializeField] protected float attackRange = 2f; //공격 범위
        [SerializeField] protected float attackDelay = 2f; //공격 딜레이

        //회전
        [SerializeField] protected float rotateSpeed = 10f; //타겟을 향해 회전속도

        //넉백
        [SerializeField] protected float knockbackForce = 1.0f; //밀리는 크기
        [SerializeField] protected float knockbackDuration = 0.2f; //넉백 시간


        //애니메이션 파라미터
        readonly int m_HashHurt = Animator.StringToHash("Hurt");
        #endregion

        #region Property
        public Transform Target => m_DetectionModule.Target;
        public float DistanceToTarget => m_DetectionModule.DistanceToTarget;
        public float AttackRange => attackRange;
        public float AttackDelay => attackDelay;
        public bool IsAttackable
        {
            get
            {
                if(Target)
                {
                    if(DistanceToTarget <= AttackRange)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Unity Event Method
        protected virtual void Awake()
        {
            //참조
            m_Damageable = GetComponent<EnemyDamageable>();
            m_DetectionModule = GetComponent<DetectionModule>();
            m_Animator = GetComponent<Animator>();
            m_CharacterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            //Damageable 이벤트 함수 등록
            m_Damageable.OnDamaged += OnDamaged;
            m_Damageable.OnDie += OnDie;
            //m_Damageable.OnHeal += OnHeal;
        }

        private void OnDisable()
        {
            //Damageable 이벤트 함수 등록
            m_Damageable.OnDamaged += OnDamaged;
            m_Damageable.OnDie += OnDie;
            //m_Damageable.OnHeal += OnHeal;
        }

        protected virtual void Start()
        {
            //상태머신 생성 및 (적의 기본)상태 생성 후 등록
            m_StateMachine = new StateMachine(this, new IdleState());
            m_StateMachine.RegisterState(new WalkState());
            m_StateMachine.RegisterState(new AttackState());
            m_StateMachine.RegisterState(new DeathState());

            //상속 받으면 추가로 새로운 상태를 등록한다
            //...
        }

        protected virtual void Update()
        {
            //현재 상태의 업데이트를 실행
            m_StateMachine.Update(Time.deltaTime);
        }
        #endregion

        #region Custom Method
        //상태 변경
        public State ChangeState(System.Type newType)
        {
            return m_StateMachine.ChangeState(newType);
        }

        public void SetAttackDelay(float delay)
        {
            attackDelay = delay;
        }

        private void OnDamaged(float damage, GameObject damageSource)
        {
            //애니메이션 변경
            m_Animator.SetTrigger(m_HashHurt);

            //넉백 : 뒤로 밀어낸다
            Vector3 displacement = (-transform.forward) * knockbackForce;
            StartCoroutine(KnockbackRoutine(displacement, knockbackDuration));

            //상태 변경
            ChangeState(typeof(IdleState));
        }

        IEnumerator KnockbackRoutine(Vector3 displacement, float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = 1 - (elapsed / duration);
                Vector3 move = displacement * t * Time.deltaTime / duration;
                m_CharacterController.Move(move);

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        private void OnDie()
        {
            //상태 변경
            ChangeState(typeof(DeathState));

            //죽음 설정
        }

        //타겟을 바라본다 - 공격시
        public void FaceToTarget()
        {
            //타겟 체크
            if (Target == null)
                return;

            //방향을 구하고 그 방향으로 회전
            Vector3 dir = (Target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,
                        Time.deltaTime * rotateSpeed);
        }

        //공격 시작 - 애니메이션 이벤트 호출
        public void MeleeAttackStart(int throwingAttack = 0)
        {
            Debug.Log("Enemy MeleeAttackStart : m_InAttack = true");
            meleeWeapon.StartAttack(throwingAttack != 0);
        }

        //공격 끝 - 애니메이션 이벤트 호출
        public void MeleeAttackEnd()
        {
            Debug.Log("Enemy MeleeAttackEnd : m_InAttack = false");
            meleeWeapon.EndAttack();
        }
        #endregion


    }
}