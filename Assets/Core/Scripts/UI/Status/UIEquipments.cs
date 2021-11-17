using UnityEngine;
namespace Game.UI
{
    public class UIEquipments : SubWindow
    {
        [SerializeField] UIItemSlotWithType[] slots;
        public override void Refresh()
        {
            for(int i = 0; i < player.own.equipment.Count; i++)
            {
                if(player.own.equipment[i].isEmpty)
                {
                    slots[i].Unassign();
                }
                else
                {
                    slots[i].Assign(player.own.equipment[i], i);
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