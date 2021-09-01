using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Achievements/UsedGold", order = 0)]
    public class UsedGoldAchievement : ScriptableAchievement
    {
        public ulong target;
        public bool IsFulfilled(Player player)
        {
            return player.own.archive.usedGold >= target;
        }
    }
}