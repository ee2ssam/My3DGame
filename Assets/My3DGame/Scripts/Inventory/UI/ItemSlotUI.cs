using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace My3DGame
{
    /// <summary>
    /// 아이템 슬롯 UI를 관리하는 클래스
    /// 속성 - UI: 아이콘이미지, 아이템 갯수 텍스트, 선택 이미지
    /// 기능 : 슬롯 UI 업데이트, 선택여부
    /// </summary>
    public class ItemSlotUI : MonoBehaviour
    {
        #region Variables
        public Image iconImage;
        public TextMeshProUGUI amountText;
        public GameObject selectImage;
        #endregion

        #region Custom Method
        //매개 변수 ItemSlot 받아 슬롯 UI 업데이트
        public void UpdateSlot(ItemSlot slot)
        {
            //슬롯 체크
            if (slot == null)
                return;

            //빈슬롯 체크
            if(slot.item.id < 0 || slot.item.name == null
                || slot.amount <= 0)
            {
                iconImage.sprite = null;
                iconImage.gameObject.SetActive(false);
                amountText.text = string.Empty;
            }
            else //아이템 있으면
            {
                iconImage.sprite = slot.ItemObject.icon;
                iconImage.gameObject.SetActive(true);
                amountText.text = slot.amount <= 1 ?
                    string.Empty : slot.amount.ToString();
            }
        }

        //슬롯 선택 여부
        public void SelectSlot(bool isSelect)
        {
            selectImage.SetActive(isSelect);
        }
        #endregion
    }
}