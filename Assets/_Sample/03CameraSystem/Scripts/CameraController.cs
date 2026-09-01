using UnityEngine;
using My3DGame;

namespace MySample
{
    /// <summary>
    /// 3인칭 시점의 카메라 위치 제어
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        #region Variables
        protected PlayerInput m_Input;

        [Header("Cinemachine")]
        public GameObject cinemachineCameraTarget;  //씨네머신 트래킹 타겟 오브젝트

        [SerializeField] private float topClamp = 70.0f;
        [SerializeField] private float bottomClamp = -30.0f;
        [SerializeField] private float cameraAngleOverride = 0.0f;
        [SerializeField] private bool lockCameraPosition = false;   //카메라 고정 여부

        private float m_CinemachineTargetYaw;
        private float m_CinemachineTargetPitch;
        private const float _thershold = 0.01f;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            m_Input = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            //초기화
            m_CinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
        }

        private void LateUpdate()
        {
            CameraRotation();
        }
        #endregion

        #region Custom Method
        //카메라 위치 제어
        void CameraRotation()
        {

        }
        #endregion

    }
}