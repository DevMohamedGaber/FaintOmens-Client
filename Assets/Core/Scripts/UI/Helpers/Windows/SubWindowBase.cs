using UnityEngine;
namespace Game.UI
{
    public class SubWindowBase : MonoBehaviour
    {
        protected Player player => Player.localPlayer;
        public bool isVisible => gameObject.activeSelf;
        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
        public virtual void Refresh() {}
        protected virtual void OnEnable()
        {
            if(player == null)
            {
                UIManager.data.OnLocalPlayerNotFound();
            }
        }
    }
}