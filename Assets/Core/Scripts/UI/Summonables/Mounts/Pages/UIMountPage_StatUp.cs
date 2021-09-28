using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class UIMountPage_StatUp : UIMountPage {
        [SerializeField] GameObject upgradePanel;
        [SerializeField] GameObject maxed;
        [SerializeField] TMP_Text current;
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] TMP_Text countTxt;
        Player player => Player.localPlayer;
        ScriptableItem item => ScriptableItem.dict[Storage.data.mount.starsUpItemId];
        public override void Refresh() {
            base.Refresh();

            Mount? mount = player.own.mounts.Get(id);
            if(mount != null) {
                current.text = mount.Value.stars.ToString();
                bool isMaxed = mount.Value.stars >= Storage.data.mount.starsCap;
                maxed.SetActive(isMaxed);
                upgradePanel.SetActive(!isMaxed);
                if(!isMaxed) {
                    uint count = player.InventoryCountById(Storage.data.mount.starsUpItemId);
                    uint req = Storage.data.mount.starUpReqCount[(int)mount.Value.stars];
                    countTxt.text = $"<color={(count > req ? "green" : "red")}>{LanguageManger.UseSymbols($"{count} / {req}", "(", ")")}</color>";
                }
            }
            else mounts._window.Status();
        }
        public void OnStarUp() {
            int pIndex = player.own.mounts.Has(id);
            if(pIndex == -1) {
                Notify.list.Add("mount isn't active", "المرافق غير مفعل");
                return;
            }
            byte currentStar = player.own.mounts[pIndex].stars;
            if(currentStar >= Storage.data.mount.starsCap) {
                Notify.list.Add("mount already reached max tier", "المرافق وصل لاعلي سمو");
                return;
            }
            if(player.InventoryCountById(Storage.data.mount.starsUpItemId) < Storage.data.mount.starUpReqCount[currentStar]) {
                Notify.list.Add("Not enough starsUpItemId", "starsUpItemId غير كافي");
                return;
            }
            player.CmdMountStarUp(id);
        }
        void Awake() {
            slot.Assign(item.name);
            nameTxt.text = item.Name;
        }
    }
}