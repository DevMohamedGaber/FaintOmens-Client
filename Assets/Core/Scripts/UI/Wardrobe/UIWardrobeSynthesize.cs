using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class UIWardrobeSynthesize : MonoBehaviour
    {
        [SerializeField] float updateInterval = .5f;
        [SerializeField] UIWardrobeSynthesize_slots slots;
        [SerializeField] UIWardrobeSynthesize_UI ui;
        [SerializeField] uint[] cost;
        [SerializeField] int goodLuck;
        Player player => Player.localPlayer;
        List<UIWardrobeSlot> inventory = new List<UIWardrobeSlot>();
        int successRate => slots.main.IsAssigned() ? 100 - (slots.main.data.plus * 7) + (slots.bless.IsAssigned() ? 50 : 0) : 0;
        public void UpdateData() {
            byte lastIndex = 0;
            // equiped items
            if(!slots.main.IsAssigned()) {
                for(byte i = 0; i < player.own.clothing.Count; i++) {
                    if(player.own.clothing[i].isUsed) {
                        //if(synthesizeSlots[0].Check(i, true) || synthesizeSlots[1].Check(i, true))
                        //    continue;
                        inventory[lastIndex].Assign(player.own.clothing[i], i, true);
                        lastIndex++;
                    }
                }
            }
            // inventory
            for(byte i = 0; i < player.own.inventory.Count; i++) {
                if(player.own.inventory[i].amount > 0) {
                    if(player.own.inventory[i].item.data is ClothingItem itemData) {
                        //if(synthesizeSlots[0].Check(i) || synthesizeSlots[1].Check(i))
                        //    continue;
                        if(slots.main.IsAssigned() && 
                        (   player.own.inventory[i].item.plus != slots.main.data.plus
                        ||  itemData.equipCategory != ((ClothingItem)slots.main.data.data).equipCategory 
                        ||  i == slots.other.ID))
                            continue;
                        inventory[lastIndex].Assign(player.own.inventory[i], i);
                        lastIndex++;
                    }
                    else if(slots.main.IsAssigned() && player.own.inventory[i].item.id == goodLuck) {
                        if(slots.bless.IsAssigned())
                            continue;
                        inventory[lastIndex].Assign(player.own.inventory[i], i);
                        lastIndex++;
                    }
                }
            }
            // empty slots
            for(byte i = lastIndex; i < inventory.Count; i++)
                inventory[i].Unassign();
            // ui
            ui.cost.text = slots.main.IsAssigned() ? cost[slots.main.data.plus].ToString() : "0";
            ui.cost.color = slots.main.IsAssigned() && player.own.gold < cost[slots.main.data.plus] ? Color.red : Color.white; 
            ui.rate.text = slots.main.IsAssigned() ? $"{successRate}%" : "-";
            ui.rate.color = slots.main.IsAssigned() && successRate < 50 ? Color.red : Color.white;
        }
        public void OnSynthesize() {
            if(!Check())
                return;
            // if success rate < 50% show check else procced
            if(successRate <= 50 && !slots.bless.IsAssigned()) ui.confirm.SetActive(true);
            else OnConfirmSynthesization();
        }
        public void OnConfirmSynthesization(bool check = false) {
            if(check && !Check())
                return;
            player.CmdWardrobeSynthesize(slots.main.ID, slots.main.isEquiped, slots.other.ID, slots.bless.ID);
        }
        public void OnSynthesizeDone(bool success) {
            slots.bless.Unassign();
            slots.other.Unassign();
            if(slots.main.isEquiped) slots.result.Assign(player.own.clothing[slots.main.ID], slots.main.ID, true);
            else                     slots.result.Assign(player.own.inventory[slots.main.ID], slots.main.ID);
            slots.main.Unassign();
            if(success) {
                // show effect on the result slot
            }
        }
        bool Check() {
            if(!slots.main.IsAssigned() || !slots.other.IsAssigned()) {
                Notifications.list.Add("Please choose 2 items to synthesize");
                return false;
            }
            if(slots.main.data.plus != slots.other.data.plus) {
                Notifications.list.Add("Both items has to have the same enhancment level");
                return false;
            }
            if(player.own.gold < cost[slots.main.data.plus]) {
                Notify.DontHaveEnoughGold();
                return false;
            }
            return true;
        }
        void OnSelect(UIItemSlot itemSlot) {
            if(slots.result.IsAssigned())
                slots.result.Unassign();
            if(itemSlot.data.data is ClothingItem) {
                if(!slots.main.IsAssigned()) {
                    slots.main.Assign(itemSlot, ((UIWardrobeSlot)itemSlot).isEquiped);
                    ((UIWardrobeSlot)itemSlot).Unassign();
                }
                else {
                    slots.other.Assign(itemSlot, false);
                    ((UIWardrobeSlot)itemSlot).Unassign();
                }
            }
            else if(itemSlot.amount > 0 && itemSlot.data.id == goodLuck) {
                slots.bless.Assign(itemSlot.data);
            }
            UpdateData();
        }
        void OnEnable() {
            for(int i = 0; i < player.own.inventorySize; i++) {
                inventory.Add(Instantiate(ui.prefab, ui.content, false).GetComponent<UIWardrobeSlot>());
                inventory[i].onClick.SetListener((itemSlot) => OnSelect(itemSlot));
            }
            InvokeRepeating("UpdateData", 0, updateInterval);
        }
        void OnDisable() {
            CancelInvoke("UpdateData");
            for(int i = 0; i < ui.content.childCount; i++)
                Destroy(ui.content.GetChild(i).gameObject);
            inventory = new List<UIWardrobeSlot>();
            slots.main.Unassign();
            slots.other.Unassign();
            slots.result.Unassign();
            ui.confirm.SetActive(false);
        }
        [System.Serializable] class UIWardrobeSynthesize_slots {
            public UIWardrobeSlot main;
            public UIWardrobeSlot other;
            public UIItemSlot bless;
            public UIWardrobeSlot result;
        }
        [System.Serializable] class UIWardrobeSynthesize_UI {
            public TMP_Text cost;
            public TMP_Text rate;
            public Transform content;
            public GameObject prefab;
            public GameObject confirm;
        }
    }
}