using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class UIMountPage_Upgrade : UIMountPage {
        [SerializeField] GameObject upgradePanel;
        [SerializeField] GameObject maxed;
        [SerializeField] TMP_Text current;
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] TMP_Text countTxt;
        Player player => Player.localPlayer;
        ScriptableItem item => ScriptableItem.dict[Storage.data.mount.upgradeItemId];
        public override void Refresh() {
            base.Refresh();

            Mount? mount = player.own.mounts.Get(id);
            if(mount != null) {
                current.text = mount.Value.tier.ToString();
                bool isMaxed = mount.Value.tier >= mount.Value.data.maxTier;
                maxed.SetActive(isMaxed);
                upgradePanel.SetActive(!isMaxed);
                if(!isMaxed) {
                    uint count = player.InventoryCountById(Storage.data.mount.upgradeItemId);
                    uint req = Storage.data.mount.upgradeReqCount[(int)mount.Value.tier];
                    countTxt.text = $"<color={(count > req ? "green" : "red")}>{LanguageManger.UseSymbols($"{count} / {req}", "(", ")")}</color>";
                }
            }
            else mounts._window.Status();
        }
        public void OnUpgrade() {
            int pIndex = player.own.mounts.Has(id);
            if(pIndex == -1) {
                Notify.list.Add("mount isn't active", "المرافق غير مفعل");
                return;
            }
            Tier currentTier = player.own.mounts[pIndex].tier;
            if(currentTier >= player.own.mounts[pIndex].data.maxTier) {
                Notify.list.Add("mount already reached max tier", "المرافق وصل لاعلي سمو");
                return;
            }
            if(player.InventoryCountById(Storage.data.mount.upgradeItemId) < Storage.data.mount.upgradeReqCount[(int)currentTier]) {
                Notify.list.Add("Not enough upgradeItemId", "upgradeItemId غير كافي");
                return;
            }
            player.CmdMountUpgrade(id);
        }
        void Awake() {
            slot.Assign(item.name);
            nameTxt.text = item.Name;
        }
    }
}