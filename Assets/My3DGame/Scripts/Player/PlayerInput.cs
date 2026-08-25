using My3DGame;
using UnityEngine;

namespace MySample2
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

        #region Custom Method
        #endregion
    }
}