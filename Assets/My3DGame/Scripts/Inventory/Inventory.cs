using UnityEngine;
using System;
using System.Linq;

namespace My3DGame
{
    /// <summary>
    /// 아이템 슬롯들을 관리하는 클래스
    /// 속성: 슬롯 목록
    /// 기능: 인벤토리 비우기, 아이템 존재 여부 체크
    /// </summary>
    [Serializable]
    public class Inventory
    {
        #region Variables
        public ItemSlot[] slots = new ItemSlot[16];
        #endregion

        #region Constructor
        #endregion

        #region Custom Method
        //인벤토리 비우기 : 모든 슬롯 비우기
        public void Clear()
        {
            foreach (var itemSlot in slots)
            {
                itemSlot.RemoveItem();
            }
        }

        //아이템 존재 여부 체크 - 아이템 아이디 체크
        public bool IsContain(int id)
        {
            return slots.FirstOrDefault(i => i.item.id == id) != null;
        }

        //아이템 존재 여부 체크 - 아이템 오브젝트 체크
        public bool IsContain(ItemObjectSO itemObject)
        {
            return IsContain(itemObject.data.id);
        }
        #endregion
    }
}