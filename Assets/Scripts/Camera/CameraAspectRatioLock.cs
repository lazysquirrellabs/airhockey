using UnityEngine;

namespace AirHockey.Camera
{
    /// <summary>
    /// Locks a <see cref="Camera"/> to a maximum <see cref="AspectRatio"/> so the game screen scales properly on
    /// different aspect ratios.
    /// </summary>
    public class CameraAspectRatioLock : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private AspectRatio _minimumRatio;
        
        #endregion

        #region Setup

        private void Start()
        {
            var screenRatio =  new AspectRatio((uint) Screen.width, (uint) Screen.height) ;
            if (screenRatio < _minimumRatio)
                _camera.orthographicSize = _minimumRatio / screenRatio * _camera.orthographicSize;
        }

        #endregion
    }
}