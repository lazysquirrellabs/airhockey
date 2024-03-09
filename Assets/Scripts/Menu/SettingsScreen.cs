using LazySquirrelLabs.AirHockey.UI;
using UnityEngine;
using Screen = LazySquirrelLabs.AirHockey.UI.Screen;

namespace LazySquirrelLabs.AirHockey.Menu
{
    /// <summary>
    /// The settings/options screen in the main menu.
    /// </summary>
    internal class SettingsScreen : Screen
    {
        #region Serialized fields

        [SerializeField] private AudioGroupSlider[] _sliders;

        #endregion

        #region Internal

        /// <summary>
        /// Loads the audio levels for all audio sliders.
        /// </summary>
        internal void LoadAudioLevels()
        {
            foreach (var slider in _sliders)
                slider.Setup();
        }

        #endregion
    }
}