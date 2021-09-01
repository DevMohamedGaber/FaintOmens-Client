using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Quests/KillMonsters", order=0)]
    public class KillMonstersQuest : ScriptableQuest
    {
        [Header("Fulfillment")]
        public Monster monster;
        public uint amount;
        public override string GetProgress(Quest quest)
        {
            if(quest.progress == amount)
            {
                return LanguageManger.Decide("Quest Completed", "تمت المهمة");
            }
            return LanguageManger.Decide($"Kill {monster.Name} ({quest.progress}/{amount})",
                                         $"اقتل {monster.Name} ({quest.progress}/{amount})");
        }
        public override bool IsFulfilled(Quest quest)
        {
            return quest.progress >= amount;
        }
    }
}