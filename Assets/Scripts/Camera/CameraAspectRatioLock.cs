using UnityEngine;

namespace AirHockey.Camera
{
    public class CameraAspectRatioLock : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private AspectRatio _maximumRatio;
        
        #endregion

        #region Setup

        private void Start()
        {
            var screenRatio = (float) Screen.width / Screen.height;
            if (screenRatio < _maximumRatio)
                _camera.orthographicSize = _maximumRatio / screenRatio * _camera.orthographicSize;
        }

        #endregion
    }
}