using UnityEngine;
using System.Collections.Generic;

namespace My3DGame
{
    /// <summary>
    /// 상태들을 등록 받아 관리하는 봉인 클래스
    /// 속성 : 상태들을 저장하는 변수(Dictionary), 상태머신 소유주, 현재 상태
    /// 기능 : 상태 등록, 현재 상태 업데이트, 상태 변경
    /// </summary>
    public sealed class StateMachine
    {
        #region Variables
        private Enemy enemy;    //이 상태머신을 가지고 있는 객체, 상태머신 소유주

        //상태들을 저장하는 변수(자료구조)
        private Dictionary<System.Type, State> states = new Dictionary<System.Type, State>();

        private State m_CurrentState;       //현재 상태
        private State m_PreviousState;      //이전 상태
        private float m_ElapseTime = 0f;    //현재 상태가 진행된 누적 시간 카운팅
        #endregion

        #region Property
        public State CurrentState => m_CurrentState;
        public State PreviousState => m_PreviousState;
        public float ElapseTime => m_ElapseTime;    
        #endregion

        //생성자, 매개변수: 소유주(Enemy), 초기 상태(State)
        public StateMachine(Enemy _enemy, State initalState)
        {
            this.enemy = _enemy;

            //초기 상태를 상태목록에 저장
            RegisterState(initalState);

            //현재상태 초기화
            m_CurrentState = initalState;
            m_CurrentState.OnEnter();       //상태 들어가기
            m_ElapseTime = 0f;

            Debug.Log($"{initalState}상태로 처음 시작");
        }

        //매개변수로 받은 상태를 상태 목록에 등록
        public void RegisterState(State state)
        {

        }
    }
}
