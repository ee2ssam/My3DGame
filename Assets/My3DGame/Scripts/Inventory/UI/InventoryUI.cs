using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

        protected EventTrigger trigger;

        //슬롯 선택
        protected GameObject selectedSlotObject = null; //현재 선택된 슬롯 오브젝트
        #endregion

        #region Unity Event Method
        protected virtual void Awake()
        {
            //참조
            trigger = GetComponent<EventTrigger>();

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


            //이벤트 트리거 구성
            AddEvent(EventTriggerType.PointerEnter,
                delegate { OnEnterInterface(this.gameObject);});
            AddEvent(EventTriggerType.PointerExit,
                delegate { OnExitInterface(this.gameObject); });

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

        //이벤트 트리거 메소드
        #region EventTrigger Method
        //이벤트 함수 등록
        protected void AddEvent(EventTriggerType type, UnityAction<BaseEventData> action)
        {
            //trigger 체크
            if (trigger == null)
                return;

            //이벤트 엔트리 구성하여 이벤트 트리거에 등록
            EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        //인벤토리UI 오브젝트에 마우스가 들어오면 호출되는 함수
        public void OnEnterInterface(GameObject go)
        {
            Debug.Log($"OnEnterInterface Object: {go.name}");
            MouseData.inventoryUIMouseOver = go.GetComponent<InventoryUI>();
        }

        //인벤토리UI 오브젝트에서 마우스가 나가면 호출되는 함수
        public void OnExitInterface(GameObject go)
        {
            Debug.Log($"OnExitInterface Object: {go.name}");
            MouseData.inventoryUIMouseOver = null;
        }

        //슬롯UI 오브젝트에 마우스가 들어오면 호출되는 함수
        public void OnEnterSlot(GameObject go)
        {
            Debug.Log($"OnEnter Slot Object: {go.name}");
            MouseData.slotObjectMouseOver = go;
        }

        //슬롯UI 오브젝트에 마우스가 나가면 호출되는 함수
        public void OnExitSlot(GameObject go)
        {
            Debug.Log($"OnEnter Slot Object: {go.name}");
            MouseData.slotObjectMouseOver = null;
        }

        //슬롯UI 오브젝트를 가지고 마우스로 드래그를 시작할때 호출
        public void OnStartDrag(GameObject go)
        {
            Debug.Log($"OnStartDrag Slot Object: {go.name}");
            MouseData.tempItemBeginDragged = CreateDragItemObect(go);
        }

        //슬롯UI 오브젝트를 가지고 마우스로 드래그ing 호출
        public void OnDrag(GameObject go)
        {
            if (MouseData.tempItemBeginDragged == null)
                return;
            //드래그 오브젝트와 마우스 위치를 동기화
            MouseData.tempItemBeginDragged.GetComponent<RectTransform>().position
                = Mouse.current.position.ReadValue();
        }


        //슬롯UI 오브젝트를 가지고 마우스로 드래그를 끝낼때(드랍) 호출
        public void OnEndDrag(GameObject go)
        {
            Debug.Log($"OnEndDrag Slot Object: {go.name}");
            //임시 게임오브젝트 킬
            Destroy(MouseData.tempItemBeginDragged);
        }
        #endregion

        #region Custom Method
        //마우스 드래그시 포인터에 달고 다니는 아이템 오브젝트 생성
        protected GameObject CreateDragItemObect(GameObject go)
        {
            //아이템 슬롯의 아이템 체크
            if (slotUIs[go].item.id <= -1)
                return null;

            GameObject dragItem = new GameObject();
            RectTransform rectTransform = dragItem.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(50, 50);
            dragItem.transform.SetParent(this.transform.parent);    //부모 캔바스 지정
            Image iconImage = dragItem.AddComponent<Image>();
            iconImage.sprite = slotUIs[go].ItemObject.icon;
            iconImage.raycastTarget = false;
            dragItem.name = "Drag Item";

            return dragItem;
        }
        #endregion
    }
}