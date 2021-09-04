using UnityEngine;
namespace Game.UI
{
    public class UIShopItem : MonoBehaviour
    {
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] UnityEngine.UI.Image currency;
        [SerializeField] TMPro.TMP_Text costTxt;
        public UIBasicButton button;
        public void Set(ShopItem item)
        {
            slot.Assign(item.item);
            nameTxt.text = item.item.item.Name;
            currency.sprite = UIManager.data.assets.currency[(int)item.currency];
            costTxt.text = item.cost.ToString();
        }
        void Awake() {
            if(button == null)
            {
                UIBasicButton buttonScript = transform.GetComponent<UIBasicButton>();
                if(buttonScript != null)
                {
                    button = buttonScript;
                }
                else
                {
                    Debug.Log("UIBasicButton not found");
                }
            }
        }
    }
}