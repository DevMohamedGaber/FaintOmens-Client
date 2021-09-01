using UnityEngine;
namespace Game.UI
{
    public class UIArena_Page : MonoBehaviour
    {
        protected Player player => Player.localPlayer;
        public virtual void Refresh() {}
    }
}