using UnityEngine;
namespace Game.UI
{
    public class Workshop_Repair : SubWindow
    {
        [SerializeField] Transform invContent;
        [SerializeField] UIWorkshopItemSlot selectedSlot;
        //[SerializeField] UIItemSlot selectedSlot;
        [SerializeField] TMPro.TMP_Text duraTxt;
        [SerializeField] TMPro.TMP_Text costTxt;
        UIWorkshopItemSlot[] slots;
        public override void Refresh()
        {
            RefreshInventory();
            if(selectedSlot.IsAssigned())
            {
                duraTxt.text = $"{selectedSlot.data.durability} / {selectedSlot.data.MaxDurability()}";
                costTxt.text = selectedSlot.data.GetRepairDurabilityCost().ToString();
            }
        }
        void RefreshInventory()
        {
            int nextSlot = 0, i = 0;
            // equipments
            for(i = 0; i < player.own.equipment.Count; i++)
            {
                if(!player.own.equipment[i].isEmpty)
                    continue;
                if(selectedSlot.IsAssigned() && selectedSlot.from == WorkshopOperationFrom.Equipments && selectedSlot.ID == i)
                    continue;
                if(player.own.equipment[i].item.HasMaxDurability())
                    continue;
                slots[nextSlot].Assign(player.own.equipment[i], i, WorkshopOperationFrom.Equipments);
                slots[nextSlot].onDoubleClick.SetListener((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                nextSlot++;
            }
            // accessories
            for(i = 0; i < player.own.accessories.Count; i++)
            {
                if(!player.own.accessories[i].isEmpty)
                    continue;
                if(selectedSlot.IsAssigned() && selectedSlot.from == WorkshopOperationFrom.Accessories && selectedSlot.ID == i)
                    continue;
                if(player.own.accessories[i].item.HasMaxDurability())
                    continue;
                slots[nextSlot].Assign(player.own.accessories[i], i, WorkshopOperationFrom.Accessories);
                slots[nextSlot].onDoubleClick.SetListener((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                nextSlot++;
            }
            // inventory
            for(i = 0; i < player.own.inventorySize; i++)
            {
                if(!player.own.inventory[i].isEquipment)
                    continue;
                if(selectedSlot.IsAssigned() && selectedSlot.from == WorkshopOperationFrom.Inventory && selectedSlot.ID == i)
                    continue;
                if(player.own.inventory[i].item.HasMaxDurability())
                    continue;
                slots[nextSlot].Assign(player.own.inventory[i], i, WorkshopOperationFrom.Inventory);
                slots[nextSlot].onDoubleClick.SetListener<UIItemSlot>((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                nextSlot++;
            }
            // empty the rest
            if(nextSlot < slots.Length - 1)
            {
                for(i = nextSlot; i < slots.Length; i++)
                {
                    slots[i].Unassign();
                }
            }
        }
        void OnSelectSlot(UIWorkshopItemSlot itemSlot)
        {
            if(itemSlot.IsAssigned())
            {
                selectedSlot.Assign(itemSlot);
                Refresh();
            }
        }
        public void OnDeselectSlot()
        {
            selectedSlot.Unassign();
            Refresh();
        }
        public void OnRepair()
        {
            if(!selectedSlot.IsAssigned())
            {
                Notifications.list.Add("Please select an item to enhance");
                return;
            }
            if(selectedSlot.data.HasMaxDurability())
            {
                Notifications.list.Add("Equipment already has max durability");
                return;
            }
            if(player.own.gold < selectedSlot.data.GetRepairDurabilityCost())
            {
                Notify.DontHaveEnoughGold();
                return;
            }
            player.CmdWorkshopEquipmentRepair(selectedSlot.ID, selectedSlot.from);
        }
        void Awake()
        {
            slots = new UIWorkshopItemSlot[(int)player.own.inventorySize + Storage.data.player.equipmentCount + Storage.data.player.accessoriesCount];
            for(int i = 0; i < slots.Length; i++)
            {
                GameObject go = Instantiate(UIManager.data.assets.itemSlots.workshop, invContent, false);
                slots[i] = go.transform.GetComponent<UIWorkshopItemSlot>();
            }
        }
    }
}