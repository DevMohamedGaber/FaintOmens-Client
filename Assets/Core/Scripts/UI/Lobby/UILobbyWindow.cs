using UnityEngine;
using Game.Network;
namespace Game.UI
{
    public class UILobbyWindow : MonoBehaviour {
        [SerializeField] protected NetworkManagerMMO networkManager;
        public virtual void OnError(Game.Network.NetworkError error) {}
        public virtual void OnDisconnect(string msg = "") {}
        
        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Close() => gameObject.SetActive(false);
        protected virtual void OnEnable() {
            if(UIManager.data.lobby.current != null)
                UIManager.data.lobby.current.Close();
            UIManager.data.lobby.current = this;
        }
        protected virtual void OnDisable() {
            UIManager.data.lobby.current = null;
        }
    }
}