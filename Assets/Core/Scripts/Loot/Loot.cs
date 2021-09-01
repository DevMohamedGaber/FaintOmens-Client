using UnityEngine;
using Mirror;
namespace Game
{
    public class Loot : NetworkBehaviourNonAlloc
    {
        [SyncVar] public int owner;
        [SyncVar] public ItemSlot item;
        public bool IsEmpty()
        {
            return item.amount == 0 || item.item.id == 0;
        }
        public override void OnStartClient()
        {
            // set effect baised on the highest quality item in the loot
        }
        [Command] public void CmdClaim(uint playerId) {}
    }
}