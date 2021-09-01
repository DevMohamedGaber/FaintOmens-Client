using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Achievements/Popularity", order = 0)]
    public class PopularityAchievement : ScriptableAchievement
    {
        public int target;
        public bool IsFulfilled(Player player)
        {
            return player.own.popularity >= target;
        }
    }
}