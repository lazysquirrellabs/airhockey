using System;
using UnityEngine;

namespace AirHockey.Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Events

        public event Action OnReturn;

        #endregion

        #region Update

        private void Update()
        {
#if UNITY_ANDROID
            if (Input.GetKeyDown(KeyCode.Escape))
                OnReturn?.Invoke();
#endif
        }

        #endregion
    }
}