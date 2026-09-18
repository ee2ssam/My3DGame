using UnityEngine;
using System.Collections;

namespace My3DGame
{
    /// <summary>
    /// 무기 공격시 무기 궤적 이펙트 효과
    /// 공격시 이펙트 활성화 및 애니메이션 플레이, 라이트 활성화
    /// </summary>
    public class TimeEffect : MonoBehaviour
    {
        #region Variables
        public Light staffLight;    //무기 라이트

        private Animation m_Animation;
        #endregion

        private void Awake()
        {
            //참조
            m_Animation = GetComponent<Animation>();

            //이펙트 비활성화
            this.gameObject.SetActive(false);
        }

        //공격시 이펙트 활성화 및 애니메이션 플레이, 라이트 활성화
        public void Activate()
        {
            this.gameObject.SetActive(true);
            staffLight.enabled = true;

            if(m_Animation)
                m_Animation.Play();

            //무기 이펙트 효과 초기화
            StartCoroutine(DisableAtEndOfAnimation());
        }

        //무기 이펙트 효과 초기화
        IEnumerator DisableAtEndOfAnimation()
        {
            yield return new WaitForSeconds(m_Animation.clip.length);

            this.gameObject.SetActive(false);
            staffLight.enabled = false;
        }


    }
}
