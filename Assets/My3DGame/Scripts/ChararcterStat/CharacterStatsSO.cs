using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

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
        [SerializeField] private UserData userData;      //유저 게임 데이터

        //초기화 체크
        private bool isInitialize = false;

        //속성 값 변경시 등록된 함수 호출
        public UnityAction<CharacterStatsSO> OnChangedStats;
        #endregion

        #region Property
        public int Level
        {
            get {  return userData.level; }
            private set { userData.level = value; }
        }

        public int Exp
        {
            get { return userData.exp; }
            private set { userData.exp = value; }
        }

        public int Gold
        {
            get { return userData.gold; }
            private set { userData.gold = value; }
        }

        public float Health
        {
            get { return userData.health; }
            private set { userData.health = value; }
        }

        public float Mana
        {
            get { return userData.mana; }
            private set { userData.mana = value; }
        }
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //캐릭터 속성 목록 초기화
            InitializeAttributes();
        }
        #endregion

        #region Custom Method
        //캐릭터 속성 목록 초기화
        private void InitializeAttributes()
        {
            //초기화 체크
            if (isInitialize)
                return;

            isInitialize = true;

            //속성의 ModifiableInt 객체 생성
            foreach (var attribute in attributes)
            {
                //attribute value 생성
                attribute.value = new ModifiableInt(OnMoidefiedValue);
            }

            //attribute value 값 셋팅
            SetBaseValue(CharacterAttribute.Agility, 100);
            SetBaseValue(CharacterAttribute.Intellect, 100);
            SetBaseValue(CharacterAttribute.Stamina, 100);
            SetBaseValue(CharacterAttribute.Strength, 100);
            SetBaseValue(CharacterAttribute.Health, 100);
            SetBaseValue(CharacterAttribute.Mana, 100);

            //유저 게임 데이터 초기화
            Level = 1;
            Exp = 0;
            Gold = 1000;    //초기 소지금
            Health = (float)GetModifiredValue(CharacterAttribute.Health);
            Mana = (float)GetModifiredValue(CharacterAttribute.Mana);
        }
        #endregion

        #region Custom Method
        //모든 attribute의 value값이 각각 변경될때 마다 호출되는 함수
        private void OnMoidefiedValue(ModifiableInt value)
        {            
            if(OnChangedStats != null)
            {
                OnChangedStats.Invoke(this);
            }
        }

        //속성 값 초기화
        private void SetBaseValue(CharacterAttribute type, int value)
        {
            foreach (var attribute in attributes)
            {
                if(attribute.type == type)
                {
                    attribute.value.BaseValue = value;
                }
            }
        }

        //속성 값 최종 연산 결과 가져오기
        public int GetModifiredValue(CharacterAttribute type)
        {
            foreach (var attribute in attributes)
            {
                if (attribute.type == type)
                {
                    return attribute.value.ModifiedValue;
                }
            }

            return -1;
        }
        #endregion

        #region userdata Method
        //레벨 - 레벨업

        //골드

        //체력, 마나
        #endregion
    }
}