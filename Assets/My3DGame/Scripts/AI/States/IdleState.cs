using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 대기 상태를 관리하는 클래스, State를 상속 받는다
    /// 적 디텍션 되면 걷기(추격) 상태 변경 
    /// 공격 범위에 들어오면 공격 상태 변경, 대기 상태에서 공격 딜레이 체크
    /// 패트롤이 가능한 적이면 일정 시간이 지나면 다음 웨이 포인트로 이동한다
    /// </summary>
    public class IdleState : State
    {
        #region Variables
        //참조
        private Animator m_Animator;

        //패트롤
        private bool m_IsPatrol = false;
        private float m_MinTime = 0f;
        private float m_MaxTime = 2f;
        private float m_IdleTime = 0f;  //대기시간

        //애니메이션 파라미터
        //readonly int m_Hash = Animator.StringToHash("");
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        #endregion

        #region Custom Method
        public override void OnInitialize()
        {
            //참조
            m_Animator = enemy.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            //애니메이터 파라미터 셋팅
            m_Animator.SetFloat(m_HashForwardSpeed, 0f);

            //패트롤 초기값 설정
            if(enemy is EnemyPatrol)
            {
                m_IsPatrol = true;
                m_IdleTime = Random.Range(m_MinTime, m_MaxTime);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            //적 체크
            if(enemy.Target)
            {
                if(enemy.IsAttackable)
                {
                    //공격 딜레이 체크
                    if(enemy.AttackDelay <= stateMachine.ElapseTime)
                    {
                        stateMachine.ChangeState(typeof(AttackState));
                    }

                    //공격대상을 바라본다
                    enemy.FaceToTarget();
                }
                else
                {
                    stateMachine.ChangeState(typeof(WalkState));
                }
            }
            else if(m_IsPatrol)
            {
                //대기 타이머 체크
                if(stateMachine.ElapseTime > m_IdleTime)
                {
                    stateMachine.ChangeState(typeof(PatrolState));
                }
            }
        }
        #endregion
    }
}