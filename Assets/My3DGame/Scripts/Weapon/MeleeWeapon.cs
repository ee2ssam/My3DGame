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

        protected Vector3[] m_PreviousPos;
        protected Vector3 m_Direction;                          //hit 방향

        protected bool m_InAttack = false;
        protected bool m_IsThrowingHit = false;

        protected static RaycastHit[] s_RaycastHitCache = new RaycastHit[32];
        protected static Collider[] s_ColliderCache = new Collider[32];
        protected GameObject m_Owner;           //무기 주인
        [SerializeField] protected float m_AttackDamage = 10; //공격 데미지
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
                    
                    for (int j = 0; j < contacts; j++)
                    {
                        Collider collider = s_RaycastHitCache[j].collider;
                        if(collider != null)
                        {                            
                            CheckDamage(collider, apt);
                        }
                    }

                    m_PreviousPos[i] = worldPos;
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
        //무기 주인 셋팅
        public void SetOwner(GameObject owner)
        {
            m_Owner = owner;
        }

        //hit한 콜라이더에게 데미지 주기
        private void CheckDamage(Collider other, AttackPoint apt)
        {
            //Damageable 체크
            Damageable d = other.GetComponent<Damageable>();

            if (d == null)
                return;

            //셀프 데미지 체크
            if (d.gameObject == m_Owner)
                return;

            //레이어 마스크(bit 연산) 체크
            //if((targetLayers.value & (1 << other.gameObject.layer)) == 0)
            //    return;

            d.TakeDamage(m_AttackDamage, this.gameObject);
        }


        public void StartAttack(bool throwingAttack)
        {
            m_InAttack = true;
            m_IsThrowingHit = throwingAttack;

            m_PreviousPos = new Vector3[attackPoints.Length];
            for (int i = 0; i < attackPoints.Length; i++)
            {
                Vector3 worldPos = attackPoints[i].attackRoot.position
                    + attackPoints[i].attackRoot.TransformVector(attackPoints[i].offset);
                m_PreviousPos[i] = worldPos;
            }
        }

        public void EndAttack()
        {
            m_InAttack = false;
        }
        #endregion

    }
}