/* - MODIFICATIONS TO THE MAIN CLASS -
- remove the main class
*/
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIMallItem : MonoBehaviour {
        public UIItemSlot itemSlot;
        public Text Name;
        public Image currencyIcon;
        public Text cost;
        public Button button;
    }
}