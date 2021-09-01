using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class UITrade : UIWindowBase {
        [SerializeField] Transform inventory;
        [SerializeField] UICountSwitch countSwitch;
        [SerializeField] UITradePanel myTrade;
        [SerializeField] UITradePanel otherTrade;
        [SerializeField] GameObject acceptBtn;
        [SerializeField] GameObject acceptedObj;
        [SerializeField] int slotsPerTrade;
        bool accepted;
        byte confirmTimes;
        public override void Refresh() {
            int nextIndex = 0;
            for(int i = 0; i < player.own.inventorySize; i++) {
                if(!player.own.inventory[i].isEmpty && !player.own.inventory[i].item.bound) {
                    UIItemSlot slot = inventory.GetChild(nextIndex).GetComponent<UIItemSlot>();
                    slot.Assign(player.own.inventory[i], i);
                    // if already selected reduse the selected amount
                    int selectedIndex = myTrade.IndexOf(i);
                    if(selectedIndex > -1) {
                        slot.SetAmount(slot.amount - myTrade.offerItems[selectedIndex].amount);
                    }
                    // if still has more items
                    if(slot.amount > 0) {
                        slot.onDoubleClick.SetListener((itemSlot) => {
                            if(itemSlot.amount < 2) {
                                myTrade.AddItem(slot);
                                Refresh();
                            }
                            else ShowSelectAmount(itemSlot);
                        });
                        nextIndex++;
                    }
                    else slot.Unassign();
                }
            }
        }
        public void OnConfirmCounter() {
            // mimic from UpdateInventory
        }
        public void UpdateConfirmedOffer(ItemSlot[] offeredItems, uint offeredGold, uint offeredDiamonds) {
            otherTrade.Set(offeredItems, offeredGold, offeredDiamonds);
        }
        public void OnConfirm() {
            if(accepted) {
                UINotifications.list.Add("you have already accepted the offer");
                return;
            }
            if(confirmTimes < 3) {
                TradeOfferContent offer = myTrade.GetOfferContent();
                if(offer.IsValid(player)) {
                    player.CmdTradeConfirmOffer(offer);
                    confirmTimes++;
                }
            }
            else UINotifications.list.Add("you can't confirm more than 3 times");
        }
        public void OnAccept() {
            if(!accepted) {
                TradeOfferContent offer = myTrade.GetOfferContent();
                if(offer.IsValid(player)) {
                    player.CmdTradeAcceptOffer();
                }
            }
            else UINotifications.list.Add("Already accepted the trade offer");
        }
        public void OtherAccepted() {
            acceptedObj.SetActive(true);
        }
        public void MineAccepted() {
            accepted = true;
            acceptBtn.SetActive(false);
        }
        public void OnGoldEndEdit(string textValue) {
            uint value = Convert.ToUInt32((string)textValue);
            if(player.own.gold < value) {
                Notify.DontHaveEnoughGold();
                myTrade.gold.text = "0";
            }
        }
        public void OnDiamondsEndEdit(string textValue) {
            uint value = Convert.ToUInt32((string)textValue);
            if(player.own.diamonds < value) {
                Notify.DontHaveEnoughDiamonds();
                myTrade.diamonds.text = "0";
            }
        }
        void ShowSelectAmount(UIItemSlot slot) {
            countSwitch.Limits(1, slot.amount);
            countSwitch.index = slot.ID;
            countSwitch.gameObject.SetActive(true);
        }
        void Awake() {
            myTrade.Initialize(slotsPerTrade);
            otherTrade.Initialize(slotsPerTrade);
        }
        void OnDisable() {
            myTrade.Clear();
            otherTrade.Clear();
        }
        
        [Serializable]
        public struct UITradePanel {
            [SerializeField] Transform itemsContent;
            [SerializeField] bool canDelete;
            public TMP_InputField gold;
            public TMP_InputField diamonds;
            public List<UIItemSlot> offerItems;
            List<UIItemSlot> itemsList;
            public void AddItem(UIItemSlot newSlot, uint amount = 0) {
                if(offerItems.Count > 0) {
                    for(int i = 0; i < offerItems.Count; i++) {
                        if(offerItems[i].ID == newSlot.ID) {
                            offerItems[i].SetAmount(offerItems[i].amount + newSlot.amount);
                            UpdateSlots();
                            return;
                        }
                    }
                }
                offerItems.Add(newSlot);
                if(amount > 0) {
                    offerItems[offerItems.Count - 1].SetAmount(amount);
                }
                UpdateSlots();
            }
            public void Set(ItemSlot[] newItems, uint newGold, uint newDiamonds) {
                gold.text = newGold.ToString();
                diamonds.text = newDiamonds.ToString();
                offerItems.Clear();
                if(newItems.Length > 0) {
                    for(int i = 0; i < newItems.Length; i++) {
                        offerItems.Add(new UIItemSlot());
                        offerItems[i].Assign(newItems[i]);
                    }
                }
                UpdateSlots();
                /*for(int i = 0; i < items.Count; i++) {
                    if(itemList.Length > 0 && i < itemList.Length) {
                        items[i].Assign(itemList[i]);
                    }
                    else items[i].Unassign();
                }*/
            }
            public void UpdateSlots() {
                for(int i = 0; i < itemsList.Count; i++) {
                    if(i < offerItems.Count) {
                        itemsList[i].Assign(offerItems[i]);
                        if(canDelete) {
                            UITradePanel instance = this;
                            itemsList[i].onDoubleClick.SetListener((itemSlot) => {
                                for(int j = 0; j < instance.offerItems.Count; j++) {
                                    if(instance.offerItems[j].ID == itemSlot.ID) {
                                        instance.offerItems.RemoveAt(j);
                                        instance.UpdateSlots();
                                        UIManager.data.pages.trade.Refresh();
                                    }
                                }
                            });
                        }
                    }
                    else itemsList[i].Unassign();
                }
            }
            public TradeOfferContent GetOfferContent() {
                TradeOfferContent offer = new TradeOfferContent();
                offer.gold = Convert.ToUInt32(gold.text);
                offer.diamonds = Convert.ToUInt32(diamonds.text);
                offer.items = new IndexedAmount[offerItems.Count];
                if(offerItems.Count > 0) {
                    for(int i = 0; i < offerItems.Count; i++) {
                        offer.items[i].index = offerItems[i].ID;
                        offer.items[i].amount = offerItems[i].amount;
                    }
                }
                return offer;
            }
            public int IndexOf(int index) {
                if(offerItems.Count > 0) {
                    for(int i = 0; i < offerItems.Count; i++) {
                        if(offerItems[i].ID == index)
                            return i;
                    }
                }
                return -1;
            }
            public void Initialize(int count) {
                UIUtils.BalancePrefabs(UIManager.data.assets.itemSlotPrefab, count, itemsContent);
                offerItems = new List<UIItemSlot>();
                itemsList = new List<UIItemSlot>();
                for(int i = 0; i < count; i++) {
                    itemsList.Add(itemsContent.GetChild(i).GetComponent<UIItemSlot>());
                }
            }
            public void Clear() {
                offerItems.Clear();
                gold.text = "0";
                diamonds.text = "0";
            }
        }
    }
}