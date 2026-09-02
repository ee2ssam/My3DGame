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
        public Transform cinemachineCameraTarget;  //씨네머신 트래킹 타겟 오브젝트

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
            m_CinemachineTargetYaw = cinemachineCameraTarget.rotation.eulerAngles.y;
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
            //입력값 처리
            if(m_Input.Look.sqrMagnitude >= _thershold && lockCameraPosition == false)
            {
                float deltaTimeMultiplier = 1.0f;
                m_CinemachineTargetYaw += m_Input.Look.x * deltaTimeMultiplier;
                m_CinemachineTargetPitch += m_Input.Look.y * deltaTimeMultiplier;
            }

            m_CinemachineTargetYaw = ClampAngle(m_CinemachineTargetYaw, float.MinValue, float.MaxValue);
            m_CinemachineTargetPitch = ClampAngle(m_CinemachineTargetPitch, bottomClamp, topClamp);

            cinemachineCameraTarget.rotation = Quaternion.Euler(m_CinemachineTargetPitch + cameraAngleOverride,
                m_CinemachineTargetYaw, 0f);
        }

        //각도 Clamp 기능
        private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;

            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        #endregion

    }
}