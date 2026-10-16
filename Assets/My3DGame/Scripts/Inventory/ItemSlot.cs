using UnityEngine;
using System;

namespace My3DGame
{
    /// <summary>
    /// 인벤토리에 들어가는 아이템 슬롯을 관리하는 클래스
    /// 속성: 아이템, 아이템 갯수, 장착가능 타입
    /// 기능: 슬롯업데이트, 슬롯 비우기, 아이템 수량 연산, 슬롯에 장착 여부 체크
    /// </summary>
    [Serializable]
    public class ItemSlot
    {
        #region Variables
        public Item item;       //슬롯에 들어가는 아이템
        public int amount;      //슬롯에 들어간 아이템의 갯수

        //장착 가능 타입 : 장착 가능 타입이 지정이 안되면 모두 장착 가능
        public ItemType[] allowedItems = new ItemType[0];

        //슬롯 갱신하기전에 등록된 함수를 호출해서 실행
        public Action<ItemSlot> OnPreUpdate;
        //슬롯 갱신후에 등록된 함수를 호출해서 실행
        public Action<ItemSlot> OnPostUpdate;

        //
        public InventorySO parent;
        #endregion

        #region Property
        public ItemObjectSO ItemObject
        {
            get
            { 
                return item.id >= 0 ? parent.dataBase.itemObjects[item.id] : null;
            }
        }
        #endregion

        #region Constructor
        //빈 슬롯 만들기
        public ItemSlot()
        {
            UpdateSlot(new Item(), 0);
        }

        //매개변수로 들어온 아이템과 수량으로 슬롯 만들기
        public ItemSlot(Item _item, int _amount)
        {
            UpdateSlot(_item, _amount);
        }
        #endregion

        #region Custom Method
        //슬롯의 내용이 변경시 업데이트 호출
        public void UpdateSlot(Item _item, int _amount)
        {
            //아이템 수량 체크
            if(_amount <= 0)
            {
                //아이템 정보 삭제
                _item = new Item();
            }

            OnPreUpdate?.Invoke(this);
            this.item = _item;
            this.amount = _amount;
            OnPostUpdate?.Invoke(this);
        }

        //슬롯 비우기, 슬롯에서 아이템 제거하기
        public void RemoveItem()
        {
            UpdateSlot(new Item(), 0);
        }

        //슬롯의 아이템 갯수 연산
        public void AddAmount(int value)
        {
            UpdateSlot(this.item, this.amount += value);
        }

        //슬롯에 장착 가능 여부 체크
        public bool CanPlaceInSlot(ItemObjectSO itemObject)
        {
            //무조건 장착 가능 조건
            //장착 조건이 없을때, 장착할 아이템이 없을때
            if(allowedItems.Length <= 0 || itemObject == null
                || itemObject.data.id < 0)
            {
                return true;
            }

            //조건 체크
            foreach (var itemType in allowedItems)
            {
                if(itemObject.type == itemType)
                {
                    return true;
                }
            }

            //장착 불가
            return false;
        }
        #endregion
    }
}