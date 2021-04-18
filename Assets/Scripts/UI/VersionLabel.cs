using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI
{
    /// <summary>
    /// Sets a Text label with the application version.
    /// </summary>
    public class VersionLabel : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private Text _label;

        #endregion

        #region Setup

        private void Awake()
        {
            _label.text = $"VERSION {Application.version}";
        }

        #endregion
    }
}