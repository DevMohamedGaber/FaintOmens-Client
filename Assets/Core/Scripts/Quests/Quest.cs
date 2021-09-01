using System;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    [Serializable]
    public struct Quest
    {
        public ushort id;
        public QuestType type; // general => 0; daily => 1; guild => 2
        public Quality quality;
        public uint progress;
        public bool completed;

        public ItemSlot[] rewardItems => data.rewardItems;
        public int successor => data.successor != null ? data.successor.name : 0;
        public uint rewardGold => data.rewardGold;
        public ScriptableQuest data
        {
            get
            {
                if (!ScriptableQuest.dict.ContainsKey(id))
                    throw new KeyNotFoundException("There is no ScriptableQuest with hash=" + id + ". Make sure that all ScriptableQuests are in the Resources folder so they are loaded properly.");
                return ScriptableQuest.dict[id];
            }
        }
        public byte requiredLevel => data.requiredLevel;
        public int predecessor => data.predecessor != null ? data.predecessor.name : 0;
        public uint rewardExperience => data.rewardExperience;

        public bool IsFulfilled()
        {
            return data.IsFulfilled(this);
        }
        public string GetProgress()
        {
            return data.GetProgress(this);
        }
    }
}