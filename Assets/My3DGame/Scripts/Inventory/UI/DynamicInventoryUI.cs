using UnityEngine;
using System.Collections.Generic;

namespace My3DGame
{
    /// <summary>
    /// 가변적인 아이템 슬롯을 가지 인벤토리의 UI를 관리하는 클래스
    /// InventoryUI 상속 받는다
    /// </summary>
    public class DynamicInventoryUI : InventoryUI
    {
        #region Variables
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

                //slotUIs에 등록
                inventory.Slots[i].slotUI = go;
                slotUIs.Add(go, inventory.Slots[i]);
                go.name += i.ToString();
            }
        }
        #endregion
    }
}