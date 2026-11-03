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
        public UnityAction OnChangedStats;
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

        public float MaxHealth => GetModifiredValue(CharacterAttribute.Health);
        public float MaxMana => GetModifiredValue(CharacterAttribute.Mana);
        public float HealthRatio => Health / MaxHealth;
        public float ManaRatio => Mana / MaxMana;
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
            //if (isInitialize)
            //    return;

            isInitialize = true;

            //속성의 ModifiableInt 객체 생성
            foreach (var attribute in attributes)
            {
                //attribute value 생성
                attribute.value = new ModifiableInt(OnMoidefiedValue);
            }

            //attribute value 값 셋팅
            SetBaseValue(CharacterAttribute.Agility, 10);
            SetBaseValue(CharacterAttribute.Intellect, 10);
            SetBaseValue(CharacterAttribute.Stamina, 10);
            SetBaseValue(CharacterAttribute.Strength, 10);
            SetBaseValue(CharacterAttribute.Health, 100);
            SetBaseValue(CharacterAttribute.Mana, 100);

            //유저 게임 데이터 초기화
            Level = 1;
            Exp = 0;
            Gold = 1000;    //초기 소지금
            Health = (float)GetModifiredValue(CharacterAttribute.Health);
            Mana = (float)GetModifiredValue(CharacterAttribute.Mana);
        }
        
        //모든 attribute의 value값이 각각 변경될때 마다 호출되는 함수
        private void OnMoidefiedValue(ModifiableInt value)
        {            
            if(OnChangedStats != null)
            {
                OnChangedStats.Invoke();
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
        public bool AddExp(int amount)
        {
            bool isLevlup = false;

            //경험치 누적
            Exp += amount;
                        
            //레벨업 체크, 반복
            while(Exp >= GetExpForLevelup(Level))
            {
                Exp -= GetExpForLevelup(Level);
                Level++;

                //레벨업 보상
                //...

                isLevlup = true;
            }

            //속성값 변경시 등록된 함수 호출
            if (OnChangedStats != null)
            {
                OnChangedStats.Invoke();
            }

            return isLevlup;
        }

        //(nowLevel)에서 레벨업에 필요한 경험치 가져오기
        public int GetExpForLevelup(int nowLevel)
        {
            //경험치 공식, 경험치 테이블...
            return nowLevel * 100;
        }

        //골드
        public void AddGold(int amount)
        {
            //overflow check(21억)

            Gold += amount;

            //속성값 변경시 등록된 함수 호출
            if (OnChangedStats != null)
            {
                OnChangedStats.Invoke();
            }
        }

        public bool UseGold(int amount)
        {
            if (Gold < amount)
                return false;

            Gold -= amount;

            //속성값 변경시 등록된 함수 호출
            if (OnChangedStats != null)
            {
                OnChangedStats.Invoke();
            }

            return true;
        }

        public bool EnoughGold(int amount)
        {
            if (Gold < amount)
                return false;
            
            return true;
        }

        //체력, 마나
        public void AddMana(int amount)
        {
            Mana += amount;
            
            if(Mana > MaxMana)
            {
                Mana = MaxMana;
            }

            //속성값 변경시 등록된 함수 호출
            if (OnChangedStats != null)
            {
                OnChangedStats.Invoke();
            }
        }

        public bool UseMana(int amount)
        {
            if (Mana < amount)
                return false;

            Mana -= amount;

            //속성값 변경시 등록된 함수 호출
            if (OnChangedStats != null)
            {
                OnChangedStats.Invoke();
            }

            return true;
        }

        public bool EnoughMana(int amount)
        {
            if (Mana < amount)
                return false;

            return true;
        }

        public void AddHealth(int amount)
        {
            Health += amount;

            if (Mana > MaxHealth)
            {
                Mana = MaxHealth;
            }

            //속성값 변경시 등록된 함수 호출
            if (OnChangedStats != null)
            {
                OnChangedStats.Invoke();
            }
        }

        public void UseHealth(int amount)
        {   
            Health -= amount;

            if(Health < 0f)
                Health = 0f;

            //속성값 변경시 등록된 함수 호출
            if (OnChangedStats != null)
            {
                OnChangedStats.Invoke();
            }
        }
        #endregion
    }
}