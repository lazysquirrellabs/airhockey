using System;
using UnityEngine;

namespace AirHockey.Camera
{
    /// <summary>
    /// A screen/camera aspect ratio represented by its width and height.
    /// </summary>
    [Serializable]
    public struct AspectRatio
    {
        #region Serialized fields

        [SerializeField] private uint _height;
        [SerializeField] private uint _width;

        #endregion

        #region Setup

        /// <summary>
        /// <see cref="AspectRatio"/> constructor.
        /// </summary>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        public AspectRatio(uint width, uint height)
        {
            _width = width;
            _height = height;
        }

        #endregion
        
        #region Public

        public static implicit operator float(AspectRatio ratio) => (float) ratio._width / ratio._height;

        public static bool operator <(AspectRatio aspectRatio1, AspectRatio aspectRatio2)
        {
            float ratio1 = aspectRatio1;
            float ratio2 = aspectRatio2;
            return ratio1 < ratio2;
        }

        public static bool operator >(AspectRatio aspectRatio1, AspectRatio aspectRatio2)
        {
            float ratio1 = aspectRatio1;
            float ratio2 = aspectRatio2;
            return ratio1 > ratio2;
        }

        #endregion
    }
}