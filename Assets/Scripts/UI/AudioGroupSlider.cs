using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AirHockey.UI
{
    /// <summary>
    /// A UI <see cref="Slider"/> that controls an <see cref="AudioMixerGroup"/> parameter.
    /// </summary>
    public class AudioGroupSlider : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private AudioMixerGroup _group;
        [SerializeField] private string _parameter;
        [SerializeField] private Slider _slider;

        #endregion

        #region Setup

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(HandleSliderChange);
        }

        #endregion

        #region Public

        /// <summary>
        /// Sets the <see cref="AudioGroupSlider"/> up. Loads audio levels from player prefs and sets them.
        /// </summary>
        public void Setup()
        {
            // If there is no saved prefs, load the default value from the mixer
            _group.audioMixer.GetFloat(_parameter, out var value);
            value = PlayerPrefs.GetFloat(_parameter, value);
            _group.audioMixer.SetFloat(_parameter, value);
            _slider.value = value;
            _slider.onValueChanged.AddListener(HandleSliderChange);
        }

        #endregion

        #region Event handlers

        private void HandleSliderChange(float value)
        {
            // Set the audio level
            _group.audioMixer.SetFloat(_parameter, value);
            // Store the audio level in the player preferences
            PlayerPrefs.SetFloat(_parameter, value);
        }

        #endregion
    }
}