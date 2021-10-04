using UnityEngine;
using System.Collections.Generic;
using TMPro;
namespace Game.UI
{
    public class Inventory_Sell : SubWindow
    {
        [SerializeField] Transform content;
        [SerializeField] TMP_Text totalGoldTxt;
        UISelectableItemSlot[] slots;
        public override void Refresh()
        {
            if(player.own.inventory.Count < 1)
                return;

            int i;
            for(i = 0; i < player.own.inventorySize; i++)
            {
                if(player.own.inventory[i].isEmpty || !player.own.inventory[i].item.data.sellable)
                {
                    slots[i].Unassign();
                }
                else
                {
                    slots[i].Assign(slot: player.own.inventory[i], id: i);
                    slots[i].onClick.AddListener((itemSlot) => UpdateSellPrice());
                    slots[i].onDoubleClick.SetListener((itemSlot) =>
                    {
                        if(itemSlot.IsAssigned() && itemSlot.amount > 0)
                        {
                            UIManager.data.inScene.tooltip.Show(itemSlot.data, ToolTipFrom.Inventory, itemSlot.ID);
                        }
                    });
                }
            }
            for(i = player.own.inventorySize; i < Storage.data.player.maxInventorySize; i++)
            {
                slots[i].Lock();
            }
            UpdateSellPrice();
        }
        void UpdateSellPrice()
        {
            totalGoldTxt.text = GetTotalSellPrice().ToString();
        }
        uint GetTotalSellPrice()
        {
            uint result = 0;
            for (int i = 0; i < player.own.inventorySize; i++)
            {
                if(slots[i].IsAssigned())
                {
                    result += slots[i].SellPrice();
                }
            }
            return result;
        }
        int[] GetSelectedIndexes()
        {
            List<int> result = new List<int>();
            for (int i = 0; i < player.own.inventorySize; i++)
            {
                if(slots[i].IsSelected() && slots[i].IsAssigned() && slots[i].data.data.sellable)
                {
                    result.Add(slots[i].ID);
                }
            }
            return result.ToArray();
        }
        public void onConfirm()
        {
            int[] items = GetSelectedIndexes();
            if(items.Length == 0)
            {
                Notify.list.Add("Select items to sell");
                return;
            }
            player.CmdSellInventoryItemsForGold(items);
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            Refresh();
        }
        private void Awake()
        {
            UIUtils.BalancePrefabs(UIManager.data.assets.itemSlots.selectable, Storage.data.player.maxInventorySize, content);
            slots = new UISelectableItemSlot[Storage.data.player.maxInventorySize];
            for(int i = 0; i < Storage.data.player.maxInventorySize; i++)
            {
                slots[i] = content.GetChild(i).GetComponent<UISelectableItemSlot>();
            }
        }
    }
}