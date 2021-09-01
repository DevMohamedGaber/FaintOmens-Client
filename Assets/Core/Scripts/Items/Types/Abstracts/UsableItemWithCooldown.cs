using UnityEngine;
namespace Game
{
    public abstract class UsableItemWithCooldown : UsableItem
    {
        [Header("Cooldown")]
        public float cooldown; // potion usage interval, etc.
        [Tooltip("Cooldown category can be used if different potion items should share the same cooldown. Cooldown applies only to this item name if empty.")]
    #pragma warning disable CS0649 // Field never assigned to
        [SerializeField] string _cooldownCategory; // leave empty for itemname based cooldown. fill in for category.
    #pragma warning restore CS0649 // Field never assigned to
        public string cooldownCategory =>
            // defaults to per-item-name cooldown if empty. otherwise category.
            string.IsNullOrWhiteSpace(_cooldownCategory) ? name.ToString() : _cooldownCategory;

        // usage ///////////////////////////////////////////////////////////////////
        // [Server] and [Client] CanUse check for UI, Commands, etc.
        public virtual bool CanUse(Player player, int inventoryIndex)
        {
            // check level etc. and make sure that cooldown buff elapsed (if any)
            return player.level >= minLevel && player.GetItemCooldown(cooldownCategory) == 0;
        }
        // [Client] OnUse Rpc callback for effects, sounds, etc.
        // -> can't pass slotIndex because .Use might clear it before getting here already
        public virtual void OnUsed(Player player) {}
    }
}
