using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class UIPetPage_Train : UIPetPage {
        [SerializeField] TMP_Text current;
        [SerializeField] TMP_Text p_to_attr;
        [SerializeField] GameObject maxed;
        [SerializeField] GameObject trainPanel;
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] TMP_Text countTxt;
        Player player => Player.localPlayer;
        ScriptableItem item => ScriptableItem.dict[Storage.data.pet.trainItemId];
        public override void Refresh() {
            base.Refresh();
            PetInfo? pet = player.own.pets.Get(id);
            if(pet != null) {
                current.text = pet.Value.potential.ToString();
                p_to_attr.text = (pet.Value.potential % Storage.data.pet.potentialToAP).ToString();

                bool isMaxed = pet.Value.potential == Storage.data.pet.potentialMax;
                maxed.SetActive(isMaxed);
                trainPanel.SetActive(!isMaxed);
                uint count = player.InventoryCountById(Storage.data.pet.trainItemId);
                countTxt.text = $"<color={(count > 0 ? "green" : "white")}>{LanguageManger.UseSymbols(count.ToString(), "(", ")")}</color>";
            }
            else pets._window.Status();
        }
        public void OnTrain() {
            int pIndex = player.own.pets.Has(id);
            if(pIndex == -1) {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.pets[pIndex].potential >= Storage.data.pet.potentialMax) {
                Notify.list.Add("Pet already reached max potential", "المرافق وصل لاعلي طموح");
                return;
            }
            if(player.InventoryCountById(Storage.data.pet.trainItemId) < 1) {
                Notify.list.Add("Not enough trainingItem", "تمرين غير كافي");
                return;
            }
            player.CmdPetTrain(id);
        }
        void Awake() {
            slot.Assign(item.name);
            nameTxt.text = item.Name;
        }
    }
}