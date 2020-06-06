using System;
using UnityEngine;

namespace AirHockey.Camera
{
    [Serializable]
    public struct AspectRatio
    {
        #region Serialized fields

        [SerializeField] private uint _height;
        [SerializeField] private uint _width;

        #endregion

        #region Properties

        public static implicit operator float(AspectRatio ratio) => (float) ratio._height / ratio._width;

        #endregion
    }
}