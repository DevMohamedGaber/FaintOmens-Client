using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Achievements/Honor", order = 0)]
    public class HonorAchievement : ScriptableAchievement
    {
        public int target;
        public bool IsFulfilled(Player player)
        {
            return player.own.TotalHonor >= target;
        }
    }
}