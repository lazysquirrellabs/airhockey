using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Match
{
    public class Timer : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private Text _text;

        #endregion

        #region Public

        public void SetTime(uint seconds) => _text.text = $"{seconds / 60:00}:{seconds % 60:00}";

        public void Show(uint minutes)
        {
            SetTime(minutes * 60);
            gameObject.SetActive(true);
        }

        #endregion
    }
}