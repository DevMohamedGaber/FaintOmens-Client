using UnityEngine;
namespace Game.UI
{
    public class Window : MonoBehaviour
    {
        protected Player player => Player.localPlayer;
        public bool isVisible => gameObject.activeSelf;
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        public virtual void Close(bool remember = false)
        {
            gameObject.SetActive(false);
            if(remember)
            {
                UIManager.data.history.Push(this);
                if(UIManager.data.history.Count > 0)
                {
                    UIManager.data.history.Pop().Show();
                }
            }
        } 
        public virtual void Refresh() {}
        public virtual void UpdateCurrency() {}
        protected virtual void OnEnable()
        {
            if(player)
            {
                if(UIManager.data.currenOpenWindow != null)
                {
                    UIManager.data.currenOpenWindow.Close();
                }
                UIManager.data.currenOpenWindow = this;
                UpdateCurrency();
            }
            else
            {
                Close();
                UIManager.data.OnLocalPlayerNotFound();
            }
        }
        protected virtual void OnDisable()
        {
            UIManager.data.currenOpenWindow = null;
        }
    }
}