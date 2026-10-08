using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 패트롤이 가능한 Enemy를 관리하는 클래스, Enemy를 상속 받는다
    /// 기능 : Enemy 기능 + 패트롤
    /// </summary>
    public class EnemyPatrol : Enemy
    {
        #region Variables
        //웨이포인트 목록
        public Transform[] waypoints;
        #endregion

        #region Unity Event Method
        protected override void Start()
        {
            base.Start();

            //상속 받으면 추가로 새로운 상태를 등록한다
            m_StateMachine.RegisterState(new PatrolState());
        }
        #endregion
    }
}