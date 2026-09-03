using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// GameInput_Actions의 인풋 값을 읽는 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
    public class InputReader : ScriptableObject, GameInput_Actions.IPlayerActions, GameInput_Actions.IUIActions, GameInput_Actions.ICMActions
    {
        #region Variables
        protected GameInput_Actions _input;

        //Player Action 입력시 실행되는 이벤트 함수
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction JumpEvent = delegate { };
        public event UnityAction JumpCanceledEvent = delegate { };

        //CM Action 입력시 실행되는 이벤트 함수
        public event UnityAction<Vector2> LookEvent = delegate { };
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            if(_input == null)
            {
                _input = new GameInput_Actions();
                _input.Player.SetCallbacks(this);
                _input.CM.SetCallbacks(this);
                _input.UI.SetCallbacks(this);
            }

            EnablePayerInput();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }
        #endregion

        #region Input Action Controll
        //모든 인풋 비활성화, 인풋 초기화
        public void DisableAllInput()
        {
            _input.Player.Disable();
            _input.CM.Disable();
            _input.UI.Disable();
        }

        //플레이어 인풋 활성화
        public void EnablePayerInput()
        {
            DisableAllInput();

            _input.Player.Enable();
            _input.CM.Enable();
        }

        //UI 인풋 활성화
        public void EnableUIInput()
        {
            DisableAllInput();

            _input.UI.Enable();
        }
        #endregion

        #region Action Map - Player
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                JumpEvent.Invoke();
            }
            if (context.phase == InputActionPhase.Canceled)
            {
                JumpCanceledEvent.Invoke();
            }
        }
        #endregion

        #region Action Map - UI
        public void OnSubmit(InputAction.CallbackContext context)
        {
            
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            
        }
        #endregion

        #region Action Map - CM
        public void OnLook(InputAction.CallbackContext context)
        {
            LookEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            
        }

        public void OnFreeLook(InputAction.CallbackContext context)
        {
            
        }
        #endregion
    }
}