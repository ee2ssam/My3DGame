using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 콤보 공격 애니메이션 상태가 시작되면 해당 공격의 스태프 이펙트를 플레이 시킨다
    /// </summary>
    public class EllenStaffEffectSMB : StateMachineBehaviour
    {
        #region Variables
        public int effectIndex;         //콤보 공격 인덱스에 맞는 이펙트 인덱스
        #endregion


        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 콤보 공격 애니메이션 상태가 시작되면 무기의 공격의 스태프 이펙트를 플레이
            PlayerController playerCtrl = animator.GetComponent<PlayerController>();
            playerCtrl.m_Weapon.timeEffects[effectIndex].Activate();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

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