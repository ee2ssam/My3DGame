using Unity.VisualScripting;
using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 탑다운 카메라의 Rotation을 제어하는 클래스
    /// </summary>
    public class TopDownCamera : MonoBehaviour
    {
        #region Variables
        public Transform cinemachineCameraTarget;       //카메라 타겟 오브젝트

        [SerializeField] private float angleX = 0f;     //카메라 회전값
        [SerializeField] private float angleY = 0f;     //카메라 회전값

        [SerializeField] private float rotateFrequency = 0.5f;           //Lerp 속도 계수값
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            HandleCamera();
        }

        private void LateUpdate()
        {
            HandleCamera();
        }
        #endregion

        #region Custom Method
        void HandleCamera()
        {
            //타겟이 바라보는 방향 + angleY
            Quaternion finalRotation = Quaternion.Euler(angleX,
                cinemachineCameraTarget.rotation.eulerAngles.y + angleY,
                transform.rotation.eulerAngles.z);

            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, rotateFrequency * Time.deltaTime);
        }
        #endregion
    }
}