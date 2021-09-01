using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIWorkshop_Quality : UISubWindowBase {
    #region Data
        [SerializeField] Transform invContent;
        [SerializeField] GameObject prefab;
        [SerializeField] UIWorkshopItemSlot selectedSlot;
        [SerializeField] Slider progress;
        [SerializeField] TMPro.TMP_Text progressTxt;
        [SerializeField] UICountableItemSlot[] stoneSlots;
        [SerializeField] ushort[] stoneIds;
        UIWorkshopItemSlot[] slots;
    #endregion
    #region Update
        public override void Refresh() {
            RefreshInventory();
            UpdateEnhancmentSlots();
        }
        void RefreshInventory() {
            int nextSlot = 0, i = 0;
            // equipments
            for(i = 0; i < player.equipment.Count; i++) {
                if(player.equipment[i].isEmpty) continue;
                if(!player.equipment[i].item.quality.isGrowth) continue;
                if(selectedSlot.IsAssigned() && selectedSlot.from == WorkshopOperationFrom.Equipments && selectedSlot.ID == i)
                    continue;
                slots[nextSlot].Assign(player.equipment[i], i, WorkshopOperationFrom.Equipments);
                slots[nextSlot].onDoubleClick.SetListener((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                nextSlot++;
            }
            // accessories
            for(i = 0; i < player.own.accessories.Count; i++) {
                if(player.own.accessories[i].isEmpty) continue;
                if(!player.own.accessories[i].item.quality.isGrowth) continue;
                if(selectedSlot.IsAssigned() && selectedSlot.from == WorkshopOperationFrom.Accessories && selectedSlot.ID == i)
                    continue;
                slots[nextSlot].Assign(player.own.accessories[i], i, WorkshopOperationFrom.Accessories);
                slots[nextSlot].onDoubleClick.SetListener((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                nextSlot++;
            }
            // inventory
            for(i = 0; i < player.own.inventorySize; i++) {
                if(player.own.inventory[i].isEmpty) continue;
                if(player.own.inventory[i].item.data is EquipmentItem) {
                    if(!player.own.inventory[i].item.quality.isGrowth) continue;
                    if(selectedSlot.IsAssigned() && selectedSlot.from == WorkshopOperationFrom.Inventory && selectedSlot.ID == i)
                        continue;
                    slots[nextSlot].Assign(player.own.inventory[i], i, WorkshopOperationFrom.Inventory);
                    slots[nextSlot].onDoubleClick.SetListener<UIItemSlot>((itemSlot) => OnSelectSlot((UIWorkshopItemSlot)itemSlot));
                    nextSlot++;
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
                
                if(selectedSlot.data.plus == Storage.data.item.maxPlus) {
                    OnDeselectSlot();
                    return;
                }
                // stones Count
                progress.value = selectedSlot.data.quality.progress;
                progress.maxValue = selectedSlot.data.quality.expMax;
                progressTxt.text = $"{selectedSlot.data.quality.progress} / {selectedSlot.data.quality.expMax}  ({((selectedSlot.data.quality.progress / selectedSlot.data.quality.expMax) * 100).ToString("F0")}%)";
                for(int i = 0; i < stoneSlots.Length; i++) {
                    stoneSlots[i].SetAmount(player.InventoryCountById(stoneIds[i]), 1);
                }
                return;
            }
            else {
                progressTxt.text = "";
                progress.value = 1;
                progress.maxValue = 1;
                for(int i = 0; i < stoneSlots.Length; i++) {
                    stoneSlots[i].Unassign();
                }
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
                selectedSlot.Assign(itemSlot);
                Refresh();
            }
        }
    #endregion
    #region Operations
        public void OnSelectStone(int index) {
            player.CmdWorkshopUpgradeItemQuality(selectedSlot.ID, selectedSlot.from, index);
        }
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
            for(int i = 0; i < stoneSlots.Length; i++) {
                stoneSlots[i].Assign(stoneIds[i]);
            }
        }
    #endregion
    }
}