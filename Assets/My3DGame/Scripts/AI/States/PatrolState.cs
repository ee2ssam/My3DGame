using UnityEngine;
using UnityEngine.AI;

namespace My3DGame
{
    /// <summary>
    /// 적의 패트롤을 관리하는 클래스, State 상속 받는다
    /// </summary>
    public class PatrolState : State
    {
        #region Variables
        //참조
        private Animator m_Animator;
        private NavMeshAgent m_Agent;

        //패트롤
        private EnemyPatrol m_EnemyPatrol;

        private Transform m_TargetWaypoint; //목표 웨이포인트 오브젝트
        private int m_WayPointIndex = 0;    //웨이포인트 인덱스

        //애니메이션 파라미터
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        #endregion

        #region Property
        private Transform[] Waypoints => m_EnemyPatrol?.waypoints;
        #endregion

        #region Custom Method
        public override void OnInitialize()
        {
            //참조
            m_Animator = enemy.GetComponent<Animator>();
            m_Agent = enemy.GetComponent<NavMeshAgent>();

            //부모 객체로 부터 자식 객체 가져오기
            if (enemy is EnemyPatrol)
            {
                m_EnemyPatrol = enemy as EnemyPatrol;
            }   
        }

        public override void OnEnter()
        {
            //초기화
            m_Agent.stoppingDistance = 0.2f;

            if(m_TargetWaypoint == null)
            {
                FindNextWayPoint();
            }

            if (m_TargetWaypoint != null)
            {
                //목표 지점 설정
                m_Agent.SetDestination(m_TargetWaypoint.position);
            }
            else //타겟이 없으면
            {
                enemy.ChangeState(typeof(IdleState));
            }   
        }

        public override void OnUpdate(float deltaTime)
        {
            if (enemy.Target)
            {
                if (enemy.IsAttackable)
                {
                    stateMachine.ChangeState(typeof(AttackState));
                }
                else
                {
                    stateMachine.ChangeState(typeof(WalkState));
                }
            }
            else //적이 감지가 안되면 계속 패트롤
            {
                //도착 판정
                if(m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                {
                    FindNextWayPoint();
                    //대기 상태로 갔다가 0~2초 대기후 다시 패트롤
                    stateMachine.ChangeState(typeof(WalkState));
                }
                else
                {
                    //애니메이션
                    float speed = m_Agent.velocity.magnitude;
                    m_Animator.SetFloat(m_HashForwardSpeed, speed);
                }
            }
        }

        public override void OnExit()
        {
            //m_Agent 길찾기 기능 초기화
            m_Agent.ResetPath();
        }

        //다음 웨이 포인틀 찾는다
        private void FindNextWayPoint()
        {
            m_TargetWaypoint = null;

            if(Waypoints != null && Waypoints.Length > 1)
            {
                m_WayPointIndex = (m_WayPointIndex + 1) % Waypoints.Length;
                m_TargetWaypoint = Waypoints[m_WayPointIndex];
            }
        }
        #endregion
    }
}