using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Points/Honor", order=0)]
    public class HonorBadge : UsableItem
    {
        public uint honor = 1;
    }
}