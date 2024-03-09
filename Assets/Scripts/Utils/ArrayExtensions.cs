using System;

namespace LazySquirrelLabs.AirHockey.Utils
{
	internal static class ArrayExtensions
    {
        #region Internal

        /// <summary>
        /// Selects a random element of an array.
        /// </summary>
        /// <param name="array">The array to select an element from.</param>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown whenever the source array is null or empty.</exception>
        internal static T Random<T>(this T[] array)
        {
            if (array == null)
                throw new ArgumentException("Source array can not be null");

            if (array.Length == 0)
                throw new ArgumentException("Source array can not be empty");
            
            var randomIndex = UnityEngine.Random.Range(0, array.Length);
            return array[randomIndex];
        }

        #endregion
    }
}