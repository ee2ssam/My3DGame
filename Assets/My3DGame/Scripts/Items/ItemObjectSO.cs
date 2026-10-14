using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 아이템 데이터를 관리하는 스크립터블 오브젝트 클래스
    /// </summary>
    [CreateAssetMenu(fileName = "New ItemObject", menuName = "Items/Item")]
    public class ItemObjectSO : ScriptableObject
    {
        #region Variables
        public Item data = new Item();  //아이템 아이디, 이름, 능력치

        public ItemType type;           //아이템 종류        
        public bool isStackable;        //인벤토리 저장시 하나의 슬롯에 다수를 누적 저장 가능 여부
        public int shopPrice;           //상점 구매 금액
        public Sprite icon;             //아이템 아이콘 이미지
        public GameObject modelPrefab;  //장비 아이템 프리팹 오브젝트
        #endregion

        #region Custom Method
        //아이템 정보를 가져와서 아이템 만들기
        public Item CreateItem()
        {
            Item newItem = new Item(this);
            return newItem;
        }
        #endregion
    }
}