namespace AirHockey.Utils
{
    public static class ArrayExtensions
    {
        #region Public

        public static T Random<T>(this T[] array)
        {
            var randomIndex = (UnityEngine.Random.Range(0, array.Length));
            return array[randomIndex];
        }

        #endregion
    }
}