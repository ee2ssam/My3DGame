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


            //이벤트 트리거 구성
            AddEvent(this.gameObject, EventTriggerType.PointerEnter,
                delegate { OnEnterInterface(this.gameObject);});
            AddEvent(this.gameObject, EventTriggerType.PointerExit,
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
        protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            //이벤트 트리거 가져오기
            EventTrigger trigger = go.GetComponent<EventTrigger>();

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
            Debug.Log($"OnExit Slot Object: {go.name}");
            MouseData.slotObjectMouseOver = null;
        }

        //슬롯UI 오브젝트를 가지고 마우스로 드래그를 시작할때 호출
        public void OnStartDrag(GameObject go)
        {
            Debug.Log($"OnStartDrag Slot Object: {go.name}");
            MouseData.tempItemBeginDragged = CreateDragItemObect(go);

            //현재 선택된 슬롯 디셀렉
            UpdateSelecSlot(null);
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

            //드랍 위치 체크해서 위치에 맞게 구현
            if(MouseData.inventoryUIMouseOver == null)
            {
                //마우스 위치가 인벤토리UI 오브젝트 밖에 있을 경우
                //아이템 버리기
            }
            else if(MouseData.slotObjectMouseOver != null)
            {
                //마우스의 위치가 슬롯UI 오브젝트 위에 있을 경우
                //마우스가 위치한 인벤토리UI에서 슬롯UI오브젝트에서
                //아이템 슬롯 가져오기
                ItemSlot mouseHoverSlot = MouseData.inventoryUIMouseOver
                    .slotUIs[MouseData.slotObjectMouseOver];
                //드랍한 아이템 슬롯과 아이템 교환
                inventory.SwapItems(slotUIs[go], mouseHoverSlot);
            }
        }

        //슬롯UI 오브젝트를 선택시 호출 (마우스 클릭)
        public void OnSelectSlot(GameObject go)
        {
            Debug.Log($"OnSelect Slot Object: {go.name}");


            //선택한 슬롯
            ItemSlot slot = slotUIs[go];
            //아이템 체크
            if(slot.item.id >= 0)
            {
                UpdateSelecSlot(go);
            }
            else
            {
                UpdateSelecSlot(null);
            }
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

        //슬롯 선택시 슬롯 업데이트
        public void UpdateSelecSlot(GameObject go)
        {
            //선택한 오브젝트 저장
            selectedSlotObject = go;

            //go 체크
            foreach (KeyValuePair<GameObject, ItemSlot> slot in slotUIs)
            {
                ItemSlotUI itemSlotUI = slot.Key.GetComponent<ItemSlotUI>();
                if (slot.Key == go)
                {
                    itemSlotUI.SelectSlot(true);
                }
                else
                {
                    itemSlotUI.SelectSlot(false);
                }
            }
        }

        //인벤토리 UI 창 닫기 
        public void CloseUI()
        {
            //TO DO : 창을 닫을때 해야 할일    

        }
        #endregion
    }
}