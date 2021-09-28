using UnityEngine;
using System;
using TMPro;
namespace Game.UI
{
    public class Workshop_Plus : SubWindow
    {
    #region Data
        [Header("UI")]
        [SerializeField] Transform invContent;
        [SerializeField] GameObject prefab;
        [SerializeField] UIWorkshopItemSlot selectedSlot;
        [SerializeField] UICountableItemSlot stonesSlot;
        [SerializeField] UIItemSlot luckCharmSlot;
        [SerializeField] TMP_Text sRateTxt;
        [SerializeField] GameObject luckCharmListObj;
        [SerializeField] TMP_Text costTxt;
        [SerializeField] UIItemSlot[] luckCharmSlots;
        [Header("Data")]
        [SerializeField] Item[] stonesList;
        [SerializeField] Item[] luckCharmsList;
        [SerializeField] Color[] sRateColors;
        UIWorkshopItemSlot[] slots;
    #endregion
    #region Update
        public override void Refresh()
        {
            RefreshInventory();
            UpdateEnhancmentSlots();
        }
        void RefreshInventory()
        {
            int nextSlot = 0, i = 0;
            // equipments
            for(i = 0; i < player.equipment.Count; i++)
            {
                if(!player.equipment[i].isEmpty)
                {
                    if(player.equipment[i].item.plus == Storage.data.item.maxPlus || (selectedSlot.IsAssigned() &&
                        selectedSlot.from == WorkshopOperationFrom.Equipments && selectedSlot.ID == i))
                        continue;
                    slots[nextSlot].Assign(player.equipment[i], i, WorkshopOperationFrom.Equipments);
                    slots[nextSlot].onDoubleClick.SetListener((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                    nextSlot++;
                }
            }
            // accessories
            for(i = 0; i < player.own.accessories.Count; i++)
            {
                if(!player.own.accessories[i].isEmpty)
                {
                    if(player.own.accessories[i].item.plus == Storage.data.item.maxPlus || (selectedSlot.IsAssigned() &&
                        selectedSlot.from == WorkshopOperationFrom.Accessories && selectedSlot.ID == i))
                        continue;
                    slots[nextSlot].Assign(player.own.accessories[i], i, WorkshopOperationFrom.Accessories);
                    slots[nextSlot].onDoubleClick.SetListener((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                    nextSlot++;
                }
            }
            // inventory
            for(i = 0; i < player.own.inventorySize; i++)
            {
                if(!player.own.inventory[i].isEmpty && player.own.inventory[i].item.data is EquipmentItem)
                {
                    if(player.own.inventory[i].item.plus == Storage.data.item.maxPlus || (selectedSlot.IsAssigned() &&
                        selectedSlot.from == WorkshopOperationFrom.Inventory && selectedSlot.ID == i))
                        continue;
                    slots[nextSlot].Assign(player.own.inventory[i], i, WorkshopOperationFrom.Inventory);
                    slots[nextSlot].onDoubleClick.SetListener<UIItemSlot>((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                    nextSlot++;
                }
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
        void UpdateEnhancmentSlots()
        {
            // update selected item
            if(selectedSlot.IsAssigned())
            {
                // selected equipment
                if(selectedSlot.from == WorkshopOperationFrom.Equipments)
                {
                    selectedSlot.UpdateData(player.equipment[selectedSlot.ID]);
                }
                else if(selectedSlot.from == WorkshopOperationFrom.Accessories)
                {
                    selectedSlot.UpdateData(player.own.accessories[selectedSlot.ID]);
                }
                else if(selectedSlot.from == WorkshopOperationFrom.Inventory)
                {
                    selectedSlot.UpdateData(player.own.inventory[selectedSlot.ID]);
                }
                
                if(selectedSlot.data.plus == Storage.data.item.maxPlus)
                {
                    OnDeselectSlot();
                    return;
                }
                // stones
                Item reqStone = stonesList[(int)Math.Floor((int)selectedSlot.data.plus / 12d)];
                stonesSlot.Assign(reqStone, player.InventoryCountById(reqStone.id), Storage.data.item.plusUpCount[selectedSlot.data.plus]);
                costTxt.text = Storage.data.item.plusUpCost[(int)selectedSlot.data.plus].ToString();
                float sRate = Storage.data.item.plusUpSuccessRate[(int)selectedSlot.data.plus];
                // update luck Charm Slot
                if(luckCharmSlot.IsAssigned())
                {
                    uint lcCount = player.InventoryCountById(luckCharmSlot.data.id);
                    if(lcCount == 0)
                    {
                        luckCharmSlot.Unassign();
                    }
                    else
                    {
                        luckCharmSlot.SetAmount(lcCount);
                        sRate += ((LuckCharmItem)luckCharmsList[luckCharmSlot.ID].data).amount;
                    }
                }
                sRateTxt.text = sRate.ToString("F0") + "%";
                //Debug.Log((int)Math.Floor(sRate / 25f) - 1);
                sRateTxt.color = sRateColors[(int)Math.Floor(sRate / 25f) - 1];
                return;
            }
            else
            {
                stonesSlot.Unassign();
                luckCharmSlot.Unassign();
                sRateTxt.text = "-";
                sRateTxt.color = Color.white;
                costTxt.text = "-";
            }
        }
    #endregion
    #region Selected ItemSlot
        public void OnDeselectSlot()
        {
            selectedSlot.Unassign();
            Refresh();
        }
        void OnSelectSlot(UIWorkshopItemSlot itemSlot)
        {
            if(itemSlot.IsAssigned())
            {
                selectedSlot.Assign(itemSlot);
                Refresh();
            }
        }
    #endregion
    #region Luck Charm
        public void OnSelectLuckCharm(int index)
        {
            if(index == -1)
            {
                luckCharmSlot.Unassign();
            }
            else
            {
                if(index < 0 && index >= luckCharmsList.Length)
                {
                    Notifications.list.Add("Select a valid luckCharm");
                    return;
                }
                //uint playerLCCount = player.InventoryCountById(luckCharmsList[index].id);
                //if(playerLCCount < 1) {
                //    Notifications.list.Add("Insuffitionte luck Charm count");
                //    return;
                //}
                CloseLuckCharmListObj();
                luckCharmSlot.Assign(luckCharmSlots[index]);
            }
        }
        public void OnOpenLuckCharmList()
        {
            for(int i = 0; i < luckCharmSlots.Length; i++)
            {
                luckCharmSlots[i].SetAmount(player.InventoryCountById(luckCharmSlots[i].data.id));
            }
        }
        public void CloseLuckCharmListObj()
        {
            luckCharmListObj.SetActive(false);
        }
    #endregion
    #region Operations
        public void OnEnhance()
        {
            if(!selectedSlot.IsAssigned())
            {
                Notifications.list.Add("Please select an item to enhance");
                return;
            }
            if(!stonesSlot.IsAssigned())
            {
                Notifications.list.Add("Please select an item to enhance");
                return;
            }
            if(selectedSlot.data.plus >= Storage.data.item.maxPlus)
            {
                Notifications.list.Add("Item reached max plus");
                return;
            }
            if(player.own.gold < Storage.data.item.plusUpCost[(int)selectedSlot.data.plus])
            {
                Notifications.list.Add("Gold isn't enough");
                return;
            }
            if(stonesSlot.amount < (int)Math.Floor((int)selectedSlot.data.plus / 12d))
            {
                Notifications.list.Add("Not enough stones");
                return;
            }
            if(luckCharmSlot.IsAssigned() && luckCharmSlot.amount > 0)
            {
                Notifications.list.Add("Selected LuckCharm isn't enough");
                return;
            } 
            player.CmdWorkshopPlus(selectedSlot.ID, selectedSlot.from, luckCharmSlot.ID);
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            Refresh();
        }
        void OnDisable()
        {
            selectedSlot.Unassign();
            for(int i = 0; i < slots.Length; i++)
            {
                slots[i].Unassign();
            }
        }
        void Awake()
        {
            slots = new UIWorkshopItemSlot[(int)player.own.inventorySize + Storage.data.player.equipmentCount + Storage.data.player.accessoriesCount];
            for(int i = 0; i < slots.Length; i++)
            {
                GameObject go = Instantiate(prefab, invContent, false);
                slots[i] = go.transform.GetComponent<UIWorkshopItemSlot>();
            }
            for(int i = 0; i < luckCharmSlots.Length; i++)
            {
                luckCharmSlots[i].Assign(luckCharmsList[i]);
            }
        }
    #endregion
    }
}