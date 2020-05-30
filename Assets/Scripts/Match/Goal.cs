using System;
using Physics;
using UnityEngine;

namespace AirHockey.Match
{
    public class Goal : MonoBehaviour
    {
        #region Events

        public event Action OnScore;

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

        private void Score(Collider2D _)
        {
            OnScore?.Invoke();
        }

        #endregion
    }
}