using UnityEngine;
using UnityEngine.EventSystems;

namespace AirHockey.UI
{
    public class URLClicker : MonoBehaviour, IPointerClickHandler
    {
        #region Serialized fields

        [SerializeField] private string _url;

        #endregion

        #region Event handlers

        public void OnPointerClick(PointerEventData eventData)
        {
            Application.OpenURL(_url);
        }
        
        #endregion
    }
}