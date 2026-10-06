using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 죽음을 관리하는 클래스, State를 상속 받는다
    /// </summary>
    public class DeathState : State
    {
        #region Variables
        //참조
        private Animator m_Animator;

        //애니메이션 파라미터
        readonly int m_HashIsDeath = Animator.StringToHash("IsDeath");
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
            m_Animator.SetBool(m_HashIsDeath, true);
        }

        public override void OnUpdate(float deltaTime)
        {

        }
        #endregion
    }
}