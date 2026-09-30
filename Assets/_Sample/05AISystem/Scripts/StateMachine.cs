using UnityEngine;
using System.Collections.Generic;

namespace MySample
{
    /// <summary>
    /// 상태들을 등록 받아 관리하는 봉인 클래스
    /// 속성 : 상태들을 저장하는 변수(Dictionary), 상태머신 소유주, 현재 상태
    /// 기능 : 상태 등록, 현재 상태 업데이트, 상태 변경
    /// </summary>
    public sealed class StateMachine<T>
    {
        #region Variables
        private T context;    //이 상태머신을 가지고 있는 객체, 상태머신 소유주

        //상태들을 저장하는 변수(자료구조)
        private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

        private State<T> m_CurrentState;       //현재 상태
        private State<T> m_PreviousState;      //이전 상태
        private float m_ElapseTime = 0f;    //현재 상태가 진행된 누적 시간 카운팅
        #endregion

        #region Property
        public State<T> CurrentState => m_CurrentState;
        public State<T> PreviousState => m_PreviousState;
        public float ElapseTime => m_ElapseTime;    
        #endregion

        //생성자, 매개변수: 소유주(T), 초기 상태(State)
        public StateMachine(T _context, State<T> initalState)
        {
            this.context = _context;

            //초기 상태를 상태목록에 저장
            RegisterState(initalState);

            //현재상태 초기화
            m_CurrentState = initalState;
            m_CurrentState.OnEnter();       //상태 들어가기
            m_ElapseTime = 0f;

            Debug.Log($"{initalState}상태로 처음 시작");
        }

        //매개변수로 받은 상태를 상태 목록에 등록
        public void RegisterState(State<T> state)
        {
            //등록하는 상태 셋팅
            state.SetState(context, this);

            //상태 저장
            states[state.GetType()] = state;
        }

        //현재 상태(m_CurrentState) 업데이트
        public void Update(float deltaTime)
        {
            m_ElapseTime += deltaTime;
            m_CurrentState.OnUpdate(deltaTime);
        }

        //상태 변경
        // A -> B
        // A 나가기, B -> Current, B 들어가기
        public R ChangeState<R>() where R : State<T>
        {
            //현재 상태 체크
            var newType = typeof(R);
            if(m_CurrentState != null && newType == m_CurrentState.GetType())
            {
                //현재 상태와 새로운 상태가 동일
                return m_CurrentState as R;
            }

            //oldState(현재 상태)에서 나가기
            if(m_CurrentState != null)
            {
                m_CurrentState.OnExit();
            }

            //m_CurrentState를 새로운 상태로 변경
            m_PreviousState = m_CurrentState;
            m_CurrentState = states[newType];

            //newState(현재 상태)로 들어가기
            m_CurrentState.OnEnter();
            m_ElapseTime = 0f;

            return m_CurrentState as R;
        }
    }
}
