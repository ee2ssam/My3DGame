using UnityEngine;

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
        [SerializeField] private Vector2 m_Movement;
        [SerializeField] private bool m_Jump;
        #endregion

        #region Property
        public Vector2 Movement
        {
            get { return m_Movement; }
            private set { m_Movement = value; }
        }

        public bool Jump
        {
            get { return m_Jump; }
            private set { m_Jump = value; }
        }
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //inputReader 이벤트 함수 등록
            inputReader.MoveEvent += OnMove;
            inputReader.JumpEvent += OnJumpStarted;
            inputReader.JumpCanceledEvent += OnJumpCanceled;
        }

        private void OnDisable()
        {
            //inputReader 이벤트 함수 제거
            inputReader.MoveEvent -= OnMove;
            inputReader.JumpEvent -= OnJumpStarted;
            inputReader.JumpCanceledEvent -= OnJumpCanceled;

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
        #endregion
    }
}