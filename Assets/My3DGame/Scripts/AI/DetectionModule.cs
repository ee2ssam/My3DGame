using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 적이 일정거리안에 들어왔는지 체크
    /// </summary>
    public class DetectionModule : MonoBehaviour
    {
        #region Variables                
        protected Transform m_Target;
        protected float m_DistanceToTarget; //타겟과의 거리

        public LayerMask targetMask;    //타겟팅할 적의 레이어

        [SerializeField]
        protected float detectionRange = 5f;    //디텍팅 범위
        [SerializeField]
        protected float detectionDelay = 0.1f;  //티텍팅 시간

        #endregion

        #region Property        
        public Transform Target => m_Target;
        public float DistanceToTarget => m_DistanceToTarget;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //0.1초마다 디텍팅
            InvokeRepeating("UpdateDetection", 0f, detectionDelay);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
        #endregion

        #region Custom Method
        private void UpdateDetection()
        {
            Transform nearestTarget = null;
            float minDistance = Mathf.Infinity;

            //가장 가까운 적 찾기
            Collider[] targets = Physics.OverlapSphere(transform.position, 
                detectionRange, targetMask);

            foreach (Collider target in targets)
            {
                float distance = Vector3.Distance(transform.position,
                    target.transform.position);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    nearestTarget = target.transform;
                }
            }

            //타겟 체크
            if(nearestTarget != null && minDistance <= detectionRange)
            {
                m_DistanceToTarget = minDistance;
                m_Target = nearestTarget;
            }
            else
            {
                m_DistanceToTarget = 0;
                m_Target = null;
            }
        }
        #endregion
    }
}