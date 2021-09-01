using UnityEngine;
namespace Game.UI
{
    public class UIPetPage : MonoBehaviour {
        [SerializeField] protected UIPetsList pets;
        protected ushort id;
        public virtual void Refresh() {
            id = pets.sId;
        }
    }
}