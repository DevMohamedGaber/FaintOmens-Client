using UnityEngine;
namespace Game.UI
{
    public class MountPage : MonoBehaviour
    {
        [SerializeField] protected MountsList mounts;
        protected ushort id;
        protected Player player => Player.localPlayer;
        public virtual void Refresh()
        {
            id = mounts.sId;
        }
    }
}