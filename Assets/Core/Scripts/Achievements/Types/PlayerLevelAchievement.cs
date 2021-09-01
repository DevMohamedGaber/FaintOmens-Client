using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Achievements/PlayerLevel", order = 0)]
    public class PlayerLevelAchievement : ScriptableAchievement
    {
        public byte target;
        public bool IsFulfilled(Player player)
        {
            return player.level >= target;
        }
    }
}