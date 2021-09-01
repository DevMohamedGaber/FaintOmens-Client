/*using UnityEngine;
using UnityEngine.UI;
using System;
public partial class UIGuildShop : MonoBehaviour {
    public GameObject panel;
    public UIGuildShopSlot guildShopSlot;
    public Transform Content;
    public Text MyContributions;
    public GameObject ConfirmMsg;
    public UIInventorySlot ConfirmSlot;
    public Text ConfirmName;
    public Text ConfirmContributions;
    public InputField ConfirmCount;
    public Button ConfirmIncrese;
    public Button ConfirmDecrese;
    public Button ConfirmButton;
    void Update() {
        if (panel.activeSelf) {
            Player player = Player.localPlayer;
            if(player && player.InGuild()) ShopHandler(player);
        }
        else panel.SetActive(false);
    }
    public void ShopHandler(Player player) {
        GuildMember me = player.guild.myInfo(player.id);
        MyContributions.text = me.contribution.ToString();
        UIUtils.BalancePrefabs(guildShopSlot.gameObject, player.GuildShopItems.Length, Content);
        for(int i = 0; i < player.GuildShopItems.Length; i++) {
            UIGuildShopSlot slot = Content.GetChild(i).GetComponent<UIGuildShopSlot>();
            GuildShopItem item = player.GuildShopItems[i];
            /*slot.item.tooltip.enabled = true;
            slot.item.tooltip.text = item.item.ToolTip();
            slot.item.image.color = Color.white;
            slot.item.image.sprite = item.item.image;
            slot.item.amountOverlay.SetActive(false);
            if(player.guild.Level < item.level) slot.Lock(item.level);
            else slot.Unlock();
            slot.Name.text = item.item.Name;
            slot.cost.text = item.cost.ToString();
            var iCopy = i;
            slot.Buy.onClick.SetListener(() => {
                if(me.contribution >= item.cost) ConfirmBuy(player, me, iCopy);
            });
        }
    }
     private void ConfirmBuy(Player player, GuildMember me, int Index) {
        ConfirmMsg.SetActive(true);
        GuildShopItem item = player.GuildShopItems[Index];
        ConfirmName.text = item.item.Name;
        ConfirmContributions.text = item.cost.ToString();
        ConfirmContributions.color = me.contribution >= (Convert.ToInt32(ConfirmCount.text) * item.cost) ? Color.white : Color.red;
        ConfirmCount.onValueChanged.SetListener((v) => {
            if(Convert.ToInt32(v) < 1)
                ConfirmCount.text = "1";
            ConfirmContributions.text = (Convert.ToInt32(v) * item.cost).ToString();
            ConfirmContributions.color = me.contribution >= (Convert.ToInt32(v) * item.cost) ? Color.white : Color.red;
        });
        ConfirmIncrese.onClick.SetListener(() => {
            ConfirmCount.text = (Convert.ToInt32(ConfirmCount.text) + 1).ToString();
        });
        ConfirmDecrese.onClick.SetListener(() => {
            ConfirmCount.text = Convert.ToInt32(ConfirmCount.text) - 1 > 1 ? (Convert.ToInt32(ConfirmCount.text) - 1).ToString() : "1";
        });
        ConfirmButton.onClick.SetListener(() => {
            if(player.gold >= (Convert.ToInt32(ConfirmCount.text) * item.cost)) 
                player.CmdBuyItemsFromGuildShop(Index, Convert.ToInt32(ConfirmCount.text));
                ConfirmMsg.SetActive(false);
                ConfirmCount.text = "1";
        });
    }

}
*/