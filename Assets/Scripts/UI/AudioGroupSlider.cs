using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AirHockey.UI
{
    public class AudioGroupSlider : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private AudioMixerGroup _group;
        [SerializeField] private string _parameter;
        [SerializeField] private Slider _slider;

        #endregion

        #region Fields

        private string _volumeParameter;

        #endregion

        #region Public

        public void Setup()
        {
            _group.audioMixer.GetFloat(_parameter, out var volume);
            volume = PlayerPrefs.GetFloat(_parameter, volume);
            _group.audioMixer.SetFloat(_parameter, volume);
            _slider.value = volume;
            _slider.onValueChanged.AddListener(HandleSliderChange);
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(HandleSliderChange);
        }

        #endregion

        #region Event handlers

        private void HandleSliderChange(float value)
        {
            _group.audioMixer.SetFloat(_parameter, value);
            PlayerPrefs.SetFloat(_parameter, value);
        }

        #endregion
    }
}