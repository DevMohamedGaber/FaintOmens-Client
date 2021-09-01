using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class UIPetPage_StatUp : UIPetPage {
        [SerializeField] GameObject upgradePanel;
        [SerializeField] GameObject maxed;
        [SerializeField] TMP_Text current;
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] TMP_Text countTxt;
        Player player => Player.localPlayer;
        ScriptableItem item => ScriptableItem.dict[Storage.data.pet.starsUpItemId];
        public override void Refresh() {
            base.Refresh();

            PetInfo? pet = player.own.pets.Get(id);
            if(pet != null) {
                current.text = pet.Value.stars.ToString();
                bool isMaxed = pet.Value.stars >= Storage.data.pet.starsCap;
                maxed.SetActive(isMaxed);
                upgradePanel.SetActive(!isMaxed);
                if(!isMaxed) {
                    uint count = player.InventoryCountById(Storage.data.pet.starsUpItemId);
                    uint req = Storage.data.pet.starUpReqCount[(int)pet.Value.stars];
                    countTxt.text = $"<color={(count > req ? "green" : "red")}>{LanguageManger.UseSymbols($"{count} / {req}", "(", ")")}</color>";
                }
            }
            else pets._window.Status();
        }
        public void OnStarUp() {
            int pIndex = player.own.pets.Has(id);
            if(pIndex == -1) {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            byte currentStar = player.own.pets[pIndex].stars;
            if(currentStar >= Storage.data.pet.starsCap) {
                Notify.list.Add("Pet already reached max tier", "المرافق وصل لاعلي سمو");
                return;
            }
            if(player.InventoryCountById(Storage.data.pet.starsUpItemId) < Storage.data.pet.starUpReqCount[currentStar]) {
                Notify.list.Add("Not enough starsUpItemId", "starsUpItemId غير كافي");
                return;
            }
            player.CmdPetStarUp(id);
        }
        void Awake() {
            slot.Assign(item.name);
            nameTxt.text = item.Name;
        }
    }
}