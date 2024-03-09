using System;
using UnityEngine;

namespace LazySquirrelLabs.AirHockey.Physics
{
    /// <summary>
    /// Propagates trigger event methods as delegate events
    /// </summary>
    internal class TriggerEvents2D : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// Invoked whenever the <see cref="OnTriggerEnter2D"/> method event is invoked.
        /// </summary>
        internal event Action<Collider2D> OnEnterTrigger;

        #endregion

        #region Event handlers

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnEnterTrigger?.Invoke(other);
        }

        #endregion
    }
}