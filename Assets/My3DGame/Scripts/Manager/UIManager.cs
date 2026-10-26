using UnityEngine;

namespace My3DGame
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        public InventorySO inventory;       //인벤토리 오브젝트

        #endregion

        #region Custom Method
        //인벤토리에 아이템 추가하기
        public void AddItemInnventory(int id)
        {
            inventory.AddItem(id);
        }
        #endregion
    }
}