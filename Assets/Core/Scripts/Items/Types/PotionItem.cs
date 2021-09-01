using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Potion", order=999)]
    public class PotionItem : UsableItemWithCooldown
    {
        [Header("Potion")]
        public int usageHealth;
        public int usageMana;
        public int usageExperience;
        public int usagePetHealth; // to heal pet
        void OnValidate()
        {
            type = ItemType.Potion;
        }
    }
}