using UnityEngine;

namespace LazySquirrelLabs.AirHockey.Utils
{
    internal class TargetFrameRateSetter : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField, Range(1, 120)] private int _targetFrameRate;

        #endregion

        #region Setup

        private void Awake()
        {
            Application.targetFrameRate = _targetFrameRate;
        }

        #endregion
    }
}