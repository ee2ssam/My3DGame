using UnityEngine;

namespace My3DGame
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        public InventorySO inventory;       //인벤토리 오브젝트

        public InventoryUI inventoryUI;
        public InventoryUI equipmentUI;
        #endregion

        #region Custom Method
        //인벤토리에 아이템 추가하기
        public void AddItemInnventory(int id)
        {
            inventory.AddItem(id);
        }

        //인벤토리 UI 오픈
        public void OpenInventoryUI()
        {
            inventoryUI.gameObject.SetActive(true);
        }

        //인벤토리 UI 클로즈
        public void CloseInventoryUI()
        {
            inventoryUI.gameObject.SetActive(false);
            inventoryUI.CloseUI();
        }

        //장착 UI 오픈
        public void OpenEquipmentUI()
        {
            equipmentUI.gameObject.SetActive(true);
        }

        //장착 UI 클로즈
        public void CloseEquipmentUI()
        {
            equipmentUI.gameObject.SetActive(false);
            equipmentUI.CloseUI();
        }
        #endregion
    }
}