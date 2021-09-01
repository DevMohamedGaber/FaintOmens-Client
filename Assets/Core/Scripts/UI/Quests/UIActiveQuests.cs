using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIActiveQuests : MonoBehaviour {
        public GameObject panal;
        public UIActiveQuest questPrefab;
        public Transform Content;
        public Button Hide;
        public QuestTypes[] types;
        public bool showen = true;
        void Update() {
            Player player = Player.localPlayer;
            if(player) {
                if(panal.activeSelf) {
                    ActiveQuestsHandler(player);
                }
                else panal.SetActive(true);
            }
            else panal.SetActive(false);
        }
        public void ActiveQuestsHandler(Player player) {
            List<Quest> quests = player.own.quests.Where(q => !q.completed).OrderBy(q => q.IsFulfilled()).ThenBy(q => q.type).ThenBy(q => q.progress).ToList();
            if(quests.Count > 0) {
                UIUtils.BalancePrefabs(questPrefab.gameObject, quests.Count, Content);
                for (int i = 0; i < quests.Count; i++) {
                    UIActiveQuest slot = Content.GetChild(i).GetComponent<UIActiveQuest>();
                    Quest quest = quests[i];

                    slot.name.text = quest.data.name.ToString();
                    slot.type.text = types[(int)quest.type].name;
                    slot.type.color = types[(int)quest.type].color;

                    if(player.level < quest.requiredLevel) {
                        slot.status.color = Color.red;
                        slot.status.text = $"Requires player Lvl.{quest.requiredLevel}";
                    } else {
                        bool isFulfilled = quest.IsFulfilled();
                        slot.status.text = quest.GetProgress();
                        slot.status.color = quest.IsFulfilled() ? Color.green : Color.white;
                        slot.btn.onClick.SetListener(() => {
                            if(isFulfilled)
                                player.CmdCompleteQuest(quest.id);
                                // TODO: Check for teleport item before teleporting the player
                        });
                    }
                    
                    slot.teleport.gameObject.SetActive(quest.data.location != new Vector3());
                    if(quest.data.location != new Vector3()) {
                        slot.teleport.onClick.SetListener(() => {
                            player.CmdTeleportToQuestLocation(quest.data.location);
                        });
                    }
                }
            }
        }
        private void Awake() {
            Hide.onClick.SetListener(() => {
                //if(showen) panal.transform.position.x = -550;
                //else panal.transform.position.x = -333;
                //showen = !showen;
            });
        }
    }
    [Serializable]
    public struct QuestTypes {
        public string name;
        public Color color;
    }
}