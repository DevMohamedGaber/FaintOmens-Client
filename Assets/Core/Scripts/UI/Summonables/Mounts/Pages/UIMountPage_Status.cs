using UnityEngine;
using System;
using TMPro;
namespace Game.UI
{
    public class UIMountPage_Status : UIMountPage {
        [SerializeField] UIMountPage_Status_Data status;
        [SerializeField] GameObject awakePanel;
        [SerializeField] UIItemSlot awakeItemSlot;
        [SerializeField] RTLTMPro.RTLTextMeshPro awakeItemName;
        Player player => Player.localPlayer;
        public override void Refresh() {
            base.Refresh();
            
            Mount? info = player.own.mounts.Get(id);
            awakePanel.SetActive(info == null);

            if(info != null) {
                
            }
            else if(ScriptableMount.dict.TryGetValue(id, out ScriptableMount data)) {
                awakeItemSlot.Assign(data.ActivateItemId);
                uint itemCount = player.InventoryCountById(data.ActivateItemId);
                awakeItemName.color = itemCount > 0 ? Color.white : Color.red;
                awakeItemName.text = LanguageManger.GetWord(data.ActivateItemId, LanguageDictionaryCategories.ItemName)
                                    + " " + LanguageManger.UseSymbols($"{(itemCount > 0 ? "1" : "0")}/1", "(", ")");
            }
        }
        public void OnActivate() {
            if(id < 1 || id > ScriptableMount.dict.Count) {
                Notify.list.Add("Please select a mount", "برجاء اختيار مرافق");
                return;
            }
            if(player.own.mounts.Has(id) != -1) {
                Notify.list.Add("This mount is already activated", "هذا المرافق مفعل مسبقا");
                return;
            }
            player.CmdMountActivate(ScriptableMount.dict[id].ActivateItemId);
        }
        [Serializable]
        public struct UIMountPage_Status_Data {
            public TMP_Text cardCount;
        }
    }
}