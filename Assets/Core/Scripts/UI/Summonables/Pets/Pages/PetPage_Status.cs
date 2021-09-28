using UnityEngine;
using System;
using TMPro;
namespace Game.UI
{
    public class PetPage_Status : PetPage
    {
        [Header("General")]
        [SerializeField] PetStats stats;
        [Header("Activation")]
        [SerializeField] GameObject awakePanel;
        [SerializeField] UIItemSlot awakeItemSlot;
        [SerializeField] RTLTMPro.RTLTextMeshPro awakeItemName;
        [Header("Feed")]
        [SerializeField] GameObject feedPanel;
        [SerializeField] Transform feedContent;
        [SerializeField] UIItemSlot[] feedSlots;
        int SelectedFeed = -1;
        public override void Refresh()
        {
            base.Refresh();
            
            PetInfo info = player.own.pets.Get(id);
            bool isActive = info.id != 0;
            if(awakePanel != null)
            {
                awakePanel.SetActive(!isActive);
            }
            if(feedPanel != null)
            {
                feedPanel.SetActive(isActive);
            }

            if(isActive)
            {
                if(stats != null)
                {
                    stats.Set(info);
                }
                RefreshFeeds();
            }
            else if(ScriptablePet.dict.TryGetValue(id, out ScriptablePet petData))
            {
                if(stats != null)
                {
                    stats.Set(petData);
                }
                if(awakeItemSlot != null)
                {
                    awakeItemSlot.Assign(petData.ActivateItemId);
                }
                if(awakeItemName != null)
                {
                    uint itemCount = player.InventoryCountById(petData.ActivateItemId);
                    awakeItemName.text = $"{LanguageManger.GetWord(petData.ActivateItemId, LanguageDictionaryCategories.ItemName)} {LanguageManger.UseSymbols($"{(itemCount > 0 ? "1" : "0")}/1", "(", ")")}";
                    awakeItemName.color = itemCount > 0 ? Color.white : Color.red;
                }
            }
        }
        public void OnActivate()
        {
            if(id < 1 || id > ScriptablePet.dict.Count)
            {
                Notify.list.Add("Please select a pet", "برجاء اختيار مرافق");
                return;
            }
            if(player.own.pets.Has(id) != -1)
            {
                Notify.list.Add("This pet is already activated", "هذا المرافق مفعل مسبقا");
                return;
            }
            player.CmdPetActivate(ScriptablePet.dict[id].ActivateItemId);
        }
        public void OnSelectFeed(int index)
        {
            if(index < 0 || SelectedFeed >= feedSlots.Length)
            {
                Notify.list.Add("Select a Feed", "اختار طعام");
                return;
            }
            SelectedFeed = index;
            RefreshFeeds();
        }
        public void OnFeed()
        {
            if(SelectedFeed < 0 || SelectedFeed >= feedSlots.Length)
            {
                Notify.list.Add("Select a Feed", "اختار طعام");
                return;
            }
            int pIndex = player.own.pets.Has(id);
            if(pIndex == -1)
            {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.pets[pIndex].level >= Storage.data.pet.lvlCap)
            {
                Notify.list.Add("Pet already reached max level", "المرافق وصل لاعلي مستوي");
                return;
            }
            if(player.InventoryCountById(feedSlots[SelectedFeed].data.id) < 1)
            {
                Notify.list.Add("Not enough Feed", "الطعام غير كافي");
                return;
            }
            player.CmdPetFeedx1(id, feedSlots[SelectedFeed].data.id);
        }
        public void OnFeedx10()
        {
            if(SelectedFeed < 0 || SelectedFeed > feedSlots.Length)
            {
                Notify.list.Add("Select a Feed", "اختار طعام");
                return;
            }
            int pIndex = player.own.pets.Has(id);
            if(pIndex == -1)
            {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.pets[pIndex].level >= Storage.data.pet.lvlCap)
            {
                Notify.list.Add("Pet already reached max level", "المرافق وصل لاعلي مستوي");
                return;
            }
            uint count = player.InventoryCountById(feedSlots[SelectedFeed].data.id);
            if(count < 1)
            {
                Notify.list.Add("Not enough Feed", "الطعام غير كافي");
                return;
            }
            player.CmdPetFeedx10(id, feedSlots[SelectedFeed].data.id);
        }
        void RefreshFeeds()
        {
            for (int i = 0; i < feedSlots.Length; i++)
            {
                feedSlots[i].UpdateAmount();
            }
        }
    }
}