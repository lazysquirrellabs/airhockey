using UnityEngine;

namespace DefaultNamespace
{
    public class MoveInput : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private Rigidbody2D _rigidbody;

        #endregion

        #region Update

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.D))
                _rigidbody.AddForce(new Vector2(15,0));
        }

        #endregion
    }
}