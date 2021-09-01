/* - MODIFICATIONS TO THE MAIN CLASS -
- Add keyword (partial)
- remove (Awake) method
*/
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace DuloGames.UI {
    public partial class UITab : IPointerClickHandler {
        [Serializable] public class OnLeftClickEvent : UnityEvent<UITab> { }
        public OnLeftClickEvent onLeftClick = new OnLeftClickEvent();

        public void SetText(string text) {
            //if(this.m_TextTarget == null) return;
            //this.m_TextTarget.text = text;
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            /*if (eventData.button == PointerEventData.InputButton.Left) {
                if (this.onLeftClick != null)
                    this.onLeftClick.Invoke(this);
            }*/
        }
    }
}