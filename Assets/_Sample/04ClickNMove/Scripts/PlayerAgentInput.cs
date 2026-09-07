using My3DGame;
using Unity.VisualScripting;
using UnityEngine;

namespace MySample
{
    /// <summary>
    /// Agent 플레이어의 인풋을 관리하는 클래스
    /// </summary>
    public class PlayerAgentInput : MonoBehaviour
    {
        #region Variables
        //인풋 리더 스크립터블오브젝트
        public InputReader inputReader;

        //인풋 제어
        [HideInInspector]
        public bool playerControllerInputBlocked;

        //Move        
        private bool m_Click;
        #endregion

        #region Property
        public bool Click
        {
            get
            {
                if (playerControllerInputBlocked)
                    return false;

                return m_Click;
            }
            set { m_Click = value; }
        }
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //inputReader 이벤트 함수 등록
            inputReader.ClickEvent += OnClick;
        }

        private void OnDisable()
        {
            //inputReader 이벤트 함수 제거
            inputReader.ClickEvent -= OnClick;

        }

        private void Start()
        {
            //인풋 활성화
            inputReader.EnablePlayerAgentInput();
        }
        #endregion

        #region Custom Method
        private void OnClick()
        {
            Click = true;
        }
        #endregion
    }
}