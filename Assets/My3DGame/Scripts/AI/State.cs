using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 상태를 관리하는 클래스, 모든 상태의 부모 추상 클래스
    /// 속성: 상태머신, 상태머신 소유주
    /// 기능: 상태 셋팅, 초기화, 들어가기, 업데이트, 나가기
    /// </summary>
    public abstract class State
    {
        #region Variables
        protected Enemy enemy;                  //상태머신 소유주
        protected StateMachine stateMachine;    //현재 상태가 등록되어 있는 상태머신        
        #endregion

        //생성자
        public State() { }

        //상태 셋팅 : 상태 머신에 상태 등록시 호출
        //매개변수로 상태머신, 상태머신 소유주를 가져온다
        public void SetState(Enemy _enemy, StateMachine _stateMachine)
        {
            this.enemy = _enemy;
            this.stateMachine = _stateMachine;

            //상태 초기화
            OnInitialize();
        }

        public virtual void OnInitialize() { }  //상태 초기화, 1회 호출
        public virtual void OnEnter() { }   //상태 들어가기, 들어갈때마다 1회 호출 
        public abstract void OnUpdate(float deltaTime);    //상태 업데이트, 매 프레임마다 호출
        public virtual void OnExit() { }    //상태 나가기, 나갈때마다 1번 호출 
    }
}