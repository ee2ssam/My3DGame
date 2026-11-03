using UnityEngine;
using TMPro;

namespace My3DGame
{
    /// <summary>
    /// 플레이어 스탯 UI를 관리하는 클래스
    /// </summary>
    public class PlayerStatsUI : MonoBehaviour
    {
        #region Variables
        public CharacterStatsSO statsObejct;    //캐릭터 스탯 오브젝트

        public InventorySO equipment;

        //텍스트
        public TextMeshProUGUI agilityText;
        public TextMeshProUGUI intellectText;
        public TextMeshProUGUI staminaText;
        public TextMeshProUGUI strengthText;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            if(equipment != null)
            {
                foreach (var slot in equipment.Slots)
                {
                    slot.OnPreUpdate += OnUnEquipItem;
                    slot.OnPostUpdate += OnEquipItem;

                    //강제로 슬롯 업데이트 실행
                    slot.OnPostUpdate?.Invoke(slot);
                }
            }
        }

        private void OnEnable()
        {
            if (statsObejct != null)
            {
                statsObejct.OnChangedStats += UpdateStatsUI;
            }
            UpdateStatsUI();
        }

        private void OnDisable()
        {
            if (statsObejct != null)
            {
                statsObejct.OnChangedStats -= UpdateStatsUI;
            }
        }
        #endregion

        #region Custom Method
        private void UpdateStatsUI()
        {
            agilityText.text = statsObejct.GetModifiredValue(CharacterAttribute.Agility).ToString();
            intellectText.text = statsObejct.GetModifiredValue(CharacterAttribute.Intellect).ToString();
            staminaText.text = statsObejct.GetModifiredValue(CharacterAttribute.Stamina).ToString();
            strengthText.text = statsObejct.GetModifiredValue(CharacterAttribute.Strength).ToString();
        }

        //아이템 탈착시 stats 아이템 buff값 제거
        private void OnUnEquipItem(ItemSlot itemSlot)
        {
            //슬롯 체크
            if (itemSlot.ItemObject == null)
                return;

            //인벤토리 타입 체크
            if (itemSlot.parent.type == InventoryType.Equiptment)
            {
                foreach (var buff in itemSlot.item.buffs)
                {
                    foreach (var attribute in statsObejct.attributes)
                    {
                        if(buff.stat == attribute.type)
                        {
                            attribute.value.RemoveModifier(buff);
                        }
                    }
                }                
            }
        }

        //아이템 탈착시 stats 아이템 buff값 제거
        private void OnEquipItem(ItemSlot itemSlot)
        {
            //슬롯 체크
            if (itemSlot.ItemObject == null)
                return;

            //인벤토리 타입 체크
            if (itemSlot.parent.type == InventoryType.Equiptment)
            {
                foreach (var buff in itemSlot.item.buffs)
                {
                    foreach (var attribute in statsObejct.attributes)
                    {
                        if (buff.stat == attribute.type)
                        {
                            attribute.value.AddModifier(buff);
                        }
                    }
                }
            }
        }
        #endregion
    }
}