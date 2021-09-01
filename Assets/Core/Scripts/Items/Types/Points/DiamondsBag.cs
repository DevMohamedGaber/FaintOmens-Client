using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Points/Diamonds", order=0)]
    public class DiamondsBag : UsableItem
    {
        public uint diamonds = 1;
        public bool bound;
    }
}