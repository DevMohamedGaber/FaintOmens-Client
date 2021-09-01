using UnityEngine;
namespace Game.UI
{
    public class UIWardrobeSlot : UIItemSlotWithType {
        [SerializeField] GameObject equiped;
        public bool isEquiped => equiped.activeSelf;
        public void Assign(WardrobeItem item, int index, bool isEquiped) {
            Unassign();
            ID = index;
            data.id = item.data.itemId;
            data.plus = item.plus;
            SetIcon(item.data.image);
            amount = 1;
            SetPlus(item.plus);
            quality.SetFrame(item.data.quality);
            if(type != null)
                type.SetActive(false);
            if(equiped != null)
                equiped.SetActive(isEquiped);
            //bound.SetActive(data.bound);
        }
        public void Assign(UIItemSlot slot, bool isEquiped) {
            base.Assign(slot);
            if(equiped != null)
                equiped.SetActive(isEquiped);
        }
        public void Unassign() {
            base.Unassign();
            equiped.SetActive(false);
        }
    }
}