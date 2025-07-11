using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

namespace My3DGame
{
    /// <summary>
    /// 플레이어와 관련된 인풋을 관리하는 클래스
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        #region Variables
        protected InputSystem_Actions inputActions;

        //input Action
        protected InputAction moveAction;

        //인풋 제어 처리
        [HideInInspector]
        public bool playerControllInputBlocked; //플레이어 상태에 따라 인풋 블록 처리
        protected bool m_ExternalInputBlocked;  //외부 요인에 따라 인풋 블록 처리

        //이동 wasd 인풋값
        protected Vector2 m_Movement;
        #endregion

        #region Property
        //이동
        public Vector2 Movement
        {
            get
            {
                if (playerControllInputBlocked || m_ExternalInputBlocked)
                    return Vector2.zero;

                return m_Movement;
            }
            private set
            {
                m_Movement = value;
            }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            inputActions = new InputSystem_Actions();
            moveAction = inputActions.Player.Move;
        }

        private void OnEnable()
        {
            //Action Map 활성화
            inputActions.Player.Enable();

            /*//액션 인풋 처리 샘플 - 이벤트 발생시 호출되는 함수 등록
            moveAction.performed += Move_Performed;
            moveAction.started += Move_Started;
            moveAction.canceled += Move_Cancled;*/
        }

        private void OnDisable()
        {
            //Action Map 비활성화
            inputActions.Player.Disable();

            /*//액션 인풋 처리 샘플 - 이벤트 발생시 호출되는 함수 해제
            moveAction.performed -= Move_Performed;
            moveAction.started -= Move_Started;
            moveAction.canceled -= Move_Cancled;*/
        }

        private void Update()
        {
            //이동 입력값 처리
            Movement = moveAction.ReadValue<Vector2>();
        }
        #endregion

        #region Custom Method
        //외부 요인에 따라 인풋 제어 블록 처리
        public void ReleasedControl()
        {
            m_ExternalInputBlocked = true;
        }

        public void GainControl()
        {
            m_ExternalInputBlocked = false;
        }
        //인풋 제어권 소유 여부
        public bool HaveControl()
        {
            return !m_ExternalInputBlocked;
        }



        /*//액션 인풋 처리 샘플
        private void Move_Performed(InputAction.CallbackContext context)
        {
            Debug.Log("Move_Performed");
        }

        private void Move_Started(InputAction.CallbackContext context)
        {
            Debug.Log("Move_Started");
        }

        private void Move_Cancled(InputAction.CallbackContext context)
        {
            Debug.Log("Move_Cancled");
        }*/
        #endregion
    }
}
