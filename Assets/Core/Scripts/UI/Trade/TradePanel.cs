using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Game.UI
{
    [Serializable]
    public struct TradePanel
    {
        [SerializeField] Transform itemsContent;
        [SerializeField] bool canDelete;
        public TMP_InputField gold;
        public TMP_InputField diamonds;
        public List<UIItemSlot> offerItems;
        List<UIItemSlot> itemsList;
        public void AddItem(UIItemSlot newSlot, uint amount = 0)
        {
            if(offerItems.Count > 0)
            {
                for(int i = 0; i < offerItems.Count; i++)
                {
                    if(offerItems[i].ID == newSlot.ID)
                    {
                        offerItems[i].SetAmount(offerItems[i].amount + newSlot.amount);
                        UpdateSlots();
                        return;
                    }
                }
            }
            offerItems.Add(newSlot);
            if(amount > 0)
            {
                offerItems[offerItems.Count - 1].SetAmount(amount);
            }
            UpdateSlots();
        }
        public void Set(ItemSlot[] newItems, uint newGold, uint newDiamonds)
        {
            gold.text = newGold.ToString();
            diamonds.text = newDiamonds.ToString();
            offerItems.Clear();
            if(newItems.Length > 0)
            {
                for(int i = 0; i < newItems.Length; i++)
                {
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
        public void UpdateSlots()
        {
            for(int i = 0; i < itemsList.Count; i++)
            {
                if(i < offerItems.Count)
                {
                    itemsList[i].Assign(offerItems[i]);
                    if(canDelete) {
                        TradePanel instance = this;
                        itemsList[i].onDoubleClick.SetListener((itemSlot) =>
                        {
                            for(int j = 0; j < instance.offerItems.Count; j++)
                            {
                                if(instance.offerItems[j].ID == itemSlot.ID)
                                {
                                    instance.offerItems.RemoveAt(j);
                                    instance.UpdateSlots();
                                    UIManager.data.pages.trade.Refresh();
                                }
                            }
                        });
                    }
                }
                else
                {
                    itemsList[i].Unassign();
                }
            }
        }
        public TradeOfferContent GetOfferContent()
        {
            TradeOfferContent offer = new TradeOfferContent();
            offer.gold = Convert.ToUInt32(gold.text);
            offer.diamonds = Convert.ToUInt32(diamonds.text);
            offer.items = new IndexedAmount[offerItems.Count];
            if(offerItems.Count > 0)
            {
                for(int i = 0; i < offerItems.Count; i++)
                {
                    offer.items[i].index = offerItems[i].ID;
                    offer.items[i].amount = offerItems[i].amount;
                }
            }
            return offer;
        }
        public int IndexOf(int index)
        {
            if(offerItems.Count > 0) {
                for(int i = 0; i < offerItems.Count; i++)
                {
                    if(offerItems[i].ID == index)
                        return i;
                }
            }
            return -1;
        }
        public void Initialize(int count)
        {
            UIUtils.BalancePrefabs(UIManager.data.assets.itemSlotPrefab, count, itemsContent);
            offerItems = new List<UIItemSlot>();
            itemsList = new List<UIItemSlot>();
            for(int i = 0; i < count; i++)
            {
                itemsList.Add(itemsContent.GetChild(i).GetComponent<UIItemSlot>());
            }
        }
        public void Clear()
        {
            offerItems.Clear();
            gold.text = "0";
            diamonds.text = "0";
        }
    }
}