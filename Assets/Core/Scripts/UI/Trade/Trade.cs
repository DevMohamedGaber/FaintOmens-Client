using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class Trade : Window
    {
        [SerializeField] Transform inventory;
        [SerializeField] UICountSwitch countSwitch;
        [SerializeField] TradePanel myTrade;
        [SerializeField] TradePanel otherTrade;
        [SerializeField] GameObject acceptBtn;
        [SerializeField] GameObject acceptedObj;
        [SerializeField] int slotsPerTrade;
        bool accepted;
        byte confirmTimes;
        public override void Refresh()
        {
            int nextIndex = 0;
            for(int i = 0; i < player.own.inventorySize; i++)
            {
                if(!player.own.inventory[i].isEmpty && !player.own.inventory[i].item.bound)
                {
                    UIItemSlot slot = inventory.GetChild(nextIndex).GetComponent<UIItemSlot>();
                    slot.Assign(player.own.inventory[i], i);
                    // if already selected reduse the selected amount
                    int selectedIndex = myTrade.IndexOf(i);
                    if(selectedIndex > -1)
                    {
                        slot.SetAmount(slot.amount - myTrade.offerItems[selectedIndex].amount);
                    }
                    // if still has more items
                    if(slot.amount > 0) {
                        slot.onDoubleClick.SetListener((itemSlot) =>
                        {
                            if(itemSlot.amount < 2)
                            {
                                myTrade.AddItem(slot);
                                Refresh();
                            }
                            else
                            {
                                ShowSelectAmount(itemSlot);
                            }
                        });
                        nextIndex++;
                    }
                    else slot.Unassign();
                }
            }
        }
        public void OnConfirmCounter()
        {
            // mimic from UpdateInventory
        }
        public void UpdateConfirmedOffer(ItemSlot[] offeredItems, uint offeredGold, uint offeredDiamonds)
        {
            otherTrade.Set(offeredItems, offeredGold, offeredDiamonds);
        }
        public void OnConfirm()
        {
            if(accepted)
            {
                Notifications.list.Add("you have already accepted the offer");
                return;
            }
            if(confirmTimes < 3)
            {
                TradeOfferContent offer = myTrade.GetOfferContent();
                if(offer.IsValid(player))
                {
                    player.CmdTradeConfirmOffer(offer);
                    confirmTimes++;
                }
            }
            else
            {
                Notifications.list.Add("you can't confirm more than 3 times");
            }
        }
        public void OnAccept()
        {
            if(!accepted) {
                TradeOfferContent offer = myTrade.GetOfferContent();
                if(offer.IsValid(player))
                {
                    player.CmdTradeAcceptOffer();
                }
            }
            else
            {
                Notifications.list.Add("Already accepted the trade offer");
            }
        }
        public void OtherAccepted()
        {
            acceptedObj.SetActive(true);
        }
        public void MineAccepted()
        {
            accepted = true;
            acceptBtn.SetActive(false);
        }
        public void OnGoldEndEdit(string textValue)
        {
            uint value = Convert.ToUInt32((string)textValue);
            if(player.own.gold < value)
            {
                Notify.DontHaveEnoughGold();
                myTrade.gold.text = "0";
            }
        }
        public void OnDiamondsEndEdit(string textValue)
        {
            uint value = Convert.ToUInt32((string)textValue);
            if(player.own.diamonds < value) {
                Notify.DontHaveEnoughDiamonds();
                myTrade.diamonds.text = "0";
            }
        }
        void ShowSelectAmount(UIItemSlot slot)
        {
            countSwitch.Limits(1, slot.amount);
            countSwitch.index = slot.ID;
            countSwitch.gameObject.SetActive(true);
        }
        void Awake()
        {
            myTrade.Initialize(slotsPerTrade);
            otherTrade.Initialize(slotsPerTrade);
        }
        void OnDisable()
        {
            myTrade.Clear();
            otherTrade.Clear();
        }
    }
}