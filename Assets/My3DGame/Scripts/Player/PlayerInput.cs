using UnityEngine;
using System.Collections;

namespace My3DGame
{
    /// <summary>
    /// 플레이어 인풋을 관리하는 클래스
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        #region Variables
        //인풋 리더 스크립터블오브젝트
        public InputReader inputReader;

        //인풋 제어
        [HideInInspector]
        public bool playerControllerInputBlocked;

        //Move
        private Vector2 m_Movement;
        private bool m_Jump;

        //Attack
        private bool m_Attack;

        private Coroutine m_AttackWaitCoroutine;
        //최소한 한 프레임동안만 m_Attack true로 해준다
        private const float k_AttackInputDuration = 0.03f;

        //Look
        [SerializeField] private Vector2 m_Look;
        #endregion

        #region Property
        public Vector2 Movement
        {
            get
            {
                if (playerControllerInputBlocked)
                    return Vector2.zero;

                return m_Movement;
            }
            private set { m_Movement = value; }
        }

        public bool Jump
        {
            get
            {
                if (playerControllerInputBlocked)
                    return false;

                return m_Jump;
            }
            private set { m_Jump = value; }
        }

        public bool Attack
        {
            get
            {
                if (playerControllerInputBlocked)
                    return false;

                return m_Attack;
            }
            private set { m_Attack = value; }
        }

        public Vector2 Look
        {
            get
            {
                if (playerControllerInputBlocked)
                    return Vector2.zero;

                return m_Look;
            }
            private set { m_Look = value; }
        }
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //inputReader 이벤트 함수 등록
            inputReader.MoveEvent += OnMove;
            inputReader.JumpEvent += OnJumpStarted;
            inputReader.JumpCanceledEvent += OnJumpCanceled;
            inputReader.AttackEvent += OnAttack;

            inputReader.LookEvent += OnLook;
        }

        private void OnDisable()
        {
            //inputReader 이벤트 함수 제거
            inputReader.MoveEvent -= OnMove;
            inputReader.JumpEvent -= OnJumpStarted;
            inputReader.JumpCanceledEvent -= OnJumpCanceled;
            inputReader.AttackEvent -= OnAttack;

            inputReader.LookEvent -= OnLook;

        }

        private void Start()
        {
            //인풋 활성화
            inputReader.EnablePlayerInput();
        }
        #endregion

        #region Custom Method
        private void OnMove(Vector2 movement)
        {
            Movement = movement;
        }

        private void OnJumpStarted()
        {
            Jump = true;            
        }

        private void OnJumpCanceled()
        {
            Jump = false;
        }

        private void OnAttack()
        {
            if (m_AttackWaitCoroutine != null)
                StopCoroutine(m_AttackWaitCoroutine);   //지정된 코루틴 강제 종료
            //StopAllCoroutines();                      //모든 코루틴 강제 종료

            m_AttackWaitCoroutine = StartCoroutine(AttackWait());
        }

        IEnumerator AttackWait()
        {
            Attack = true;
            yield return new WaitForSeconds(k_AttackInputDuration);

            Attack = false;
        }

        private void OnLook(Vector2 look)
        {
            Look = look;
        }
        #endregion
    }
}