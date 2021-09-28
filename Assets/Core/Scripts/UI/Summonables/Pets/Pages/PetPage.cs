using UnityEngine;
namespace Game.UI
{
    public class PetPage : MonoBehaviour
    {
        [SerializeField] protected PetsList pets;
        protected ushort id;
        protected Player player => Player.localPlayer;
        public virtual void Refresh()
        {
            id = pets.sId;
        }
    }
}