using UnityEngine;

namespace AirHockey.Physics
{
    public class VelocityTransfer : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private Rigidbody2D _rigidbody;

        #endregion
        
        #region Event handlers

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _rigidbody.velocity = collision.relativeVelocity;
        }

        #endregion
    }
}