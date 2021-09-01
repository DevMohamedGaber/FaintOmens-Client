using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
namespace Game.UI
{
    public class UIOpenWindowWithAction : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] GameObject window;
        [SerializeField] UnityEvent action;
        public void OnPointerClick(PointerEventData eventData)
        {
            if(window != null)
            {
                window.SetActive(true);
            }
            if(action != null)
            {
                action.Invoke();
            }
        }
    }
}