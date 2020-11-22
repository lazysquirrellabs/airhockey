using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI
{
    public class Displayable : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private Button _dismissButton;

        #endregion

        #region Setup

        protected virtual void Awake()
        {
            _dismissButton.onClick.AddListener(Hide);
        }

        protected virtual void OnDestroy()
        {
            _dismissButton.onClick.RemoveListener(Hide);
        }

        #endregion

        #region Public

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        
        #endregion
    }
}