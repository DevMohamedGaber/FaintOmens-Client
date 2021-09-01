using UnityEngine;
using UnityEngine.UI;
using System;
namespace Game.UI
{
    public class UIHotEvents : MonoBehaviour {
        /*public GameObject panel;
        public UIHotEventButton buttonPrefab;
        public GameObject NoEvents;
        public GameObject EventPreview;
        public Transform EventsContent;
        public int currentEvent = 0;
        public Text EventPeriod;
        public Text EventDescription;
        public Text EventProgress;
        public Transform objectivesContent;
        public UIHotEventObjective objectivePrefab;
        public UIInventorySlot rewardPrefab;

        public Button GiftsCodeBtn;
        public GameObject GiftsCodePage;
        void Update() {
            Player player = Player.localPlayer;
            if(player) {
                if(panel.activeSelf) {
                    if(currentEvent == -1) {
                        GiftsCodeHandler(player);
                    } else {
                        if(HotEventsSystem.events.Count > 0) {
                            ButtonsHandler(player);
                            SelectedEventHandler(player);
                        } else {
                            EventPreview.SetActive(false);
                            NoEvents.SetActive(true);
                        }
                    }
                }
            }
            else panel.SetActive(false);
        }
        void GiftsCodeHandler(Player player) {

        }
        void ButtonsHandler(Player player) {
            UIUtils.BalancePrefabs(buttonPrefab.gameObject, HotEventsSystem.events.Count, EventsContent);
            for(int i = 0; i < HotEventsSystem.events.Count; i++) {
                UIHotEventButton btn = EventsContent.GetChild(i).GetComponent<UIHotEventButton>();
                btn.name.text = HotEventsSystem.events[i].name;
                int iCopy = i;
                /*btn.button.onClick.SetListener(() => {
                    if(currentEvent == -1) {
                        GiftsCodePage.SetActive(false);
                        EventPreview.SetActive(true);
                    }
                    currentEvent = iCopy;
                    btn.newLabel.SetActive(false);
                });
            }
        }
        void SelectedEventHandler(Player player) {
            HotEvent hotEvent = HotEventsSystem.events[currentEvent];
            HotEventProgress progress = player.own.HotEventsProgress[currentEvent];

            // info
            EventPeriod.text = $"from {DateTime.FromOADate(hotEvent.startsAt)} until {DateTime.FromOADate(hotEvent.endsAt)}";
            EventDescription.text = hotEvent.description;
            EventProgress.text = getEventProgress(player, hotEvent, progress);

            // objectives and rewards
            UIUtils.BalancePrefabs(objectivePrefab.gameObject, hotEvent.objectives.Count, objectivesContent);
            for(int i = 0; i < hotEvent.objectives.Count; i++) {
                UIHotEventObjective objSlot = objectivesContent.GetChild(i).GetComponent<UIHotEventObjective>();
                HotEventObjective objective = hotEvent.objectives[i];
                objSlot.objective.text = getObjectiveInfo(hotEvent, objective);

                UIUtils.BalancePrefabs(rewardPrefab.gameObject, objective.rewards.Count, objSlot.rewardsContent);
                for(int r = 0; r < objective.rewards.Count; r++) {
                    UIInventorySlot slot = objSlot.rewardsContent.GetChild(r).GetComponent<UIInventorySlot>();
                    HotEventReward reward = objective.rewards[r];
                    /*slot.amountOverlay.SetActive(reward.amount > 1);
                    slot.amountText.text = Utils.MinifyLong(reward.amount);
                    slot.image.color = Color.white;
                    if(reward.type == "gold") {
                        slot.tooltip.enabled = false;
                        slot.image.sprite = Storage.data.currencyIcons[0];
                    }
                    else if(reward.type == "diamonds") {
                        slot.tooltip.enabled = false;
                        slot.image.sprite = Storage.data.currencyIcons[1];
                    }
                    else if(reward.type == "b.diamonds") {
                        slot.tooltip.enabled = false;
                        slot.image.sprite = Storage.data.currencyIcons[2];
                    }
                    else{
                        if (ScriptableItem.dict.TryGetValue(reward.type, out ScriptableItem itemData)) {
                            print(itemData.Name);
                            slot.tooltip.enabled = true;
                            slot.image.sprite = itemData.image;
                        }
                    }
                }

                int iCopy = i;
                bool IsFulfilled = HotEventsSystem.IsFulfilled(player, currentEvent, iCopy);
                objSlot.claim.interactable = IsFulfilled;
                objSlot.claim.GetComponentInChildren<Text>().text = !hotEvent.renewable && progress.completeTimes[i] > 0 ? "Claimed" : "claim";
                objSlot.claim.onClick.SetListener(() => {
                    player.CmdClaimHotEventReward(currentEvent, iCopy);
                });
            }
        }
        string getEventProgress(Player player, HotEvent hotEvent, HotEventProgress progress) {
            if(hotEvent.type == HotEventTypes.GatherGold) {
                return $"You have gathered: {progress.progress} Gold";
            }
            else if(hotEvent.type == HotEventTypes.LevelUp) {
                return $"You're currently Level {player.level}";
            }
            else if(hotEvent.type == HotEventTypes.BR) {
                return $"Your Battle Power: {player.battlepower}";
            }
            else if(hotEvent.type == HotEventTypes.GatherHonor) {
                return $"Your Total Honor: {player.own.TotalHonor}, and today: {player.own.TodayHonor}";
            }
            else if(hotEvent.type == HotEventTypes.SpendDiamonds) {
                return $"You've spent: {progress.progress} Diamonds";
            }
            else if(hotEvent.type == HotEventTypes.Parchase) {
                return $"You've parchased: {progress.progress} Diamonds";
            }
            else if(hotEvent.type == HotEventTypes.Kills) {
                return $"You've killed: {progress.progress}";
            }
            else if(hotEvent.type == HotEventTypes.Craft) {
                return $"You've crafted: {progress.progress}";
            }
            else if(hotEvent.type == HotEventTypes.GatherItem) {
                return $"You've gathered: {progress.progress}";
            }
            return "";
        }
        string getObjectiveInfo(HotEvent hotEvent, HotEventObjective objective) {
            if(hotEvent.type == HotEventTypes.GatherGold) {
                return objective.type == "daily" ?
                    $"Gather <b>{objective.amount}</b> Gold daily to recieve:" : $"Reach Total {objective.amount} Gold to recieve:";
            }
            else if(hotEvent.type == HotEventTypes.LevelUp) {
                return $"Reach Level <b>{objective.amount}</b> to recieve:";
            }
            else if(hotEvent.type == HotEventTypes.BR) {
                return $"Reach Battle Power <b>{objective.amount}</b> to recieve:";
            }
            else if(hotEvent.type == HotEventTypes.GatherHonor) {
                return objective.type == "daily" ?
                    $"Reach {objective.amount} Honor daily to recieve:" : $"Reach Total Honor {objective.amount} to recieve:";
            }
            else if(hotEvent.type == HotEventTypes.SpendDiamonds) {
                return objective.type == "daily" ?
                    $"Spend {objective.amount} Diamonds daily to recieve:" : $"Spend Total Diamonds {objective.amount} to recieve:";
            }
            else if(hotEvent.type == HotEventTypes.Parchase) {
                return objective.type == "daily" ?
                    $"Parchase {objective.amount} Diamonds daily to recieve:" : $"Parchase Total Diamonds {objective.amount} to recieve:";
            }
            else if(hotEvent.type == HotEventTypes.Kills) {
                return $"Kill <b>{objective.type}</b>x{objective.amount} to recieve:";
            }
            else if(hotEvent.type == HotEventTypes.Craft) {
                return $"Craft <b>{objective.type}</b>x{objective.amount} to recieve:";
            }
            else if(hotEvent.type == HotEventTypes.GatherItem) {
                return $"Gather <b>{objective.type}</b>x{objective.amount} to recieve:";
            }
            return "";
        }
        void Awake() {
            GiftsCodeBtn.onClick.SetListener(() => {
                GiftsCodePage.SetActive(true);
                EventPreview.SetActive(false);
                currentEvent = -1;
            });
        }*/
    }
}