using System;
using UnityEngine;

namespace AirHockey.Camera
{
    [Serializable]
    public struct AspectRatio
    {
        #region Serialized fields

        [SerializeField] private int _height;
        [SerializeField] private int _width;

        #endregion

        #region Properties

        public float Value => (float) _height / _width;

        #endregion
    }
}