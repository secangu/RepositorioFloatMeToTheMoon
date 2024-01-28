using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace FloatMeToTheMoon
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private SliderControl _masterSlider;
        [SerializeField] private SliderControl _melodySlider;
        [SerializeField] private SliderControl _effectsSlider;

        private void Start()
        {
            _masterSlider.Initialize("MasterVolume", PlayerPrefs.GetFloat("MasterVolume", 1.5f), ChangeMasterVolume);
            _melodySlider.Initialize("MelodyVolume", PlayerPrefs.GetFloat("MelodyVolume", 1.5f), ChangeMelodyVolume);
            _effectsSlider.Initialize("EffectsVolume", PlayerPrefs.GetFloat("EffectsVolume", 1.5f), ChangeEffectsVolume);
        }

        private void ChangeMasterVolume(float volume)
        {
            _audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }

        private void ChangeMelodyVolume(float volume)
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        }

        private void ChangeEffectsVolume(float volume)
        {
            _audioMixer.SetFloat("SFx Volume", Mathf.Log10(volume) * 20);
        }
    }

    [System.Serializable]
    public class SliderControl
    {
        public Slider slider;
        private string PlayerPrefsKey;
        private System.Action<float> onChange;

        public void Initialize(string key, float initialValue, System.Action<float> onChangeCallback)
        {
            PlayerPrefsKey = key;
            onChange = onChangeCallback;

            if (slider != null)
            {
                slider.minValue = 0.0001f;
                slider.maxValue = 3f;
                slider.value = initialValue;
                slider.onValueChanged.AddListener(OnSliderValueChanged);
            }

            // Configura los valores iniciales
            PlayerPrefs.SetFloat(PlayerPrefsKey, initialValue);
            onChange?.Invoke(initialValue);
        }

        private void OnSliderValueChanged(float value)
        {
            PlayerPrefs.SetFloat(PlayerPrefsKey, value);
            onChange?.Invoke(value);
        }
    }
}
