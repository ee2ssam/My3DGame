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

        #region Property
        public int BaseValue
        {
            get
            {
                return baseValue;
            }
            set
            {
                baseValue = value;
                UpdateModifiedValue();
            }
        }

        public int ModifiedValue => modifiedValue;
        #endregion

        #region Constructor
        //생성자 - 값 변경시 호출되는 함수를 매개변수로 받아 이벤트 함수에 등록
        public ModifiableInt(Action<ModifiableInt> method = null)
        {
            //초기화
            modifiedValue = baseValue;
            RegisterModifireEvent(method);
        }
        #endregion

        #region Custom Method
        //이벤트 함수에 등록
        private void RegisterModifireEvent(Action<ModifiableInt> method)
        {
            if(method == null)
                return;

            _OnModifiedValue += method;
        }

        //이벤트 함수에서 제거
        private void RemoveModifireEvent(Action<ModifiableInt> method)
        {
            if (method == null)
                return;

            _OnModifiedValue -= method;
        }

        //속성 최종(연산 완료) 값 구하기
        private void UpdateModifiedValue()
        {
            int addValue = 0;
            foreach(var moidifer in moidifers)
            {
                moidifer.AddValue(ref addValue);
            }

            //최종값
            modifiedValue = baseValue + addValue;

            //최종값 구할때마다 등록된 함수 호출
            _OnModifiedValue?.Invoke(this);
        }

        //속성 값 추가
        public void AddModifier(IModifier modifier)
        {
            moidifers.Add(modifier);
            UpdateModifiedValue();
        }

        //속성 값 제거
        public void RemoveModifier(IModifier modifier)
        {
            moidifers.Remove(modifier);
            UpdateModifiedValue();
        }
        #endregion
    }
}