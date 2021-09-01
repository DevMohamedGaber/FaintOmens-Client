using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIQuestsWindow : MonoBehaviour {
        public GameObject panal;
        public UIQuestCollapsableItem[] QuestsTypes;
        public UIQuestWindowItem questPrefab;
        public GameObject InfoDisplay;
        public QuestType type;

        void Update() {
            Player player = Player.localPlayer;
            if(player) {
                if(panal.activeSelf) {
                    QuestsHandler(player);
                }
            }
            else panal.SetActive(false);
        }
        protected void QuestsHandler(Player player) {
            List<Quest> quests = player.own.quests.Where(q => q.type == type).OrderBy(q => q.completed).ThenBy(q => q.progress).ToList();
            QuestsTypes[(int)type].count.text = quests.Count.ToString();
            if(quests.Count > 0) {
                UIUtils.BalancePrefabs(questPrefab.gameObject, quests.Count, QuestsTypes[(int)type].Content);
                for(int i = 0; i < quests.Count; i++) {
                    UIQuestWindowItem slot = QuestsTypes[(int)type].Content.GetChild(i).GetComponent<UIQuestWindowItem>();
                    Quest quest = quests[i];
                    slot.Name.text = quest.data.name.ToString();
                    if(player.level < quest.requiredLevel) {
                        slot.status.color = Color.red;
                        slot.status.text = $"Lvl.{quest.requiredLevel}";
                    } else if(quest.IsFulfilled()) {
                        slot.status.color = Color.green;
                        slot.status.text = "Completed";
                    } else {
                        slot.status.color = Color.white;
                        slot.status.text = quest.data.GetProgress(quest);
                    }
                }
            }
        }
        private void Awake() {
            for(int i = 0; i < QuestsTypes.Length; i++) {
                int iCopy = i;
                QuestsTypes[i].header.onClick.SetListener(() => {
                    for(int q = 0; q < QuestsTypes.Length; q++) QuestsTypes[q].panal.SetActive(false);
                    QuestsTypes[iCopy].panal.SetActive(true);
                    type = (QuestType)iCopy;
                });
            }
        }
    }
}