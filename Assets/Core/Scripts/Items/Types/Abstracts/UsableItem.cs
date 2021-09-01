using UnityEngine;
namespace Game
{
    public abstract class UsableItem : ScriptableItem
    {
        public Player player => Player.localPlayer;
        public virtual bool CanUse() => player.level >= minLevel;

        // [Client] OnUse Rpc callback for effects, sounds, etc.
        // -> can't pass slotIndex because .Use might clear it before getting here already
        public virtual void OnUsed() {}
    }
}
