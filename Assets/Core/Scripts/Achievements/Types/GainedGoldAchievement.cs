using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Achievements/GainedGold", order = 0)]
    public class GainedGoldAchievement : ScriptableAchievement
    {
        public ulong target;
        public bool IsFulfilled(Player player)
        {
            return player.own.archive.gainedGold >= target;
        }
    }
}