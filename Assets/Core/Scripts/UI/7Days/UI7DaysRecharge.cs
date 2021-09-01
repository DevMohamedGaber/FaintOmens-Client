using UnityEngine;
using UnityEngine.UI;
using System;
namespace Game.UI
{
    public class UI7DaysRecharge : MonoBehaviour
    {
        /*public GameObject panel;
        public Transform Content;
        public UIInventorySlot RewardSlotPrefab;
        void Update() {
            Player player = Player.localPlayer;
            if (player) {
                if(panel.activeSelf && DateTime.FromOADate(player.own.createdAt).AddDays(7).Day >= Server.time.Day) {
                    for(int i = 0; i < 7; i++) {
                        UIObjectiveRewardCollectBar infobar = Content.GetChild(i).GetComponent<UIObjectiveRewardCollectBar>();
                        First7DaysEventRewards info = Storage.data.Recharge7DaysEventsRewards[i];
                        infobar.objective.text = $"Recharge in day {info.day} before 12:00 AM to obtain:";
                        UIUtils.BalancePrefabs(RewardSlotPrefab.gameObject, info.rewards.Length, infobar.content);
                        for(int r = 0; r < info.rewards.Length; r++) {
                            UIInventorySlot slot = infobar.content.GetChild(r).GetComponent<UIInventorySlot>();
                            ScriptableItemAndAmount reward = info.rewards[r];
                            /*slot.image.color = Color.white;
                            slot.image.sprite = reward.item.image;
                            slot.amountOverlay.SetActive(reward.amount > 1);
                            slot.amountText.text = reward.amount.ToString();
                        }
                        bool collected = Array.Exists(player.own._7days.recharge, s => s == info.day);
                        infobar.colleted.SetActive(collected);
                        infobar.collect.interactable = !collected && Server.time.Day >= DateTime.FromOADate(player.own.createdAt).AddDays(info.day).Day && player.VIP.todayRecharge > 0;
                        if(!collected) {
                            infobar.collect.onClick.SetListener(() => player.CmdCollect7DaysRechargeReward(info.day));
                        }
                    }
                }
            }
            else panel.SetActive(false);
        }*/
    }
}