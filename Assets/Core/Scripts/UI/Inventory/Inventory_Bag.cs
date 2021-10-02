using UnityEngine;
using System;
using TMPro;
namespace Game.UI
{
    public class Inventory_Bag : SubWindow
    {
        [SerializeField] Transform content;
        [SerializeField] TMP_Text countText;
        [SerializeField] GameObject OpenSlotConfirmMsg;
        UIItemSlot[] slots;
        DateTime lastSort;
        public override void Refresh()
        {
            for(int i = 0; i < player.own.inventorySize; i++)
            {
                if(player.own.inventory.Count > 0 && player.own.inventory[i].amount > 0)
                {
                    slots[i].Assign(slot: player.own.inventory[i], id: i);
                    slots[i].onClick.SetListener((itemSlot) => {
                        if(itemSlot.IsAssigned() && itemSlot.amount > 0) {
                            UIManager.data.inScene.tooltip.Show(itemSlot.data, ToolTipFrom.Inventory, itemSlot.ID);
                            /*if(itemSlot.data.data is UsableItem &&
                                ((UsableItem)itemSlot.data.data).CanUse()) {
                                    player.CmdUseInventoryItem(itemSlot.ID);
                                    ToolTip.data.Hide();
                                }
                            ToolTip.data.onUse = () => {
                                if(itemSlot.data.data is UsableItem &&
                                    ((UsableItem)itemSlot.data.data).CanUse()) {
                                        player.CmdUseInventoryItem(itemSlot.ID);
                                        ToolTip.data.Hide();
                                    }
                            };*/
                            //ToolTip.data.ShowItem(itemSlot.data, itemSlot.transform.position, true);
                        }
                    });
                }
                else
                {
                    slots[i].Unassign();
                }
            }
            for(int i = player.own.inventorySize; i < Storage.data.player.maxInventorySize; i++)
            {
                int iCopy = i;
                slots[i].Lock(() => OpenSlotConfirm(iCopy));
            }
            countText.text = $"{player.InventorySlotsOccupied()} / {player.own.inventorySize}";
        }
        public void OnSort()
        {
            if(lastSort.AddSeconds(10) <= Server.time)
            {
                player.CmdSortInventory();
                lastSort = Server.time;
            }
        }
        public void OpenSlotConfirm(int slotIndex)
         {
            /*if(slotIndex <= Storage.data.player.maxInventorySize - 1) {
                OpenSlotConfirmMsg.SetActive(true);
                int slots = slotIndex - player.own.inventorySize + 1;
                OpenSlotConfirmHeader.text = $"OPEN {(slots > 1 ? $"{slots} SLOTS" : $"SLOT #{slotIndex+1}")}";
                OpenSlotConfirmKeys.text = slots.ToString();
                OpenSlotConfirmKeys.color = player.InventoryCountById(5455) > slots ? Color.white : Color.red;
                OpenSlotConfirmButton.onClick.SetListener(() => {
                    player.CmdOpenInventorySlots(slots);
                    OpenSlotConfirmMsg.SetActive(false);
                });
            }*/
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            Refresh();
        }
        private void Awake()
        {
            UIUtils.BalancePrefabs(UIManager.data.assets.itemSlotPrefab, Storage.data.player.maxInventorySize, content);
            slots = new UIItemSlot[Storage.data.player.maxInventorySize];
            for(int i = 0; i < Storage.data.player.maxInventorySize; i++)
            {
                slots[i] = content.GetChild(i).GetComponent<UIItemSlot>();
            }
        }
    }
}