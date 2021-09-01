using UnityEngine;
using System;
using TMPro;
namespace Game.UI
{
    public class UIPetPage_Status : UIPetPage {
        [SerializeField] UIPetPage_Status_Data status;
        [SerializeField] GameObject awakePanel;
        [SerializeField] UIItemSlot awakeItemSlot;
        [SerializeField] RTLTMPro.RTLTextMeshPro awakeItemName;
        Player player => Player.localPlayer;
        public override void Refresh() {
            base.Refresh();
            
            PetInfo? info = player.own.pets.Get(id);
            awakePanel.SetActive(info == null);

            if(info != null) {
                
            }
            else if(ScriptablePet.dict.TryGetValue(id, out ScriptablePet petData)) {
                awakeItemSlot.Assign(petData.ActivateItemId);
                uint itemCount = player.InventoryCountById(petData.ActivateItemId);
                awakeItemName.color = itemCount > 0 ? Color.white : Color.red;
                awakeItemName.text = LanguageManger.GetWord(petData.ActivateItemId, LanguageDictionaryCategories.ItemName)
                                    + " " + LanguageManger.UseSymbols($"{(itemCount > 0 ? "1" : "0")}/1", "(", ")");
            }
        }
        public void OnActivate() {
            if(id < 1 || id > ScriptablePet.dict.Count) {
                Notify.list.Add("Please select a pet", "برجاء اختيار مرافق");
                return;
            }
            if(player.own.pets.Has(id) != -1) {
                Notify.list.Add("This pet is already activated", "هذا المرافق مفعل مسبقا");
                return;
            }
            player.CmdPetActivate(ScriptablePet.dict[id].ActivateItemId);
        }
        [Serializable]
        public struct UIPetPage_Status_Data {
            public TMP_Text cardCount;
        }
    }
}