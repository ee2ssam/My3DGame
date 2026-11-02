using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace My3DGame
{
    /// <summary>
    /// 플레이 화면의 플레이어 데이터 UI를 관리하는 클래스
    /// </summary>
    public class PlayerUserDataUI : MonoBehaviour
    {
        #region Variables
        public CharacterStatsSO statsObejct;    //캐릭터 스탯 오브젝트

        //게이지 UI
        public Image healthBar;
        public Image manaBar;

        //텍스트
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI expText;
        public TextMeshProUGUI goldText;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            if(statsObejct != null)
            {
                statsObejct.OnChangedStats += UpdateUserDataUI;
            }
        }

        private void OnDisable()
        {
            if (statsObejct != null)
            {
                statsObejct.OnChangedStats -= UpdateUserDataUI;
            }
        }
        #endregion

        #region Custom Method
        private void UpdateUserDataUI()
        {
            healthBar.fillAmount = statsObejct.HealthRatio;
            manaBar.fillAmount = statsObejct.ManaRatio;

            levelText.text = statsObejct.Level.ToString();
            int needExp = statsObejct.GetExpForLevelup(statsObejct.Level);
            expText.text = statsObejct.Exp.ToString() + " / " + needExp.ToString();
            goldText.text = statsObejct.Gold.ToString();
        }
        #endregion
    }
}