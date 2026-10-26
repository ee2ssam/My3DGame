using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace My3DGame
{
    /// <summary>
    /// 갯수와 자리가 고정된 아이템 슬롯을 가진 인벤토리의 UI를 관리하는 클래스
    /// InventoryUI 상속 받는다
    /// </summary>
    public class StaticInventoryUI : InventoryUI
    {
        #region Variables
        public GameObject[] staticSlots;

        #endregion

        #region abstract method
        //인벤토리를 가져와서 슬롯 목록 생성 - 게임 시작시
        public override void CreateSlots()
        {
            slotUIs = new Dictionary<GameObject, ItemSlot>();

            for (int i = 0; i < inventory.Slots.Length; i++)
            {
                GameObject go = staticSlots[i];

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
        //매개 변수로 들어온 아이템 슬롯을 장착
        public void Euip(ItemSlot itemSlot)
        {   
            inventory.EquipItem(itemSlot);
        }
        #endregion
    }
}