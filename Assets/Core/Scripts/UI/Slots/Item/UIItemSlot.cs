using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
namespace Game.UI
{
    public class UIItemSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Image icon;
        [SerializeField] protected TMP_Text amountText;
        [SerializeField] protected UIQualityController quality;
        [SerializeField] TMP_Text plusText;
        [SerializeField] protected GameObject bound;
        [SerializeField] BasicButton unlockBtn;
        public OnClickEvent onClick = new OnClickEvent();
        public OnDoubleClickEvent onDoubleClick = new OnDoubleClickEvent();
        public Item data;
        public uint amount;
        public int ID;
        public bool IsAssigned() => data.id > 0;
        public bool isLocked => unlockBtn.gameObject.activeSelf;
        protected void SetData(Item item, uint amount) {
            data = item;
            if(item.id > 0) {
                SetIcon(data.data.image);
                SetAmount(amount);
                SetPlus(data.plus);
                quality.SetFrame(data.quality.current);
                bound.SetActive(data.bound);
            }
        }
        public void Assign(Item item, uint amount = 1) {
            Unassign();
            SetData(item, amount);
        }
        public void Assign(ItemSlot slot, int id = -1) {
            this.Assign(slot.item, slot.amount);
            ID = id;
        }
        public void Assign(int itemId, uint amount = 1) {
            data.Reset((ushort)itemId);
            this.Assign(data, amount);
        }
        public void Assign(UIItemSlot slot) {
            this.Assign(slot.data, slot.amount);
            ID = slot.ID;
        }
        public void Unassign() {
            SetIcon();
            SetAmount();
            SetPlus();
            bound.SetActive(false);
            data.Reset();
            ID = -1;
            onClick.RemoveAllListeners();
            onDoubleClick.RemoveAllListeners();
            Unlock();
        }
        public void SetAmount(uint amount = 0) {
            this.amount = amount;
            if(amountText != null)
                amountText.text = amount > 1 ? amount.ToString() : "";
        }
        public void SetIcon(Sprite image = null) {
            icon.gameObject.SetActive(image != null);
            if(image != null && icon != null)
                icon.sprite = image;
        }
        protected void SetPlus(int plus = 0) {
            if(plusText != null)
                plusText.text = plus > 0 ? $"+{plus}" : "";
        }
        public virtual void OnPointerClick(PointerEventData eventData) {
            if(eventData.clickCount == 1 && onClick != null)
                onClick.Invoke(this);
            else if(eventData.clickCount == 2 && onDoubleClick != null) {
                eventData.clickCount = 0;
                onDoubleClick.Invoke(this);
            }
        }
        public void Lock(UnityAction onUnLock = null) {
            Unassign();
            unlockBtn.gameObject.SetActive(true);
            unlockBtn.onClick = onUnLock;
        }
        void Unlock() {
            if(isLocked) {
                unlockBtn.gameObject.SetActive(false);
                unlockBtn.onClick = null;
            }
        }
        void Awake() {
            quality.gameObject.SetActive(true);
            if(IsAssigned())
                Assign(data);
            else Unassign();
        }
        [System.Serializable] public class OnClickEvent : UnityEvent<UIItemSlot> { }
        [System.Serializable] public class OnDoubleClickEvent : UnityEvent<UIItemSlot> { }
    }
}