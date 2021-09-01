using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Game.UI
{
    public class UICountableItemSlot : UIItemSlot {
        uint max;
        public void Assign(Item item, uint amount, uint max) {
            if(item.id < 1)
                return;
            Unassign();
            data = item;
            SetIcon(data.data.image);
            SetAmount(amount, max);
            SetPlus(data.plus);
            quality.SetFrame(data.quality.current);
            bound.SetActive(data.bound);
        }
        public void SetAmount(uint amount, uint max) {
            this.amount = amount;
            this.max = max;
            amountText.text = $"{amount}/{max}";
            amountText.color = amount >= max ? Color.white : Color.red;
        }
    }
}