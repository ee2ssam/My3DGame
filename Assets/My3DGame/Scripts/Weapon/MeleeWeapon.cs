using System;
using UnityEngine;
using static My3DGame.MeleeWeapon;

namespace My3DGame
{
    /// <summary>
    /// 근접 전투 무기를 관리하는 클래스
    /// </summary>
    public class MeleeWeapon : MonoBehaviour
    {
        /// <summary>
        /// 무기의 공격 충돌 체크 포인트
        /// </summary>
        [Serializable]
        public struct AttackPoint
        {
            public float radius;            //충돌 체크 반경
            public Transform attackRoot;    //충돌 포인트 위치 기준 오브젝트
            public Vector3 offset;          //충돌 포인트 위치 조정값
        }

        #region Variables
        //충돌 체크 포인트 배열
        [SerializeField] protected AttackPoint[] attackPoints = new AttackPoint[0];

        //충돌 체크
        [SerializeField] protected LayerMask targetLayers;       //충돌 레이어 마스크

        [SerializeField] protected Vector3[] m_PreviousPos;
        protected Vector3 m_Direction;                          //hit 방향

        protected bool m_InAttack = false;
        protected bool m_IsThrowingHit = false;

        protected static RaycastHit[] s_RaycastHitCache = new RaycastHit[32];
        protected static Collider[] s_ColliderCache = new Collider[32];
        #endregion


        #region Unity Event Method
        private void FixedUpdate()
        {
            //충돌 체크 - 공격 중
            if(m_InAttack)
            {
                for (int i = 0; i < attackPoints.Length; i++)
                {
                    AttackPoint apt = attackPoints[i];

                    Vector3 worldPos = apt.attackRoot.position + apt.attackRoot.TransformVector(apt.offset);
                    Vector3 attackVector = worldPos - m_PreviousPos[i];

                    if(attackVector.magnitude < 0.001f)
                    {
                        attackVector = Vector3.forward * 0.001f;
                    }

                    //공격 방향으로 레이 쏘기
                    Ray r = new Ray(worldPos, attackVector.normalized);
                    int contacts = Physics.SphereCastNonAlloc(r, apt.radius, s_RaycastHitCache,
                        attackVector.magnitude, ~0, QueryTriggerInteraction.Ignore);

                    Debug.Log($"hit contacts : {contacts}");
                    for (int j = 0; j < contacts; j++)
                    {
                        Collider collider = s_RaycastHitCache[j].collider;
                        if(collider != null)
                        {
                            Debug.Log($"{collider.gameObject.name}에게 데미지 주기");
                            CheckDamage(collider, apt);
                        }
                    }
                }
            }
        }

        //충돌 체크 포인트 기즈모 그리기
        private void OnDrawGizmosSelected()
        {
            //흰색 반투명
            Gizmos.color = new Color(1f, 1f, 1f, 0.4f);
            foreach (var attackPoint in attackPoints)
            {
                if(attackPoint.attackRoot != null)
                {
                    Vector3 worldPostion = attackPoint.attackRoot.TransformVector(attackPoint.offset);
                    Gizmos.DrawSphere(attackPoint.attackRoot.position + worldPostion, attackPoint.radius);
                }
            }
        }
        #endregion

        #region Custom Method
        //hit한 콜라이더에게 데미지 주기
        private void CheckDamage(Collider other, AttackPoint apt)
        {
            //Damageable 체크
            Damageable d = other.GetComponent<Damageable>();

            if (d == null)
                return;

            d.TakeDamage(10, this.gameObject);
        }


        public void StartAttack(bool throwingAttack)
        {
            m_InAttack = true;
        }

        public void EndAttack()
        {
            m_InAttack = false;
        }
        #endregion

    }
}