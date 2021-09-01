using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Points/Gold", order=0)]
    public class GoldBag : UsableItem
    {
        public uint gold = 1;
    }
}