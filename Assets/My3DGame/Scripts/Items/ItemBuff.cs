using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 아이템 능력치를 관리하는 클래스
    /// 속성: 아이템속성, 능력치, 능력치 최소,최대값
    /// 기능: 능력치 더하기, 능력치 생성하기
    /// </summary>
    [System.Serializable]
    public class ItemBuff : IModifier
    {
        #region Variables
        public CharacterAttribute stat; //아이템 능력치 속성
        public int value;               //아이템 능력치

        [SerializeField] private int min;    //아이템 능력치의 최소값
        [SerializeField] private int max;    //아이템 능력치의 최대값
        #endregion

        #region Property
        public int Min => min;
        public int Max => max;
        #endregion

        #region Constructor
        public ItemBuff() { }

        public ItemBuff(int _min, int _max)
        {
            this.min = _min;
            this.max = _max;
            GenerateValue();
        }
        #endregion

        #region Custom Method
        //value 값 생성
        private void GenerateValue()
        {
            value = Random.Range(min, max);
        }

        //ItemBuff가 가지고 있는 능력치(value)를 참조 매개변수
        //값에 더해서 다시 결과를 보내준다
        public void AddValue(ref int baseValue)
        {
            baseValue += value;
        }
        #endregion
    }
}