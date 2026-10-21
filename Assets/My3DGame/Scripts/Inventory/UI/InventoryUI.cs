using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace My3DGame
{
    /// <summary>
    /// 인벤토리 UI를 관리하는 클래스들의 부모 (추상) 클래스
    /// 속성 : 인벤토리 오브젝트, UI에 있는 슬롯 오브젝트를 관리하는 목록(Dictionary)
    /// 필수(abstract) 기능 : 슬롯 오브젝트 생성(인벤토리에 있는 슬롯 숫자만큼)
    /// 기능 : 아이템 다른 인벤토리로 옮기기 :  드래그 앤 드랍 기능
    /// </summary>
    [RequireComponent(typeof(EventTrigger))]
    public abstract class InventoryUI : MonoBehaviour
    {
        //필수 기능: abstract method
        #region abstract method
        //인벤토리를 가져와서 슬롯 목록 생성
        public abstract void CreateSlots();
        #endregion

        #region Variables
        public InventorySO inventory;       //인벤토리 오브젝트

        public Dictionary<GameObject, ItemSlot> slotUIs
            = new Dictionary<GameObject, ItemSlot>();

        //슬롯 선택
        protected GameObject selectedSlotObject = null; //현재 선택된 슬롯 오브젝트
        #endregion

        #region Unity Event Method
        protected virtual void Awake()
        {
            //인벤토리를 가져와서 슬롯 목록 생성
            CreateSlots();

            //inventory오브젝트에 있는 슬롯(Slots) 셋팅
            if (inventory != null)
            {
                for (int i = 0; i < inventory.Slots.Length; i++)
                {
                    inventory.Slots[i].parent = inventory;
                    inventory.Slots[i].OnPostUpdate += OnPostUpdate;

                    //itemSlot 내용을 강제로 적용 - 슬롯 업데이트 실행
                    inventory.Slots[i].OnPostUpdate?.Invoke(inventory.Slots[i]);
                }
            }
        }
        #endregion

        #region Custom Method
        //슬롯의 내용이 변경후 호출되는 함수
        protected void OnPostUpdate(ItemSlot slot)
        {
            //slot 체크
            if (slot == null || slot.slotUI == null)
                return;

            ItemSlotUI itemSlotUI = slot.slotUI.GetComponent<ItemSlotUI>();
            if (itemSlotUI != null)
            {
                itemSlotUI.UpdateSlot(slot);
            }
        }
        #endregion
    }
}