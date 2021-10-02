using UnityEngine;
namespace Game.UI
{
    public class MountStats : Stats
    {
        public void SetWithNextTireBonus(Mount data)
        {
            bool isMax = data.tier >= data.data.maxTier;
            //hp.Set(data.healthMax, (data.data.health.Get((int)data.level) / 5) * data.qualityBonus);
        }
        public void SetWithNextStarBonus(Mount data)
        {
            
        }
        public void Set(Mount data)
        {
            hp.Set(data.healthMax);
        }
        public void Set(ScriptableMount data)
        {
            hp.Set(data.health.Get(1));
        }
    }
}