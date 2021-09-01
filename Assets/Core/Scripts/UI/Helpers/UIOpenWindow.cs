using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Game.UI
{
    public class UIOpenWindow : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] GameObject window;
        public void OnPointerClick(PointerEventData eventData)
        {
            if(window != null)
            {
                window.SetActive(true);
            }
        }
    }
}