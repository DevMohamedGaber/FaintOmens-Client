using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Points/Exp", order=0)]
    public class ExpOrb : UsableItem
    {
        public uint experience;
        // usage
        public override bool CanUse()
        {
            return base.CanUse() && player.level < Storage.data.player.maxLevel;
        }
    }
}