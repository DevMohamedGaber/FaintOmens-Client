using UnityEngine;
namespace Game.UI
{
    public class PetStats : Stats
    {
        public void SetWithNextTireBonus(PetInfo pet)
        {
            bool isMax = pet.tier >= pet.data.maxTier;
            hp.Set(pet.healthMax, (pet.data.health.Get((int)pet.level) / 5) * pet.qualityBonus);
        }
        public void SetWithNextStarBonus(PetInfo pet)
        {
            
        }
        public void Set(PetInfo pet)
        {
            hp.Set(pet.healthMax);
        }
        public void Set(ScriptablePet pet)
        {
            hp.Set(pet.health.Get(1));
        }
    }
}