using System;
using UnityEngine;

namespace Physics
{
    public class TriggerEvents2D : MonoBehaviour
    {
        #region Events

        public event Action<Collider2D> OnEnterTrigger;

        #endregion

        #region Event handlers

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnEnterTrigger?.Invoke(other);
        }

        #endregion
    }
}