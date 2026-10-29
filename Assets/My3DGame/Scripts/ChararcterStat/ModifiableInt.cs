using UnityEngine;
using System;
using System.Collections.Generic;

namespace My3DGame
{
    /// <summary>
    /// 캐릭터 속성 값(value)을 관리하는 클래스
    /// </summary>
    [Serializable]
    public class ModifiableInt
    {
        #region Variables
        private int baseValue;      //속성 기본(초기) 값
        [SerializeField]
        private int modifiedValue;  //속성 최종(연산 완료) 값

        //modifiedValue가 값 변경시 등록된 함수 실행
        private event Action<ModifiableInt> _OnModifiedValue;

        //modifiedValue 값 계산시 추가되는 ItemBuff 값을 저장하는 리스트
        private List<IModifier> moidifers = new List<IModifier>();
        #endregion

        #region Constructor
        public ModifiableInt(Action<ModifiableInt> method = null)
        {
            //초기화
            modifiedValue = baseValue;
        }
        #endregion

        #region Custom Method

        #endregion
    }
}