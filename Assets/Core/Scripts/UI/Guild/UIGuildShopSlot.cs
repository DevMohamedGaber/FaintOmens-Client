using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIGuildShopSlot : MonoBehaviour
    {
        public UIInventorySlot item;
        public Text Name;
        public Text cost;
        public Button Buy;
        public Text ReqLevel;
        public Image Cover;
        public void Lock(int lvl) {
            Buy.gameObject.SetActive(false);
            ReqLevel.text = $"Require Guild Lvl.{lvl}";
            Cover.gameObject.SetActive(true);
        }
        public void Unlock() {
            Buy.gameObject.SetActive(true);
            ReqLevel.text = "";
            Cover.gameObject.SetActive(false);
        }
    }
}