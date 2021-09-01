using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Achievements/AchievementPoints", order = 0)]
    public class AchievementPointsAchievement : ScriptableAchievement
    {
        public short target;
        public bool IsFulfilled(Player player)
        {
            return player.own.archive.achievementPoints >= target;
        }
    }
}