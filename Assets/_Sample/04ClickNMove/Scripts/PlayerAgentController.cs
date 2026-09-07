using UnityEngine;
using UnityEngine.AI;

namespace MySample
{
    /// <summary>
    /// Agent 플레이어를 관리하는 클래스
    /// </summary>
    public class PlayerAgentController : MonoBehaviour
    {
        #region Variables
        protected bool m_IsGrounded = true;            // Whether or not Ellen is currently standing on the ground.

        //참조
        protected PlayerAgentInput m_Input;                 // Reference used to determine how Ellen should move.
        protected CharacterController m_CharCtrl;      // Reference used to actually move Ellen.
        protected Animator m_Animator;                 // Reference used to make decisions based on Ellen's current animation and to set parameters.
        protected NavMeshAgent m_Agent;
        protected Camera m_MainCamera;

        //그라운드 레이어 체크
        [SerializeField] protected LayerMask groundLayerMask;

        // Parameters
        //readonly int m_Hash = Animator.StringToHash("");
        readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashGrounded = Animator.StringToHash("Grounded");
        #endregion

        #region Property
        //이동 입력값 체크
        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(m_Agent.velocity.magnitude, 0f); }
            //get { return false; }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            m_Input = GetComponent<PlayerAgentInput>();
            m_Animator = GetComponent<Animator>();
            m_CharCtrl = GetComponent<CharacterController>();
            m_Agent = GetComponent<NavMeshAgent>();

            m_MainCamera = Camera.main;
        }

        private void Start()
        {
            //초기화
            m_Agent.updatePosition = false; //Agent로 이동하지 않는다, 길찾기 계산은 한다
            m_Agent.updateRotation = true;  //Agent로 회전을 한다
        }

        private void FixedUpdate()
        {
            

            TimeoutToIdle();
        }

        private void OnAnimatorMove()
        {
            //캐릭터 위치를 보정
            Vector3 position = m_Agent.nextPosition; 
            m_Animator.rootPosition = position;
            transform.position = position;

            if(m_Agent.remainingDistance > m_Agent.stoppingDistance)
            {
                m_CharCtrl.Move(m_Agent.velocity * Time.deltaTime);
            }
            else
            {
                m_CharCtrl.Move(Vector3.zero);
            }
            m_Animator.SetFloat(m_HashForwardSpeed, m_Agent.velocity.magnitude);

            // After the movement store whether or not the character controller is grounded.
            //m_IsGrounded = m_CharCtrl.isGrounded;
            //m_Animator.SetBool(m_HashGrounded, m_IsGrounded);
        }
        #endregion

        #region Custom Method
        void TimeoutToIdle()
        {
            bool inputDetected = IsMoveInput;            

            m_Animator.SetBool(m_HashInputDetected, inputDetected);
        }
        #endregion
    }
}
