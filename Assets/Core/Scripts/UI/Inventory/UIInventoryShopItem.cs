using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIInventoryShopItem : MonoBehaviour {
        public UIItemSlot slot;
        public Text Name;
        public Text cost;
        public Button button;
        public Text reqLvl;
        public GameObject redOverlay;
    }
}