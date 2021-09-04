using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Game.UI
{
    public class UIShopItem : MonoBehaviour
    {
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] Image currency;
        [SerializeField] TMP_Text costTxt;
        [SerializeField] GameObject discountObj;
        [SerializeField] TMP_Text discountTxt;
        [SerializeField] Image discountImage;
        public UIBasicButton button;
        public void Set(ShopItem item, float discount = 0f)
        {
            slot.Assign(item.item);
            nameTxt.text = item.item.item.Name;
            currency.sprite = UIManager.data.assets.currency[(int)item.currency];
            
            discountObj.SetActive(discount > 0f);
            if(discount > 0f)
            {
                discountImage.sprite = UIManager.data.assets.discountArrows[discount >= .5 ? 1 : 0];
                discountTxt.text = discount.ToString("F0") + "%";
                costTxt.text = ((int)System.Math.Ceiling(item.cost - (item.cost * discount))).ToString();
            }
            else
            {
                costTxt.text = item.cost.ToString();
            }
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