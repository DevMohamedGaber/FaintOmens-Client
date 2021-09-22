using UnityEngine;
namespace Game.UI
{
    public class UIEquipments : SubWindowBase
    {
        [SerializeField] UIItemSlotWithType[] slots;
        public override void Refresh()
        {
            for(int i = 0; i < player.equipment.Count; i++)
            {
                if(player.equipment[i].isEmpty)
                {
                    slots[i].Unassign();
                }
                else
                {
                    slots[i].Assign(player.equipment[i], i);
                }
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            Refresh();
        }
    }
}