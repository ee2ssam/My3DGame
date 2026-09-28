using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace My3DGame
{
    /// <summary>
    /// UI 슬라이더 오브젝트에 부착되어 슬라이더 값에 따라
    /// 지정된 오디오믹서의 볼륨을 조절한다
    /// </summary>
    public class MixerSliderLink : MonoBehaviour
    {
        #region Variables
        public AudioMixer audioMixer;
        public string mixerParameter;

        public float maxAttenuation = 0f;
        public float minAttenuation = -80f;

        private Slider slider;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            slider = GetComponent<Slider>();

            //현재 믹서의 볼륨값 가져와서 UI 적용
            float value;
            audioMixer.GetFloat(mixerParameter, out value);
            slider.value = (value - minAttenuation) / (maxAttenuation - minAttenuation);

            //UI 이벤트 함수 등록
            //slider.onValueChanged.AddListener(SliderValueChange);
        }
        #endregion

        #region Custom Method
        public void SliderValueChange(float value)
        {
            audioMixer.SetFloat(mixerParameter, minAttenuation + value * (maxAttenuation - minAttenuation));
        }
        #endregion
    }
}