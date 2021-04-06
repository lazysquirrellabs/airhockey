using System;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI.Generic
{
    public class GenericToggleGroup<T,TU> : ToggleGroup where T : GenericToggle<TU>
    {
        #region Events

        public event Action<TU> OnSelect; 

        #endregion
        
        #region Serialized fields

        [SerializeField] private T[] _toggles;

        #endregion

        #region Setup

        protected override void Awake()
        {
            foreach (var toggle in _toggles)
            {
                toggle.Deselect();
                toggle.OnSelect += SelectToggle;
            }
            _toggles[0].Select();
            OnSelect?.Invoke(_toggles[0].Value);
        }
        
        protected override void OnDestroy()
        {
            foreach (var toggle in _toggles)
                toggle.OnSelect -= SelectToggle;
        }

        #endregion

        #region Event handlers

        private void SelectToggle(GenericToggle<TU> selectedToggle)
        {
            foreach (var toggle in _toggles)
            {
                if (toggle != selectedToggle)
                    toggle.Deselect();
            }

            OnSelect?.Invoke(selectedToggle.Value);
        }

        #endregion
    }
}