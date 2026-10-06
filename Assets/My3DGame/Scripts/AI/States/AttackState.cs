using UnityEngine;
using UnityEngine.AI;

namespace My3DGame
{
    /// <summary>
    /// 공격 상태를 관리하는 클래스, State를 상속 받는다
    /// </summary>
    public class AttackState : State
    {
        #region Variables
        //참조
        private Animator m_Animator;

        private float m_AttackDelay = 0f;

        //애니메이션 파라미터
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashAttack = Animator.StringToHash("Attack");
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
            m_Animator.SetTrigger(m_HashAttack);

            //공격 딜레이 설정
            m_AttackDelay = 2f;
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void OnExit()
        {
            //다음 공격이 들어올때가지 지연시간 설정
            enemy.SetAttackDelay(m_AttackDelay);
        }
        #endregion
    }
}