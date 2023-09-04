using System;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI.Generic
{
    /// <summary>
    /// A generic toggle group that can hold any value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TU"></typeparam>
    internal class GenericToggleGroup<T,TU> : ToggleGroup where T : GenericToggle<TU>
    {
        #region Events

        /// <summary>
        /// Event invoked whenever one of the values is selected.
        /// </summary>
        internal event Action<TU> OnSelect; 

        #endregion
        
        #region Serialized fields

        [SerializeField] private T[] _toggles;

        #endregion

        #region Fields

        private GenericToggle<TU> _selectedToggle;

        #endregion

        #region Setup

        protected override void Awake()
        {
            foreach (var toggle in _toggles)
            {
                toggle.ForceDeselection();
                toggle.OnSelect += HandleToggleSelection;
            }
            
            // Select the first toggle
            _toggles[0].ForceSelection();
        }
        
        protected override void OnDestroy()
        {
            foreach (var toggle in _toggles)
                toggle.OnSelect -= HandleToggleSelection;
        }

        #endregion

        #region Event handlers

        private void HandleToggleSelection(GenericToggle<TU> selectedToggle)
        {
            // Unselect the previously selected toggle, if there is one
            if (_selectedToggle != null)
                _selectedToggle.ForceDeselection();

            _selectedToggle = selectedToggle;

            OnSelect?.Invoke(_selectedToggle.Value);
        }

        #endregion
    }
}