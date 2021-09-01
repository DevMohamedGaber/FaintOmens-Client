using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Clothing", order=999)]
    public class ClothingItem : UsableItem
    {
        [Header("Clothing")]
        public ClothingCategory equipCategory;
        public ushort wardropId;
        void OnValidate()
        {
            type = ItemType.Clothing;
        }
    }
}
