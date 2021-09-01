using UnityEngine;
namespace Game.UI
{
    public class UIWorkshopItemSlot : UIItemSlot {
        [SerializeField] GameObject equipedObj;
        public WorkshopOperationFrom from;
        public void Assign(ItemSlot slot, int id, WorkshopOperationFrom from = WorkshopOperationFrom.Inventory) {
            base.Assign(slot, id);
            SetFrom(from);
        }
        public void Assign(UIWorkshopItemSlot slot) {
            this.Assign(item: slot.data, amount: slot.amount);
            ID = slot.ID;
            SetFrom(slot.from);
        }
        public void UpdateData(ItemSlot newItemSlot) {
            SetData(newItemSlot.item, newItemSlot.amount);
        }
        public void Unassign() {
            base.Unassign();
            equipedObj.SetActive(false);
            from = WorkshopOperationFrom.Inventory;
        }
        void SetFrom(WorkshopOperationFrom from) {
            this.from = from;
            equipedObj.SetActive(from != WorkshopOperationFrom.Inventory);
        }
    }
}