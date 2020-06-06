using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AirHockey.SceneManagement
{
    [Serializable]
    public class SceneReference
    {
        #region Serialized fields

        [SerializeField, UsedImplicitly] private Object _scene;
        [SerializeField] private string _scenePath;

        #endregion

        #region Properties

        public static implicit operator string( SceneReference sceneField ) => sceneField._scenePath;

        #endregion
    }
}