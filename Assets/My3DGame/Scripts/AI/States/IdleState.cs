using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 대기 상태를 관리하는 클래스, State를 상속 받는다
    /// 적 디텍션 되면 걷기(추격) 상태 변경 
    /// -> 공격 범위에 들어오면 공격 상태 변경
    /// </summary>
    public class IdleState : State
    {
        #region Variables
        //참조
        private Animator m_Animator;

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
                }
                else
                {
                    stateMachine.ChangeState(typeof(WalkState));
                }
            }
        }
        #endregion
    }
}