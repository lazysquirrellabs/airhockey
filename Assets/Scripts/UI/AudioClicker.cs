using UnityEngine;
using UnityEngine.EventSystems;

namespace AirHockey.UI
{
    /// <summary>
    /// Plays an <see cref="_audioSource"/> whenever the Pointer clicks on this component.
    /// </summary>
    public class AudioClicker : MonoBehaviour, IPointerClickHandler
    {
        #region Serialized fields

        [SerializeField] private AudioSource _audioSource;

        #endregion

        #region Event handlers

        public void OnPointerClick(PointerEventData eventData)
        {
            _audioSource.Play();
        }

        #endregion
    }
}