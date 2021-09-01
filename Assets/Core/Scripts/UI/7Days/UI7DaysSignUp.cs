using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
namespace Game.UI
{
    public class UI7DaysSignUp : MonoBehaviour
    {
        [SerializeField] float updateInterval = .5f;
        [SerializeField] ItemSlotArray[] rewards;
        [SerializeField] UI7DaysReward[] list;
        [SerializeField] int[] signed;
        [SerializeField] GameObject itemPrefab;
        Player player => Player.localPlayer;
        void UpdateData() {
            int firstDay = DateTime.FromOADate(player.own.createdAt).Day;
            for(int i = 0; i < 7; i++) {
                bool collected = IsCollected(i + 1);
                list[i].collect.interactable = firstDay + i <= Server.time.Day && !collected;
                list[i].colleted.SetActive(collected);
            }
        }
        bool IsCollected(int day) {
            if(signed.Length > 0) {
                for(int i = 0; i < signed.Length; i++)
                    if(signed[i] == day)
                        return true;
            }
            return false;
        }
        public void Set(int[] signedList) {
            signed = signedList;
            UpdateData();
        }
        public void OnCollect(int day) {
            if(day < 1 || day > 7) {
                UINotifications.list.Add("UnKnowen reward has been selected", "تم اختيار جائزة غير معروفة");
                return;
            }
            player.CmdCollect7DaysSignUpReward(day);
        }
        void OnEnable() {
            if(player != null && DateTime.FromOADate(player.own.createdAt).AddDays(7) >= Server.time) {
                player.CmdGet7DaysSignedDays();
            }
            else gameObject.SetActive(false);
        }
        void Awake() {
            for(int i = 0; i < 7; i++) {
                if(rewards[i].items.Length < 1) continue;
                UIUtils.BalancePrefabs(itemPrefab, rewards[i].items.Length, list[i].content);
                for(int r = 0; r < rewards[i].items.Length; r++)
                    list[i].content.GetChild(r).GetComponent<UIItemSlot>().Assign(rewards[i].items[r].item, rewards[i].items[r].amount);
            }
        }
        /*public GameObject panel;
        public Transform Content;
        public UIInventorySlot RewardSlotPrefab;
        void Update() {
            Player player = Player.localPlayer;
            if (player) {
                if(panel.activeSelf && DateTime.FromOADate(player.own.createdAt).AddDays(7).Day >= Server.time.Day) {
                    for(int i = 0; i < 7; i++) {
                        UIObjectiveRewardCollectBar infobar = Content.GetChild(i).GetComponent<UIObjectiveRewardCollectBar>();
                        First7DaysEventRewards info = Storage.data.SignUp7DaysEventsRewards[i];
                        infobar.objective.text = $"Sign Up for {info.day} days to obtain:";
                        UIUtils.BalancePrefabs(RewardSlotPrefab.gameObject, info.rewards.Length, infobar.content);
                        for(int r = 0; r < info.rewards.Length; r++) {
                            UIInventorySlot slot = infobar.content.GetChild(r).GetComponent<UIInventorySlot>();
                            ScriptableItemAndAmount reward = info.rewards[r];
                            /*slot.image.color = Color.white;
                            slot.image.sprite = reward.item.image;
                            slot.amountOverlay.SetActive(reward.amount > 1);
                            slot.amountText.text = reward.amount.ToString();
                        }
                        bool collected = Array.Exists(player.own._7days.signup, s => s == info.day);
                        infobar.colleted.SetActive(collected);
                        infobar.collect.interactable = !collected && Server.time.Day >= DateTime.FromOADate(player.own.createdAt).AddDays(info.day).Day;
                        if(!collected) {
                            infobar.collect.onClick.SetListener(() => player.CmdCollect7DaysSignUpReward(info.day));
                        }
                    }
                    
                }
            }
            else panel.SetActive(false);
        }*/
    }
}