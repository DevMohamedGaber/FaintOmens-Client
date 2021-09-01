using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Mount", order=0)]
    public class MountItem : UsableItem
    {
        [Header("Mount")]
        public ushort mountId;
        // usage
        public override bool CanUse()
        {
            return base.CanUse() && player.own.mounts.Has(mountId)!= -1;
        }
        public void Use()
        {
            if(player.own.mounts.Has(mountId) == -1)
            {
                UIManager.data.pages.mounts.Show(mountId);
            }
        }
        void OnValidate()
        {
            type = ItemType.MountCard;
        }
    }
}