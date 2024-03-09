using System;
using UnityEngine;

namespace LazySquirrelLabs.AirHockey.Managers
{
    /// <summary>
    /// Manages non-pointer (touchscreen on device, clicks on the editor) input.
    /// </summary>
    internal class InputManager : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// Invoked whenever the a return action (e.g. Android's return button) is invoked.
        /// </summary>
        internal event Action OnReturn;

        #endregion

        #region Update

        private void Update()
        {
#if UNITY_ANDROID
            // Android's return button is mapped to KeyCode.Escape
            if (Input.GetKeyDown(KeyCode.Escape))
                OnReturn?.Invoke();
#endif
        }

        #endregion
    }
}