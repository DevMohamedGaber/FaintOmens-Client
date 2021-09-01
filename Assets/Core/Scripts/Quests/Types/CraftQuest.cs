using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Quests/Craft", order=999)]
    public class CraftQuest : ScriptableQuest
    {
        [Header("Fulfillment")]
        public ScriptableItem item;
        public uint amount;
        public override string GetProgress(Quest quest)
        {
            if(quest.progress == amount)
            {
                return LanguageManger.Decide("Quest Completed", "تمت المهمة");
            }
            return LanguageManger.Decide($"Craft {item.Name} ({quest.progress}/{amount})",
                                         $"جمع {item.Name} ({quest.progress}/{amount})");
        }

        public override bool IsFulfilled(Quest quest)
        {
            return item != null && quest.progress == amount;
        }
    }
}