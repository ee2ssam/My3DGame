using System;

namespace My3DGame
{
    /// <summary>
    /// 캐릭터의 속성과 값 을 가진 클래스
    /// </summary>
    [Serializable]
    public class Attribute
    {
        public CharacterAttribute type; //캐릭터 속성 타입
        public ModifiableInt value;               //캐릭터 속성 값

        //생성자
        public Attribute(CharacterAttribute _type)
        {
            this.type = _type;
        }
    }
}