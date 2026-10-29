using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 캐릭터 속성 값들을 관리하는 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterStatsObject", menuName = "CharacterStats")]
    public class CharacterStatsSO : ScriptableObject
    {
        #region Variables        
        public Attribute[] attributes;  //캐릭터 속성 값을 가진 배열


        #endregion
    }
}