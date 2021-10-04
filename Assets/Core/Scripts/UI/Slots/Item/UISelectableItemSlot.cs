using UnityEngine;
namespace Game.UI
{
    public class UISelectableItemSlot : UIItemSlot
    {
        [Header("Selectable")]
        [SerializeField] GameObject selectObj;
        bool selected;
        
        public bool IsSelected()
        {
            return selected;
        }
        public void Select()
        {
            selected = !selected;

            if(selectObj != null)
            {
                selectObj.SetActive(selected);
            }
        }
        public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if(IsAssigned())
            {
                Select();
            }
        }
        public override void Unassign()
        {
            base.Unassign();
            selected = false;
            selectObj.SetActive(false);
        }
    }
}