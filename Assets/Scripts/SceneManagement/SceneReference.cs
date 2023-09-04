using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AirHockey.SceneManagement
{
    /// <summary>
    /// A Unity scene reference. Useful to serialize scene references as an object field, and not a string.
    /// </summary>
    [Serializable]
    internal class SceneReference
    {
        #region Serialized fields
        
        /// <summary>
        /// Reference to the scene asset in the project folder. Used only by the property drawer.
        /// </summary>
        [SerializeField, UsedImplicitly] private Object _scene;
        /// <summary>
        /// The scene path, also set by the property drawer.
        /// </summary>
        [SerializeField] private string _scenePath;

        #endregion

        #region Public

        public static implicit operator string(SceneReference sceneField) => sceneField._scenePath;

        #endregion
    }
}