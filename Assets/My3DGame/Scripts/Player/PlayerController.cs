using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 플레이어 액션을 관리하는 클래스
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Varialbels
        //참조
        protected PlayerInput m_Input;
        protected CharacterController m_CharCtrl;
        protected Animator m_Animator;

        //애니메이션 상태와 관련 변수
        protected AnimatorStateInfo m_CurrentStateInfo;
        protected AnimatorStateInfo m_NextStateInfo;
        protected bool m_IsAnimatorTransitioning;
        protected AnimatorStateInfo m_PriviousCurrentStateInfo;
        protected AnimatorStateInfo m_PriviousNextStateInfo;
        protected bool m_PriviousIsAnimatorTransitioning;

        //이동
        public float maxForwardSpeed = 8f;
        public float minTurnSpeed = 400f;
        public float maxTurnSpeed = 1200f;

        protected bool m_IsGrounded = true;
        protected float m_DesiredForwordSpeed;
        protected float m_ForwardSpeed;
        protected float m_VerticalSpeed;

        //회전
        protected Quaternion m_TargetRotaton;
        protected float m_AngleDiff;

        //애니메이션 Parameters Hash
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashVerticalSpeed = Animator.StringToHash("VerticalSpeed");
        readonly int m_HashAngleDeltaRad = Animator.StringToHash("AngleDeltaRad");
        readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");
        readonly int m_HashGrounded = Animator.StringToHash("Grounded");

        //애니메이션 상태 Hash


        //애니메이션 상태 tag Hash

        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            m_Input = GetComponent<PlayerInput>();
            m_CharCtrl = GetComponent<CharacterController>();
            m_Animator = GetComponent<Animator>();
        }
        #endregion
    }
}
