using UnityEngine;
using UnityEngine.EventSystems;

namespace AirHockey.Movement
{
    public class InputMovementController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        #region Serialized fields

        [SerializeField] private Rigidbody2D _rigidBody;
        [SerializeField] private GameObject _prefabStart;
        [SerializeField] private GameObject _prefabEnd;

        #endregion

        #region Fields

        private bool _dragging;
        private Vector3 _lastDragWorldPos;
        private Vector2 _position;

        #endregion

        #region Update

        private void FixedUpdate()
        {
            if (!_dragging) return;
            
            _rigidBody.MovePosition(_position);
        }

        #endregion
        
        #region Event handlers
        
        public void OnDrag(PointerEventData eventData)
        {
            var thisDragPos = Camera.main.ScreenToWorldPoint(eventData.position);
            var thisDragPos2D = new Vector2(thisDragPos.x, thisDragPos.y);
            _position = thisDragPos2D;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _dragging = false;
        }
        
        #endregion
    }
}