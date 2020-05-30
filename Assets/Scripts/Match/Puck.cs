using UnityEngine;

namespace AirHockey.Match
{
    public class Puck : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private Rigidbody2D _rigidbody2D;

        #endregion

        #region Public

        public void Regroup(Vector2 position)
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.position = position;
            _rigidbody2D.rotation = 0f;
        }

        #endregion
    }
}