using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 스태프(무기)를 지정한 위치(왼손 어태치 포인트)에 부착
    /// </summary>
    public class FixedUpdateFollow : MonoBehaviour
    {
        public Transform toFollow;  //부착 위치

        private void FixedUpdate()
        {
            transform.position = toFollow.position;
            transform.rotation = toFollow.rotation;
        }
    }
}