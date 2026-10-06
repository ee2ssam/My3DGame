using UnityEngine;
using UnityEngine.AI;

namespace My3DGame
{
    /// <summary>
    /// 타겟팅된 적을 추격하는 걷기 상태, State 상속
    /// 적을 잃어 버리면 대기 상태로 변경
    /// 적이 공격범위 안에 들어오면 공격 상태로 변경
    /// </summary>
    public class WalkState : State
    {
        #region Variables
        //참조
        private Animator m_Animator;
        private NavMeshAgent m_Agent;

        //애니메이션 파라미터
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        #endregion

        #region Custom Method
        public override void OnInitialize()
        {
            //참조
            m_Animator = enemy.GetComponent<Animator>();
            m_Agent = enemy.GetComponent<NavMeshAgent>();
        }

        public override void OnEnter()
        {
            //초기화
            if(enemy.Target)
            {
                m_Agent.stoppingDistance = 1.5f;
                m_Agent.SetDestination(enemy.Target.position);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            if (enemy.Target)
            {
                if (enemy.IsAttackable)
                {
                    stateMachine.ChangeState(typeof(AttackState));
                }
                else
                {
                    m_Agent.SetDestination(enemy.Target.position);

                    //애니메이션
                    float speed = m_Agent.velocity.magnitude;
                    m_Animator.SetFloat(m_HashForwardSpeed, speed);
                }
            }
            else //타겟을 놓친것
            {
                enemy.ChangeState(typeof(IdleState));
            }
        }

        public override void OnExit()
        {
            //m_Agent 길찾기 기능 초기화
            m_Agent.ResetPath();
        }
        #endregion
    }
}