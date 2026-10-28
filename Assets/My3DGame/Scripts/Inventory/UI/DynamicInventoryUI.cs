using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace My3DGame
{
    /// <summary>
    /// 가변적인 아이템 슬롯을 가지 인벤토리의 UI를 관리하는 클래스
    /// InventoryUI 상속 받는다
    /// </summary>
    public class DynamicInventoryUI : InventoryUI
    {
        #region Variables
        public InventorySO equipment;   //장착 인벤토리 오브젝트

        public GameObject slotPrefab;   //아이템슬롯 UI 프리팹 오브젝트
        public Transform slotsParent;   //생성되는 슬롯오브젝트의 부모 오브젝트
        #endregion

        #region abstract method
        //인벤토리를 가져와서 슬롯 목록 생성 - 게임 시작시
        public override void CreateSlots()
        {
            slotUIs = new Dictionary<GameObject, ItemSlot> ();

            for (int i = 0; i < inventory.Slots.Length; i++)
            {
                GameObject go = Instantiate(slotPrefab, Vector3.zero,
                    Quaternion.identity, slotsParent);

                //생성된 슬롯 오브젝트의 트리거에 이벤트 등록
                AddEvent(go, EventTriggerType.PointerEnter,
                    delegate { OnEnterSlot(go); });
                AddEvent(go, EventTriggerType.PointerExit,
                    delegate { OnExitSlot(go); });
                AddEvent(go, EventTriggerType.BeginDrag,
                    delegate { OnStartDrag(go); });
                AddEvent(go, EventTriggerType.Drag,
                    delegate { OnDrag(go); });
                AddEvent(go, EventTriggerType.EndDrag,
                    delegate { OnEndDrag(go); });
                AddEvent(go, EventTriggerType.PointerClick,
                    delegate { OnSelectSlot(go); });

                //slotUIs에 등록
                inventory.Slots[i].slotUI = go;
                slotUIs.Add(go, inventory.Slots[i]);
                go.name += i.ToString();
            }
        }
        #endregion

        #region Custom Method
        public override void CloseUI()
        {
            base.CloseUI();

            //다이나믹 UI 닫을때 기능 추가
        }

        //선택된 아이템 사용하기
        public void UseItem()
        {
            //선택 오브젝트 체크
            if (selectedSlotObject == null)
                return;

            if(inventory.UseItem(slotUIs[selectedSlotObject]))
            {
                //TO DO: 아이템 효과 구현
                Debug.Log($"{slotUIs[selectedSlotObject].item.name} 아이템 효과 구현");
            }

            //현재 선택된 슬롯 해제
            UpdateSelecSlot(null);
        }

        //선택된 아이템 장착하기
        public void EquipItem()
        {
            //선택 오브젝트 체크
            if (selectedSlotObject == null)
                return;

            equipment.EquipItem(slotUIs[selectedSlotObject]);

            //현재 선택된 슬롯 해제
            UpdateSelecSlot(null);

        }

        //선택된 아이템 판매하기
        public void SellItem()
        {
            //선택 오브젝트 체크
            if (selectedSlotObject == null)
                return;

            if(inventory.SellItem(slotUIs[selectedSlotObject]))
            {
                //아이템 판매 대금을 받는다.. 상점 가격의 절반
                int price = (int)(slotUIs[selectedSlotObject].ItemObject.shopPrice / 2);
                Debug.Log($"{price}에 판매하였습니다");
            }

            //현재 선택된 슬롯 해제
            UpdateSelecSlot(null);
        }
        #endregion
    }
}