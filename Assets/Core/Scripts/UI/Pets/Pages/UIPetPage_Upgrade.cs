using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class UIPetPage_Upgrade : UIPetPage {
        [SerializeField] GameObject upgradePanel;
        [SerializeField] GameObject maxed;
        [SerializeField] TMP_Text current;
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] TMP_Text countTxt;
        Player player => Player.localPlayer;
        ScriptableItem item => ScriptableItem.dict[Storage.data.pet.UpgradeItemId];
        public override void Refresh() {
            base.Refresh();

            PetInfo? pet = player.own.pets.Get(id);
            if(pet != null) {
                current.text = pet.Value.tier.ToString();
                bool isMaxed = pet.Value.tier >= pet.Value.data.maxTier;
                maxed.SetActive(isMaxed);
                upgradePanel.SetActive(!isMaxed);
                if(!isMaxed) {
                    uint count = player.InventoryCountById(Storage.data.pet.UpgradeItemId);
                    uint req = Storage.data.pet.upgradeReqCount[(int)pet.Value.tier];
                    countTxt.text = $"<color={(count > req ? "green" : "red")}>{LanguageManger.UseSymbols($"{count} / {req}", "(", ")")}</color>";
                }
            }
            else pets._window.Status();
        }
        public void OnUpgrade() {
            int pIndex = player.own.pets.Has(id);
            if(pIndex == -1) {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            Tier currentTier = player.own.pets[pIndex].tier;
            if(currentTier >= player.own.pets[pIndex].data.maxTier) {
                Notify.list.Add("Pet already reached max tier", "المرافق وصل لاعلي سمو");
                return;
            }
            if(player.InventoryCountById(Storage.data.pet.UpgradeItemId) < Storage.data.pet.upgradeReqCount[(int)currentTier]) {
                Notify.list.Add("Not enough UpgradeItemId", "UpgradeItemId غير كافي");
                return;
            }
            player.CmdPetUpgrade(id);
        }
        void Awake() {
            slot.Assign(item.name);
            nameTxt.text = item.Name;
        }
    }
}