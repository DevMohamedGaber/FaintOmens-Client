using UnityEngine;
using UnityEngine.UI;
using System;
namespace Game.UI
{
    public class UIDailySign : MonoBehaviour {
        [SerializeField] float updateInterval = .5f;
        [SerializeField] Transform content;
        [SerializeField] GameObject dayPrefab;
        [SerializeField] Transform rewardsContent;
        [SerializeField] GameObject itemSlotPrefab;
        [SerializeField] Button collectButton;
        [SerializeField] GameObject recievedRewardsOverlay;
        [SerializeField] UILanguageDefinerSingle signedCount;
        Player player => Player.localPlayer;
        int currentReward = 0;
        void UpdateData() {
            int monthDays = DateTime.DaysInMonth(Server.time.Year, Server.time.Month);
            int today = Server.time.Day;
            UIUtils.BalancePrefabs(dayPrefab, monthDays, content);
            for(int i = 1; i <= monthDays; i++) {
                UIDailySignDaySlot daySlot = content.GetChild(i - 1).GetComponent<UIDailySignDaySlot>();
                bool isSigned = Check(player.own.dsDays, i);
                daySlot.text.text = i.ToString();
                if(i < today) {
                    daySlot.signed.SetActive(isSigned);
                    daySlot.missed.SetActive(!isSigned);
                }
                else if(i == today) {
                    if(isSigned) {
                        daySlot.signed.SetActive(true);
                        daySlot.button.onClick.RemoveAllListeners();
                    }
                    else {
                        daySlot.button.onClick.SetListener(() => {
                            player.CmdSignInToday();
                            Invoke("CheckNextUnRecievedReward", 1.5f);
                        });
                    }
                }
            }
            signedCount.suffix = $": <color=white>{player.own.dsDays.Count}</color>";
            signedCount.Refresh();
        }
        void CheckNextUnRecievedReward() {
            if(player.own.dsRewards.Count > 0) {
                for(int i = 0; i < Storage.data.dailySignRewards.Length; i++) {
                    if(Check(player.own.dsRewards, i))
                        continue;
                    OnSelectReward(i);
                    return;
                }
                currentReward = Storage.data.dailySignRewards.Length - 1;
            }
            else currentReward = 0;
        }
        void UpdateRewards() {
            DailySignRewards rewards = Storage.data.dailySignRewards[currentReward];
            UIUtils.BalancePrefabs(itemSlotPrefab, rewards.rewards.Length, rewardsContent);
            for(int i = 0; i < rewards.rewards.Length; i++)
                rewardsContent.GetChild(i).GetComponent<UIItemSlot>().Assign(rewards.rewards[i].item, rewards.rewards[i].amount);
            bool isRecieved = Check(player.own.dsRewards, currentReward);
            collectButton.interactable = player.own.dsDays.Count >= rewards.days && !isRecieved;
            recievedRewardsOverlay.SetActive(isRecieved);
        }
        bool Check(SyncListByte list, int number) {
            if(list.Count > 0) {
                for(int i = 0; i < list.Count; i++) {
                    if(list[i] == number)
                        return true;
                }
            }
            return false;
        }
        public void OnSelectReward(int index) {
            if(index < -1 || index >= Storage.data.dailySignRewards.Length) {
                Notifications.list.Add("Unidentified Rewared has been selected", "تم اختيار جائزة غير معرفة");
                return;
            }
            currentReward = index;
            UpdateRewards();
        }
        public void OnRecieveReward() {
            if(currentReward < -1 || currentReward >= Storage.data.dailySignRewards.Length) {
                Notifications.list.Add("Unidentified Rewared has been selected", "تم اختيار جائزة غير معرفة");
                return;
            }
            player.CmdCollectDailySignReward(currentReward);
            Invoke("CheckNextUnRecievedReward", 1.5f);
        }
        void OnEnable() {
            if(player != null) {
                InvokeRepeating("UpdateData", 0, updateInterval);
                CheckNextUnRecievedReward();
            }
            else gameObject.SetActive(false);
        }
        void OnDisable() {
            CancelInvoke("UpdateData");
        }
        /*public GameObject panel;
        public Transform DaysContent;
        public UIDailySignDaySlot DaySlotPrefab;
        int selectedReward = 0;
        public Button[] RewardsButtons;
        public Transform RewardsContent;
        public UIInventorySlot RewardSlotPrefab;
        public Text signedDaysText;
        public Button CollectReward;

        void Update() {
            Player player = Player.localPlayer;
            if(player) {
                if(panel.activeSelf) {
                    DailySign dailySign = player.own.dailySign;
                    //DateTime ServerTime = DateTime.FromOADate(ScheduledEventsHandler.singleton.serverTime);
                    DateTime ServerTime = Server.time;
                    int monthDays = DateTime.DaysInMonth(Server.time.Year, Server.time.Month);
                    int today = Server.time.Day;
                    //days
                    UIUtils.BalancePrefabs(DaySlotPrefab.gameObject, monthDays, DaysContent);
                    for(int i = 0; i < monthDays; i++) {
                        UIDailySignDaySlot daySlot = DaysContent.GetChild(i).GetComponent<UIDailySignDaySlot>();
                        int day = i+1;
                        daySlot.text.text = day.ToString();
                        if(day < today) {
                            if(Array.Exists(dailySign.signedDays, d => d == day)) daySlot.signed.SetActive(true);
                            else daySlot.missed.SetActive(true);
                        }
                        if(day == today && Array.Exists(dailySign.signedDays, d => d == today)) daySlot.signed.SetActive(true);
                        else {
                            daySlot.button.onClick.SetListener(() => {
                                player.CmdSignInToday();
                            });
                        }
                    }
                    //rewards
                    DailySignRewards rewards = Storage.data.dailySignRewards[selectedReward];
                    UIUtils.BalancePrefabs(RewardSlotPrefab.gameObject, rewards.rewards.Length, RewardsContent);
                    for(int i = 0; i < rewards.rewards.Length; i++) {
                        UIInventorySlot slot = RewardsContent.GetChild(i).GetComponent<UIInventorySlot>();
                        ScriptableItemAndAmount reward = rewards.rewards[i];
                        
                    }
                    CollectReward.interactable = dailySign.signedDays.Length >= rewards.daysCount && !Array.Exists(dailySign.recievedRewards, r => r == selectedReward);
                    CollectReward.onClick.SetListener(() => player.CmdCollectDailySignReward(selectedReward));
                    signedDaysText.text = dailySign.signedDays.Length.ToString();
                }
            } 
            else panel.SetActive(false);
        }
        void Awake() {
            for(int i = 0; i < RewardsButtons.Length; i++) {
                int iCopy = i;
                RewardsButtons[i].onClick.SetListener(() => selectedReward = iCopy);
            }
        }*/
    }
}