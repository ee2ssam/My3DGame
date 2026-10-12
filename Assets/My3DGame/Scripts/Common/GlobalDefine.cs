using UnityEngine;

namespace My3DGame
{
    //게임 사용하는 Enum 정의

    /// <summary>
    /// 이펙트 종류 정의
    /// </summary>
    public enum EffectType
    {
        None = -1,
        NORMAL,
    }

    /// <summary>
    /// 사운드 종류 정의
    /// </summary>
    public enum SoundType
    {
        None = -1,
        MUSIC,          //배경음
        SFX,            //효과음
        VOICE,          //음성
    }

    /// <summary>
    /// 캐릭터 속성 정의
    /// </summary>
    public enum CharacterAttribute
    {
        Agility = 0,
        Intellect,
        Stamina,
        Strength,
        Health,
        Mana,
    }

    /// <summary>
    /// 아이템 종류 정의
    /// </summary>
    public enum ItemType
    {
        Helmet = 0,
        Cheat = 1,
        Pants = 2,
        Boots = 3,
        Pauldrons = 4,
        Gloves = 5,
        LeftWeapon = 6,
        RightWeapon = 7,
        Food,
        Default,
    }
}
