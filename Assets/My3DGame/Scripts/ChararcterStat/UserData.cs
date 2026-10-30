using System;

namespace My3DGame
{
    /// <summary>
    /// 게임에서 생산되는 유저 게임 데이터 정의
    /// </summary>
    [Serializable]
    public class UserData
    {
        public int level;
        public int exp;
        public int gold;
        public float health;
        public float mana;
    }
}