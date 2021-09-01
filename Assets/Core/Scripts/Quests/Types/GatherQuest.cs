using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Quests/GatherItem", order=0)]
    public class GatherQuest : ScriptableQuest
    {
        [Header("Fulfillment")]
        public Item item;
        public uint amount;
        
        public override string GetProgress(Quest quest)
        {
            if(quest.progress == amount)
            {
                return LanguageManger.Decide("Quest Completed", "تمت المهمة");
            }
            return LanguageManger.Decide($"Gather {item.Name} ({quest.progress}/{amount})",
                                         $"جمع {item.Name} ({quest.progress}/{amount})");
        }
        public override bool IsFulfilled(Quest quest)
        {
            return item.id != 0 && player.InventoryCount(item) >= amount;
        }
    }
}