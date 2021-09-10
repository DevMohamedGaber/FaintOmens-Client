using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
namespace Game.UI
{
    public class UIInventorySlot : UIItemSlot {
        [SerializeField] Button unlockBtn;
        public void Lock(UnityAction onUnLock) {
            Unassign();
            Button lockBtn = transform.Find("Locked").GetComponent<Button>();
            unlockBtn.gameObject.SetActive(true);
            unlockBtn.onClick.SetListener(onUnLock);
        }
        public override void OnPointerClick(PointerEventData eventData) {
            if(IsAssigned() && amount > 0)
                UIManager.data.inScene.tooltip.Show(data, ToolTipFrom.Inventory, ID);
                //ToolTip.data.ShowItem(data, transform.position, true, ID);
            /*if(this.m_ItemInfo != null && this.m_ItemInfo.Value.amount > 0) {
                ToolTip.data.onUse = () => {
                    if(this.m_ItemInfo.Value.item.data is UsableItem &&
                        ((UsableItem)m_ItemInfo.Value.item.data).CanUse(Player.localPlayer, ID))
                        Player.localPlayer.CmdUseInventoryItem(ID);
                };
                ToolTip.data.ShowItem(m_ItemInfo.Value.item, transform.position, true);
                Debug.Log("show tooltip from UIInventorySlot");
            }*/
        }
        /*public override void OnPointerExit(PointerEventData eventData) {
            ToolTip.data.Hide();
        }
        public bool Assign(ItemSlot itemInfo, int id) {
            // do the default behavior
            return this.Assign(itemInfo, id);
        }*/
    }
}