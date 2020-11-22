using AirHockey.UI;
using UnityEngine;
using Screen = AirHockey.UI.Screen;

namespace AirHockey.Menu
{
    public class SettingsScreen : Screen
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