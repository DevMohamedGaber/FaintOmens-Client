using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
namespace Game.UI
{
    public class UICloseWindow : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] GameObject window;
        public void OnPointerClick(PointerEventData eventData)
        {
            if(window != null)
            {
                window.SetActive(false);
            }
        }
    }
}