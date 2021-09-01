/* - MODIFICATIONS TO THE MAIN CLASS -
- remove (UIItemMall) class

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DuloGames.UI;

public partial class UIMall : MonoBehaviour {
    [SerializeField] private Text diamondsText;
    [SerializeField] private Text b_diamondsText;
    [SerializeField] private UITab categorySlotPrefab;
    [SerializeField] private Transform categoryContent;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private UIMallItem itemSlotPrefab;
    [SerializeField] private Transform itemContent;
    [SerializeField] private GameObject ConfirmMsg;
    [SerializeField] private UIItemSlot ConfirmSlot;
    [SerializeField] private Text ConfirmName;
    [SerializeField] private Image ConfirmCurrency;
    [SerializeField] private Text ConfirmCost;
    [SerializeField] private UICountSwitch ConfirmCount;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button ConfirmCloseOverLay;
    [SerializeField] private Button WindowCloseButton;
    Player player => Player.localPlayer;
    int category = 0;
    int item = -1;
    bool usingBound => Storage.data.ItemMallContent[category].bound;

    void Update() {
        if (player != null) {
            diamondsText.text = player.own.diamonds.ToString();
            b_diamondsText.text = player.own.b_diamonds.ToString();
            ShowMallItems();
            if(item > -1 && item < Storage.data.ItemMallContent[category].items.Count) ShowConfirmBuy();
            else ConfirmMsg.SetActive(false);
        }
        else gameObject.SetActive(false);
    }
    void ShowMallItems() {
        List<Item> items = Storage.data.ItemMallContent[category].items;
        UIUtils.BalancePrefabs(itemSlotPrefab.gameObject, items.Count, itemContent);
        for(int i = 0; i < items.Count; ++i) {
            UIMallItem slot = itemContent.GetChild(i).GetComponent<UIMallItem>();
            Item itemInfo = items[i];
            bool hasPrice = usingBound ? (player.own.b_diamonds >= itemInfo.data.itemMallPrice * Storage.data.BoundToUnboundRatio) : 
                                        (player.own.diamonds >= itemInfo.data.itemMallPrice);
            slot.itemSlot.Assign(itemInfo);
            slot.Name.text = itemInfo.Name;
            slot.currencyIcon.sprite = usingBound ? Storage.data.currencyIcons[2] : Storage.data.currencyIcons[1];
            slot.cost.text = itemInfo.data.itemMallPrice.ToString();
            int iCopy = i;
            slot.button.onClick.SetListener(() => {
                if(hasPrice) item = iCopy;
                //else player.Notifiy($"Insuficint Amount of {(usingBound ? "Bound Diamonds" : "Diamonds")}");
            });
        }
    }
    void ShowConfirmBuy() {
        ConfirmMsg.SetActive(true);
        Item itemInfo = Storage.data.ItemMallContent[category].items[item];
        float maxItems = usingBound ? (player.own.b_diamonds / itemInfo.data.itemMallPrice * Storage.data.BoundToUnboundRatio) : 
                                    (player.own.diamonds / itemInfo.data.itemMallPrice);
        ConfirmCount.Limits(1, (int)Math.Round((double)(maxItems)));
        ConfirmSlot.Assign(itemInfo);
        ConfirmName.text = itemInfo.Name;
        ConfirmCost.text = (itemInfo.data.buyPrice * ConfirmCount.count).ToString();
        ConfirmButton.onClick.SetListener(() => {
            player.CmdBuyItemsFromMall(category, item, ConfirmCount.count, usingBound);
            item = -1;
        });
    }
    void SetCategories() {
        UIUtils.BalancePrefabs(categorySlotPrefab.gameObject, Storage.data.ItemMallContent.Count, categoryContent);
        ToggleGroup group = categoryContent.GetComponent<ToggleGroup>();
        for(int i = 0; i < Storage.data.ItemMallContent.Count; i++) {
            categoryContent.GetChild(i).GetComponent<UITab>().enabled = true;
            UITab tab = categoryContent.GetChild(i).GetComponent<UITab>();
            tab.SetText(Storage.data.ItemMallContent[i].category);
            tab.group = group;
            group.RegisterToggle(tab);
            int icopy = i; // needed for lambdas, otherwise i is Count
            tab.onLeftClick.AddListener((tabInfo) => {
                category = icopy;// set new category and then scroll to the top again
                ScrollToBeginning();
            });
            if(i == 0) tab.isOn = true;
        }
    }
    void OnEnable() {
        SetCategories();
        WindowCloseButton.onClick.AddListener(() => gameObject.SetActive(false));
        ConfirmCloseOverLay.onClick.AddListener(() => item = -1);
    }
    void OnDisable() {
        WindowCloseButton.onClick.RemoveAllListeners();
        ConfirmCloseOverLay.onClick.RemoveAllListeners();
    }
    void ScrollToBeginning() {
        Canvas.ForceUpdateCanvases();// update first so we don't ignore recently added messages, then scroll
        scrollRect.verticalNormalizedPosition = 1;
    }
}*/