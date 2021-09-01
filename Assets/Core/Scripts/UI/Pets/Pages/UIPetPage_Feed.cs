using UnityEngine;
using RTLTMPro;
namespace Game.UI
{
    public class UIPetPage_Feed : UIPetPage {
        [SerializeField] UIPetPage_Feed_Slot[] feeds;
        Player player => Player.localPlayer;
        public override void Refresh() {
            base.Refresh();

            for(int i = 0; i < Storage.data.pet.feeds.Length; i++)
                feeds[i].Set(i);
        }
        public void OnFeed(int index) {
            if(index < 1 || index > Storage.data.pet.feeds.Length) {
                Notify.list.Add("Select a Feed", "اختار طعام");
                return;
            }
            int pIndex = player.own.pets.Has(id);
            if(pIndex == -1) {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.pets[pIndex].level >= Storage.data.pet.lvlCap) {
                Notify.list.Add("Pet already reached max level", "المرافق وصل لاعلي مستوي");
                return;
            }
            if(player.InventoryCountById(Storage.data.pet.feeds[index].name) < 1) {
                Notify.list.Add("Not enough Feed", "الطعام غير كافي");
                return;
            }
            
            player.CmdPetFeedx1(id, (ushort)Storage.data.pet.feeds[index].name);
        }
        public void OnFeedx10(int index) {
            if(index < 1 || index > Storage.data.pet.feeds.Length) {
                Notify.list.Add("Select a Feed", "اختار طعام");
                return;
            }
            int pIndex = player.own.pets.Has(id);
            if(pIndex == -1) {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.pets[pIndex].level >= Storage.data.pet.lvlCap) {
                Notify.list.Add("Pet already reached max level", "المرافق وصل لاعلي مستوي");
                return;
            }
            uint count = player.InventoryCountById(Storage.data.pet.feeds[index].name);
            if(count < 1) {
                Notify.list.Add("Not enough Feed", "الطعام غير كافي");
                return;
            }
            player.CmdPetFeedx10(id, (ushort)Storage.data.pet.feeds[index].name);
        }
        [System.Serializable]
        public struct UIPetPage_Feed_Slot {
            [SerializeField] UIItemSlot slot;
            [SerializeField] RTLTextMeshPro nameText;
            public void Set(int i) {
                FeedItem item = Storage.data.pet.feeds[i];
                slot.Assign(item.name);
                uint count = Player.localPlayer.InventoryCountById(item.name);
                nameText.text = $"{item.Name} <color={(count > 0 ? "green" : "white")}>{LanguageManger.UseSymbols(count.ToString(), "(", ")")}</color>";
            }
        }
    }
}