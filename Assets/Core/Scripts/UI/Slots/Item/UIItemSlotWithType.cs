using UnityEngine;
namespace Game.UI
{
    public class UIItemSlotWithType : UIItemSlot
    {
        [SerializeField] protected GameObject type;
        public void Assign(ItemSlot item, int id)
        {
            base.Assign(slot: item, id: id);
            if(type != null)
            {
                type.SetActive(false);
            }
        }
        public override void Unassign()
        {
            base.Unassign();
            if(type != null)
            {
                type.SetActive(true);
            }
        }
    }
}