using UnityEngine;
using UnityEngine.EventSystems;
namespace Game.UI
{
    public class UIRotatePreview : MonoBehaviour, IBeginDragHandler, IDragHandler {
        [SerializeField] float speed = .3f;
        float x;
        public void OnBeginDrag(PointerEventData eventData) => x = eventData.position.x;
        public void OnDrag(PointerEventData eventData) {
            PreviewManager.singleton.Rotate((x - eventData.position.x) * speed);
            x = eventData.position.x;
        }
        void OnDisable() {
            PreviewManager.singleton.Clear();
        }
    }
}