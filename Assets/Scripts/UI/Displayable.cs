using UnityEngine;

namespace AirHockey.UI
{
    public class Displayable : MonoBehaviour
    {
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