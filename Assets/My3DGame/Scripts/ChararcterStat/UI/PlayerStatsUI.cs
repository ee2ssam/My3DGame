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

        //텍스트
        public TextMeshProUGUI agilityText;
        public TextMeshProUGUI intellectText;
        public TextMeshProUGUI staminaText;
        public TextMeshProUGUI strengthText;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            if (statsObejct != null)
            {
                statsObejct.OnChangedStats += UpdateStatsUI;
            }
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
        #endregion
    }
}