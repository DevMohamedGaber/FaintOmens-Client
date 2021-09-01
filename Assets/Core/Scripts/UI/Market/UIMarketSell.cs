using UnityEngine;
using UnityEngine.UI;
using System;
public class UIMarketSell : MonoBehaviour {
    /*public GameObject panel;
    // inventory
    public UIInventorySlot ItemSlotPrefab;
    public Transform Content;
    // selling
    public int selectedItem = -1;
    public GameObject noItemSelected;
    public UIInventorySlot previewItemSlot;
    public GameObject SellingPanel;
    public InputField amountInput;
    public Button countIncrese;
    public Button countDecrese;
    public InputField priceInput;
    public Dropdown OffetTimeDropdown;
    public Text GoldFee;
    public Button ConfirmButton;
    // variables
    public int amount = 1;
    public int price = 2;

    void Update() {
        if(panel.activeSelf) {
            Player player = Player.localPlayer;
            if(player) {
                LoadInventory(player);
                SellHandler(player);
            }
        } else {
            selectedItem = -1;
        }
    }
    protected void LoadInventory(Player player) {
        UIUtils.BalancePrefabs(ItemSlotPrefab.gameObject, player.own.inventory.Count, Content);
        int nextSlot = 0;
        for(int i = 0; i < player.own.inventory.Count; i++) {
            if(player.own.inventory[i].amount < 1) continue;
            if(!player.own.inventory[i].item.bound && player.own.inventory[i].item.data.tradable) {
                ItemSlot itemSlot = player.own.inventory[i];
                UIInventorySlot slot = Content.GetChild(nextSlot).GetComponent<UIInventorySlot>();
                /*slot.tooltip.enabled = true;
                slot.tooltip.text = itemSlot.ToolTip();
                slot.image.color = Color.white;
                slot.image.sprite = itemSlot.item.image;
                slot.amountOverlay.SetActive(itemSlot.amount > 1);
                slot.amountText.text = itemSlot.amount.ToString();
                slot.setPlus(itemSlot.item.plus);
                var iCopy = i;
                slot.selected(iCopy == selectedItem);
                slot.button.onClick.SetListener(() => {
                    reset();
                    selectedItem = iCopy;
                });
                nextSlot++;
            }
        }
        for(int i = nextSlot; i < player.own.inventory.Count; i++) {
            UIInventorySlot slot = Content.GetChild(i).GetComponent<UIInventorySlot>();
            /*slot.tooltip.enabled = false;
            slot.image.color = Color.clear;
            slot.image.sprite = null;
            slot.amountOverlay.SetActive(false);
            slot.selected(false);
        }
    }
    protected void SellHandler(Player player) {
        noItemSelected.SetActive(selectedItem == -1);
        SellingPanel.SetActive(selectedItem > -1);
        if(selectedItem > -1) {
            ItemSlot itemSlot = player.own.inventory[selectedItem];
            /*previewItemSlot.image.color = Color.white;
            previewItemSlot.image.sprite = itemSlot.item.image;
            previewItemSlot.setPlus(itemSlot.item.plus);

            amountInput.text = amount.ToString();
            amountInput.onValueChanged.SetListener((v) => {
                if(Convert.ToInt32(v) < 1) amount = 1;
                else if(Convert.ToInt32(v) >= itemSlot.amount) amount = itemSlot.amount;
                else amount = Convert.ToInt32(v);
            });
            countIncrese.onClick.SetListener(() => {
                if(amount+1 <= itemSlot.amount) amount += 1;
            });
            countDecrese.onClick.SetListener(() => {
                if(amount-1 >= 1) amount -= 1;
            });
            priceInput.text = price.ToString();
            priceInput.onValueChanged.SetListener((v) => {
                if(Convert.ToInt32(v) < 2) price = 2;
                else price = Convert.ToInt32(v);
            });
            
            GoldFee.text = Storage.data.MarketOfferTimes[OffetTimeDropdown.value].fee.ToString();

            ConfirmButton.onClick.SetListener(() => {
                if(player.own.gold >= Storage.data.MarketOfferTimes[OffetTimeDropdown.value].fee && amount > 0 && price >= 2) {
                    //player.CmdAddMarketItem(selectedItem, amount, price, OffetTimeDropdown.value);
                    reset();
                }
            });
        }
    }
    protected void reset() {
        amount = 1;
        price = 2;
        selectedItem = -1;
        OffetTimeDropdown.value = 0;
    }
    void Start() {
        foreach(MarketOfferTime time in Storage.data.MarketOfferTimes) {
            OffetTimeDropdown.options.Add (new Dropdown.OptionData() {text=$"{time.hours} hours"});
        }
    }*/
}