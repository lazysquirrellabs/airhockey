using System;
using LazySquirrelLabs.AirHockey.Physics;
using UnityEngine;

namespace LazySquirrelLabs.AirHockey.Match
{
    /// <summary>
    /// A goal (the area where a pucks has to go in to score) in the match.
    /// </summary>
    internal class Goal : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// Invoked whenever someone scored in this <see cref="Goal"/>.
        /// </summary>
        internal event Action OnScore;

        #endregion
        
        #region Serialized fields

        [SerializeField] private TriggerEvents2D _triggerEvents;

        #endregion

        #region Setup

        private void Awake()
        {
            _triggerEvents.OnEnterTrigger += Score;
        }
        
        private void OnDestroy()
        {
            _triggerEvents.OnEnterTrigger -= Score;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles the event of the puck colliding with this <see cref="Goal"/>'s trigger.
        /// </summary>
        /// <param name="_"></param>
        private void Score(Collider2D _)
        {
            OnScore?.Invoke();
        }

        #endregion
    }
}