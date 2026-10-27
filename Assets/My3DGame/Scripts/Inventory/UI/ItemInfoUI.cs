using UnityEngine;
using TMPro;

namespace My3DGame
{
    /// <summary>
    /// 인벤토리 UI에서 선택된 아이템 정보 보여주기
    /// </summary>
    public class ItemInfoUI : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI itemNameText;

        public GameObject useButton;
        public GameObject equipButton;
        public GameObject sellButton;
        #endregion

        #region Custom Method
        //선택된 아이템 정보 보여주기
        public void SetItemInfoUI(ItemSlot itemSlot)
        {
            //Debug.Log($"{itemSlot.ItemObject.name}을 선택하였습니다");
            itemNameText.text = itemSlot.ItemObject.name;

            //버튼 셋팅 - 아이템 종류에 따라 버튼 설정

        }
        #endregion
    }
}