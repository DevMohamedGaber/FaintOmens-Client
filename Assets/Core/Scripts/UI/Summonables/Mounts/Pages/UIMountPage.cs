using UnityEngine;
namespace Game.UI
{
    public class UIMountPage : MonoBehaviour {
        [SerializeField] protected UIMountsList mounts;
        protected ushort id;
        public virtual void Refresh() {
            id = mounts.sId;
        }
    }
}