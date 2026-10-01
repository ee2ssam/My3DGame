using UnityEngine;

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
        protected StateMachine m_StateMachine;  //상태를 관리하는 상태머신
        #endregion

        #region Unity Event Method
        protected virtual void Awake()
        {
            
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
        #endregion


    }
}