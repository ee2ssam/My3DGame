using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 타이머를 돌려서 랜덤값을 구한 다음 numberOfStates 중의 하나의 상태로 전이 시킨다
    /// </summary>
    public class RandomStateSMB : StateMachineBehaviour
    {
        #region Variables
        public int numberOfStates = 3;      //랜덤 상태 갯수
        public float minNormTime = 0f;      //랜덤 타이머 최소값
        public float maxNormTime = 5f;      //랜덤 타이머 최대값

        [SerializeField]
        protected float m_RandomNormTime = 0f;  //랜덤 타이머 시간(애니 플레이 횟수)

        // Parameters
        readonly int m_HashRandomIdle = Animator.StringToHash("RandomIdle");
        #endregion

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //초기값 설정 - 랜덤 타이머 시간 값 설정    
            m_RandomNormTime = Random.Range(minNormTime, maxNormTime);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //m_HashRandomIdle 값 -1 초기화
            if(animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash
                == stateInfo.fullPathHash)
            {
                animator.SetInteger(m_HashRandomIdle, -1);
            }

            //타이머 체크
            if (stateInfo.normalizedTime > m_RandomNormTime && !animator.IsInTransition(0))
            {
                int randNumber = Random.Range(0, numberOfStates);
                animator.SetInteger(m_HashRandomIdle, randNumber);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}