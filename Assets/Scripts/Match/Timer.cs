using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Match
{
    /// <summary>
    /// A simple visual timer used in the match to show time (either left or elapsed) in minutes and seconds.
    /// </summary>
    public class Timer : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private Text _text;

        #endregion

        #region Public

        /// <summary>
        /// Sets the time displayed. Does not enable the timer if it's disabled. For that, use <see cref="Show"/>.
        /// </summary>
        /// <param name="seconds">How many seconds should be displayed on the timer. Minutes should be converted
        /// to seconds.</param>
        public void SetTime(uint seconds) => _text.text = $"{seconds / 60:00}:{seconds % 60:00}";

        /// <summary>
        /// Enables the timer and sets the time displayed.
        /// </summary>
        /// <param name="minutes">How many seconds should be displayed on the timer. Minutes should be converted
        /// to seconds.</param>
        public void Show(uint minutes)
        {
            SetTime(minutes * 60);
            gameObject.SetActive(true);
        }

        #endregion
    }
}