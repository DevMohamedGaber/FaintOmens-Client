using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
namespace Game
{
    public class Npc : NetworkBehaviourNonAlloc
    {
        [Header("Components")]
        public TextMeshPro questOverlay;

        [Header("Welcome Text")]
        [TextArea(1, 30)] public string welcome;

        [Header("Quests")]
        public ScriptableQuestOffer[] quests;
        [Header("Teleportation")]
        public TeleportNPCOffer[] teleports;

        [Header("Booth")]
        public string boothName;
        public GameObject BoothGameObject;
        //public override void ResetMovement() => agent.ResetMovement();
        public override void OnStartClient() {
            if (questOverlay != null) {
                // find local player (null while in character selection)
                if (Player.localPlayer != null) {
                    if (CanPlayerCompleteAnyQuestHere(Player.localPlayer))
                        questOverlay.text = "!";
                    else if (CanPlayerAcceptAnyQuestHere(Player.localPlayer))
                        questOverlay.text = "?";
                    else
                        questOverlay.text = "";
                }
            }
        }
        public bool CanPlayerCompleteAnyQuestHere(Player player) {
            foreach (ScriptableQuestOffer entry in quests)
                if (entry.completeHere && player.CanCompleteQuest(entry.quest.name))
                    return true;
            return false;
        }
        public bool CanPlayerAcceptAnyQuestHere(Player player) {
            foreach (ScriptableQuestOffer entry in quests)
                if (entry.acceptHere && player.CanAcceptQuest(entry.quest))
                    return true;
            return false;
        }
        public List<ScriptableQuest> QuestsVisibleFor(Player player) {
            List<ScriptableQuest> visibleQuests = new List<ScriptableQuest>();
            foreach (ScriptableQuestOffer entry in quests)
                if (entry.acceptHere && player.CanAcceptQuest(entry.quest) ||
                    entry.completeHere && player.HasActiveQuest(entry.quest.name))
                    visibleQuests.Add(entry.quest);
            return visibleQuests;
        }
    }
    [Serializable] public partial struct TeleportNPCOffer {
        public int city;
        public long cost;
        public TeleportNPCOffer(int city = -1, int cost = 0) {
            this.city = city;
            this.cost = cost;
        }
    }
    [Serializable] public class ScriptableQuestOffer {
        public ScriptableQuest quest;
        public bool acceptHere = true;
        public bool completeHere = true;
    }
}