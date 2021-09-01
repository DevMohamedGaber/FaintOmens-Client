using UnityEngine;
using UnityEngine.UI;
using System;
namespace Game.UI
{
    public partial class UIInventoryShop : MonoBehaviour {
        /*[SerializeField] private UIInventoryShopItem prefab;
        [SerializeField] private Transform Content;
        [SerializeField] private Button WindowCloseOverLay;
        [SerializeField] private GameObject ConfirmMsg;
        [SerializeField] private UIItemSlot ConfirmSlot;
        [SerializeField] private Text ConfirmName;
        [SerializeField] private Text ConfirmGold;
        [SerializeField] private UICountSwitch ConfirmCount;
        [SerializeField] private Button ConfirmButton;
        [SerializeField] private Button ConfirmCloseOverLay;
        
        private Player player => Player.localPlayer;
        int selected = -1;
        void Update() {
            if(player != null) {
                UIUtils.BalancePrefabs(prefab.gameObject, Storage.data.inventoryShopItems.Length, Content);
                for(int i = 0; i < Storage.data.inventoryShopItems.Length; i++) {
                    UIInventoryShopItem slot = Content.GetChild(i).GetComponent<UIInventoryShopItem>();
                    Item item = Storage.data.inventoryShopItems[i];
                    bool isBuyable = player.level >= item.data.minLevel;
                    bool hasGold = player.own.gold >= item.data.buyPrice;
                    slot.slot.Assign(item);
                    slot.Name.text = item.Name.ToString();
                    slot.cost.text = item.data.buyPrice.ToString();
                    slot.cost.color = hasGold ? Color.white : Color.red;
                    slot.reqLvl.text = !isBuyable ? $"Req Lvl.{item.data.minLevel}" : "";
                    slot.redOverlay.SetActive(!isBuyable);
                    int iCopy = i;
                    slot.button.onClick.SetListener(() => {
                        if(hasGold && isBuyable) selected = iCopy;
                    });
                }
                if(selected > -1 && selected < Storage.data.inventoryShopItems.Length) ShowConfirmBuy();
                else ConfirmMsg.SetActive(false);
            }
            else gameObject.SetActive(false);
        }
        void ShowConfirmBuy() {
            ConfirmMsg.SetActive(true);
            Item item = Storage.data.inventoryShopItems[selected];
            ConfirmCount.Limits(1, (int)Math.Round((double)(player.own.gold / item.data.buyPrice)));
            ConfirmSlot.Assign(item);
            ConfirmName.text = item.data.name.ToString();
            ConfirmGold.text = (item.data.buyPrice * ConfirmCount.count).ToString();
            ConfirmButton.onClick.SetListener(OnBuy);
        }
        void OnBuy() {
            player.CmdBuyItemsFromInventoryShop(selected, ConfirmCount.count);
            ReSetSelection();
        }
        void ReSetSelection() => selected = -1;
        void OnEnable() {
            ConfirmCloseOverLay.onClick.AddListener(ReSetSelection);
            WindowCloseOverLay.onClick.AddListener(() => gameObject.SetActive(false));
        } 
        void OnDisable() {
            ConfirmCloseOverLay.onClick.RemoveAllListeners();
            WindowCloseOverLay.onClick.RemoveAllListeners();
        }*/
    }
}