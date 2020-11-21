using AirHockey.UI;
using UnityEngine;

namespace AirHockey.Menu
{
    public class SettingsScreen : Displayable
    {
        #region Serialized fields

        [SerializeField] private AudioGroupSlider[] _sliders;

        #endregion

        #region Public

        public void LoadAudioLevels()
        {
            foreach (var slider in _sliders)
                slider.Setup();
        }

        #endregion
    }
}