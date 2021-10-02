using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
namespace Game.UI
{
    public class UIInventory_Sell : SubWindow
    {
        [SerializeField] UIItemSlot prefab;
        [SerializeField] Transform Content;
        [SerializeField] Button SelectAllButton;
        [SerializeField] Button ConfirmButton;
        [SerializeField] Text TotalGoldText;
        [SerializeField] Button CloseOverLay;
        List<int> items = new List<int>();
        long totalGold;
        bool isAllSelected = false;
        public void Refresh()
        {

        }
        void Update() {
            UIUtils.BalancePrefabs(prefab.gameObject, Storage.data.player.maxInventorySize, Content);
            int nextSlot = 0;
            for(int i = 0; i < player.own.inventory.Count; i++) {
                if (player.own.inventory[i].amount > 0 && player.own.inventory[i].item.data.sellable) {
                    UIItemSlot slot = Content.GetChild(nextSlot).GetComponent<UIItemSlot>();
                    slot.Assign(player.own.inventory[i], i);
                    slot.onClick.AddListener(ToggleSelection);
                    nextSlot++;
                } 
            }
            for(int i = nextSlot; i < player.own.inventory.Count; i++)
                Content.GetChild(i).GetComponent<UIItemSlot>().Unassign();

            TotalGoldText.text = totalGold.ToString();
            ConfirmButton.interactable = items.Count > 0;
        }
        void ToggleSelection(UIItemSlot slot) {
            if(items.Count > 0) {
                for (int i = 0; i < items.Count; i++) {
                    if(items[i] == slot.ID) {
                        Deselect(slot);
                        return;
                    }
                }
            } 
            Select(slot);
        }
        void Select(UIItemSlot slot) {
            if(slot.ID < 0) return;
            slot.transform.Find("Selected").gameObject.SetActive(true);
            items.Add(slot.ID);
            totalGold += slot.data.data.sellPrice * slot.amount;
        }
        void Deselect(UIItemSlot slot) {
            if(slot.ID < 0) return;
            slot.transform.Find("Selected").gameObject.SetActive(false);
            items.Remove(slot.ID);
            totalGold -= slot.data.data.sellPrice * slot.amount;
        }
        void onSell() {
            if(items.Count > 0){
                player.CmdSellInventoryItemsForGold(items.ToArray());
            }
        }
        void onSelectAll() {
            if(isAllSelected) {
                DeselectAll();
            } else {
                for(int i = 0; i < player.own.inventory.Count; i++) {
                    if(player.own.inventory[i].amount > 0 && player.own.inventory[i].item.data.sellable) {
                        if(items.Contains(i)) continue;
                        Select(GetSlotById(i));
                    }
                }
            }
            isAllSelected = !isAllSelected;
        }
        void DeselectAll() {
            for(int i = 0; i < player.own.inventory.Count; i++) {
                if(player.own.inventory[i].amount > 0 && player.own.inventory[i].item.data.sellable) {
                    if(!items.Contains(i)) continue;
                    Deselect(GetSlotById(i));
                }
            }
        }
        UIItemSlot GetSlotById(int id) {
            UIItemSlot[] list = Content.GetComponentsInChildren<UIItemSlot>();
            for(int i = 0; i < list.Length; i++) {
                if(list[i].ID == id) 
                    return list[i];
            }
            return new UIItemSlot();
        }
        void OnEnable() {
            SelectAllButton.onClick.AddListener(onSelectAll);
            ConfirmButton.onClick.AddListener(onSell);
            CloseOverLay.onClick.AddListener(() => gameObject.SetActive(false));
        } 
        void OnDisable() {
            SelectAllButton.onClick.RemoveAllListeners();
            ConfirmButton.onClick.RemoveAllListeners();
            CloseOverLay.onClick.RemoveAllListeners();
        }
    }
}