using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;
public class UIMarketItems : MonoBehaviour {
    /*public GameObject panel;
    public ScrollRect scrollRect;
    // navigate
    int currentCategory = 0; // by default 0 => the (All) category
    public bool updated = false;
    // items list
    public UIMarketBuySlot ItemSlotPrefab;
    public Transform Content;
    public GameObject noItems;
    public Text MyDiamonds;

    void Update() {
        if(panel.activeSelf) {
            Player player = Player.localPlayer;
            if(player) {
                if(!updated) UpdateItems();
                MyDiamonds.text = player.own.diamonds.ToString();
                if(player.Market.Count > 0) ListHandler(player);
                else noItems.SetActive(true);
            }
        }
    }
    protected void ListHandler(Player player) {
        noItems.SetActive(false);
        UIUtils.BalancePrefabs(ItemSlotPrefab.gameObject, player.Market.Count, Content);
        for (int i = 0; i < player.Market.Count; i++) {
            UIMarketBuySlot slot = Content.GetChild(i).GetComponent<UIMarketBuySlot>();
            MarketItemClient auctionitem = player.Market[i];
            /* item slot
            slot.item.tooltip.enabled = true;
            slot.item.tooltip.text = auctionitem.item.item.ToolTip();
            slot.item.image.color = Color.white;
            slot.item.image.sprite = auctionitem.item.item.image;
            slot.item.amountOverlay.SetActive(auctionitem.item.amount > 1);
            slot.item.amountText.text = auctionitem.item.amount.ToString();
            slot.item.setPlus(auctionitem.item.item.plus);
            // info
            slot.itemName.text = auctionitem.item.item.data.name.ToString();
            //slot.offetTime.text = Utils.TimeFromNowShorted(auctionitem.endtime);
            slot.buyPrice.text = auctionitem.price.ToString();
            slot.Buy.onClick.SetListener(() => {
                if(auctionitem.sellerId != player.id) {
                    //player.CmdBuyMarketItem(auctionitem.id);
                    Destroy(slot);
                }
            });
        }
    }
    protected void UpdateItems() {
        Player.localPlayer.CmdGetMarketItems();
        updated = true;
    }
    void Awake() => UpdateItems();
    void ScrollToBeginning() {
        Canvas.ForceUpdateCanvases();// update first so we don't ignore recently added messages, then scroll
        scrollRect.verticalNormalizedPosition = 1;
    }
    private void OnDisable() {
        updated = false;
    }*/
}