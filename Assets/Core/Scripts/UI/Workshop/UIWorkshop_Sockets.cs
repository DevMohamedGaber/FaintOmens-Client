using UnityEngine;
namespace Game.UI
{
    public class UIWorkshop_Sockets : UISubWindowBase {
    #region Data
        [Header("UI")]
        [SerializeField] Transform invContent;
        [SerializeField] GameObject prefab;
        [SerializeField] UIWorkshopItemSlot selectedSlot;
        [SerializeField] UIItemSlot[] socketSlots;
        [SerializeField] GameObject[] removeBtns;
        [Header("Unlock Msg")]
        [SerializeField] GameObject unlockMsgObj;
        [SerializeField] TMPro.TMP_Text unlockMsgSlotNumberTxt;
        [SerializeField] UICountableItemSlot unlockItemSlot;
        UIWorkshopItemSlot[] slots;
        [SerializeField] int selectedSocket = -1;
        ushort unlockItemId => Storage.data.item.unlockSocketItemId;
    #endregion
    #region Update
        public override void Refresh() {
            RefreshInventory();
            UpdateEnhancmentSlots();
        }
        void RefreshInventory() {
            int nextSlot = 0, i = 0;
            if(selectedSocket == -1) {
                // equipments
                for(i = 0; i < player.equipment.Count; i++) {
                    if(!player.equipment[i].isEmpty) {
                        if(selectedSlot.IsAssigned() && selectedSlot.from == WorkshopOperationFrom.Equipments && selectedSlot.ID == i)
                            continue;
                        slots[nextSlot].Assign(player.equipment[i], i, WorkshopOperationFrom.Equipments);
                        slots[nextSlot].onDoubleClick.SetListener((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                        nextSlot++;
                    }
                }
                // accessories
                for(i = 0; i < player.own.accessories.Count; i++) {
                    if(!player.own.accessories[i].isEmpty) {
                        if(selectedSlot.IsAssigned() && selectedSlot.from == WorkshopOperationFrom.Accessories && selectedSlot.ID == i)
                            continue;
                        slots[nextSlot].Assign(player.own.accessories[i], i, WorkshopOperationFrom.Accessories);
                        slots[nextSlot].onDoubleClick.SetListener((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                        nextSlot++;
                    }
                }
                // inventory
                for(i = 0; i < player.own.inventorySize; i++) {
                    if(!player.own.inventory[i].isEmpty && player.own.inventory[i].item.data is EquipmentItem) {
                        if(selectedSlot.IsAssigned() && selectedSlot.from == WorkshopOperationFrom.Inventory && selectedSlot.ID == i)
                            continue;
                        slots[nextSlot].Assign(player.own.inventory[i], i);
                        slots[nextSlot].onDoubleClick.SetListener((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                        nextSlot++;
                    }
                }
            }
            else {
                for(i = 0; i < player.own.inventorySize; i++) {
                    if(player.own.inventory[i].isEmpty) continue;
                    if(player.own.inventory[i].item.data is GemItem) {
                        slots[nextSlot].Assign(player.own.inventory[i], i);
                        slots[nextSlot].onDoubleClick.SetListener((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                        nextSlot++;
                    }
                }
            }
            // empty the rest
            if(nextSlot < slots.Length - 1) {
                for(i = nextSlot; i < slots.Length; i++) {
                    slots[i].Unassign();
                }
            }
        }
        void UpdateEnhancmentSlots() {
            // update selected item
            if(selectedSlot.IsAssigned()) {
                // selected equipment
                if(selectedSlot.from == WorkshopOperationFrom.Equipments) {
                    selectedSlot.UpdateData(player.equipment[selectedSlot.ID]);
                }
                else if(selectedSlot.from == WorkshopOperationFrom.Accessories) {
                    selectedSlot.UpdateData(player.own.accessories[selectedSlot.ID]);
                }
                else if(selectedSlot.from == WorkshopOperationFrom.Inventory) {
                    selectedSlot.UpdateData(player.own.inventory[selectedSlot.ID]);
                }
                // Set Sockets to Slots
                SetSocketToSlot(0, selectedSlot.data.socket1);
                SetSocketToSlot(1, selectedSlot.data.socket2);
                SetSocketToSlot(2, selectedSlot.data.socket3);
                SetSocketToSlot(3, selectedSlot.data.socket4);
            }
            else {
                for(int i = 0; i < socketSlots.Length; i++) {
                    socketSlots[i].Unassign();
                    removeBtns[i].SetActive(false);
                }
                selectedSocket = -1;
            }
        }
    #endregion
    #region Selected ItemSlot
        public void OnDeselectSlot() {
            selectedSlot.Unassign();
            Refresh();
        }
        void OnSelectSlot(UIWorkshopItemSlot itemSlot) {
            if(itemSlot.IsAssigned()) {
                if(selectedSocket > -1) {
                    if(!(itemSlot.data.data is GemItem)) {
                        UINotifications.list.Add("please select a gem");
                        return;
                    }
                    if(!selectedSlot.IsAssigned()) {
                        UINotifications.list.Add("please select equipment item to inlay");
                        return;
                    }
                    if(selectedSlot.data.HasGemWithType(((GemItem)itemSlot.data.data).bonusType)) {
                        UINotifications.list.Add("item already inlayed with a gem of the same type");
                        return;
                    }
                    player.CmdWorkshopAddGemInSocket(selectedSlot.ID, selectedSlot.from, selectedSocket, itemSlot.ID);
                }
                else {
                    selectedSlot.Assign(itemSlot);
                    Refresh();
                }
            }
        }
    #endregion
    #region Socket Slots
        public void OnSelectSocketSlot(int index) {
            selectedSocket = index != selectedSocket || selectedSocket == -1 ? index : -1;
            Refresh();
        }
        public void OnRemove(int index) {
            if(!selectedSlot.IsAssigned()) {
                UINotifications.list.Add("please select equipment item to inlay");
                return;
            }
            if(socketSlots[index].isLocked) {
                UINotifications.list.Add("socket is locked");
                return;
            }
            if(!socketSlots[index].IsAssigned()) {
                UINotifications.list.Add("socket is already empty");
                return;
            }
            player.CmdWorkshopRemoveGemFromSocket(selectedSlot.ID, selectedSlot.from, index);
        }
        void SetSocketToSlot(int i, Socket socket) {
            if(socket.id == -1) {
                socketSlots[i].Lock(() => ShowUnlockSocketMsg(i));
                removeBtns[i].SetActive(false);
            }
            else if(socket.id == 0) {
                socketSlots[i].Unassign();
                removeBtns[i].SetActive(false);
            }
            else {
                socketSlots[i].Assign(socket.data.name);
                socketSlots[i].ID = i;
                removeBtns[i].SetActive(true);
            }
        }
    #endregion
    #region Unlock Socket
        public void CloseUnlockSocketMsg() {
            unlockMsgObj.SetActive(false);
            selectedSocket = -1;
        }
        public void OnUnlock() {
            if(selectedSocket < 0 || selectedSocket > 3) {
                UINotifications.list.Add("Please select a socket to unlock");
                return;
            }
            if(!selectedSlot.IsAssigned()) {
                UINotifications.list.Add("please select equipment item to inlay");
                return;
            }
            if(!socketSlots[selectedSocket].isLocked) {
                UINotifications.list.Add("socket isn't locked");
                return;
            }
            player.CmdWorkshopUnlockSocket(selectedSlot.ID, selectedSlot.from, selectedSocket);
            CloseUnlockSocketMsg();
        }
        void ShowUnlockSocketMsg(int index) {
            selectedSocket = index;
            unlockMsgSlotNumberTxt.text = (index + 1).ToString();
            unlockItemSlot.SetAmount(player.InventoryCountById(unlockItemId) , 1);
            unlockMsgObj.SetActive(true);
        }
    #endregion
    #region Operations
        protected override void OnEnable() {
            base.OnEnable();
            Refresh();
        }
        void OnDisable() {
            selectedSlot.Unassign();
            for(int i = 0; i < slots.Length; i++) {
                slots[i].Unassign();
            }
        }
        void Awake() {
            slots = new UIWorkshopItemSlot[(int)player.own.inventorySize + Storage.data.player.equipmentCount + Storage.data.player.accessoriesCount];
            for(int i = 0; i < slots.Length; i++) {
                GameObject go = Instantiate(prefab, invContent, false);
                slots[i] = go.transform.GetComponent<UIWorkshopItemSlot>();
            }
            unlockItemSlot.Assign(unlockItemId);
            unlockItemSlot.SetAmount(0, 1);
        }
    #endregion
    }
}