using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Pet", order=0)]
    public class PetItem : UsableItem
    {
        [Header("Pet")]
        public ushort petId;
        // usage
        public override bool CanUse()
        {
            return base.CanUse() && player.own.pets.Has(petId) != -1;
        }
        public void Use()
        {
            if(player.own.pets.Has(petId) == -1)
            {
                UIManager.data.pages.pets.Show(petId);
            }
        }
        void OnValidate()
        {
            type = ItemType.PetCard;
        }
    }
}