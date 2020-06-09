using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AirHockey.UI.Generic
{
    public class GenericToggle<T> : MonoBehaviour, IPointerClickHandler
    {
        #region Events

        public event Action<GenericToggle<T>> OnSelect; 

        #endregion
        
        #region Serialized Fields

        [SerializeField] private Text _text;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;
        [SerializeField] private T _value;

        #endregion

        #region Fields

        private bool _selected;

        #endregion

        #region Properties

        public T Value => _value;

        #endregion

        #region Event handlers

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_selected) return;

            Select();
            OnSelect?.Invoke(this);
        }

        #endregion

        #region Public

        public void Select()
        {
            _selected = true;
            _text.color = _selectedColor;
        }

        public void Deselect()
        {
            _selected = false;
            _text.color = _unselectedColor;
        }

        #endregion
    }
}