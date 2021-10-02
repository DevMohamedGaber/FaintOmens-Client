using UnityEngine;
using System;
using TMPro;
namespace Game.UI
{
    public class MountPage_Status : MountPage
    {
        [Header("General")]
        [SerializeField] MountStats stats;
        [Header("Activation")]
        [SerializeField] GameObject awakePanel;
        [SerializeField] UIItemSlot awakeItemSlot;
        [SerializeField] RTLTMPro.RTLTextMeshPro awakeItemName;
        [Header("Feed")]
        [SerializeField] GameObject feedPanel;
        [SerializeField] UIItemSlot[] feedSlots;
        int SelectedFeed = -1;
        public override void Refresh()
        {
            base.Refresh();
            
            Mount info = player.own.mounts.Get(id);
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
            else if(ScriptableMount.dict.TryGetValue(id, out ScriptableMount mountData))
            {
                if(stats != null)
                {
                    stats.Set(mountData);
                }
                if(awakeItemSlot != null)
                {
                    awakeItemSlot.Assign(mountData.ActivateItemId);
                }
                if(awakeItemName != null)
                {
                    uint itemCount = player.InventoryCountById(mountData.ActivateItemId);
                    awakeItemName.text = $"{LanguageManger.GetWord(mountData.ActivateItemId, LanguageDictionaryCategories.ItemName)} {LanguageManger.UseSymbols($"{(itemCount > 0 ? "1" : "0")}/1", "(", ")")}";
                    awakeItemName.color = itemCount > 0 ? Color.white : Color.red;
                }
            }
        }
        public void OnActivate()
        {
            if(id < 1 || id > ScriptableMount.dict.Count)
            {
                Notify.list.Add("Please select a Mount", "برجاء اختيار مرافق");
                return;
            }
            if(player.own.mounts.Has(id) != -1)
            {
                Notify.list.Add("This mount is already activated", "هذا المرافق مفعل مسبقا");
                return;
            }
            player.CmdMountActivate(ScriptableMount.dict[id].ActivateItemId);
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
            int pIndex = player.own.mounts.Has(id);
            if(pIndex == -1)
            {
                Notify.list.Add("Mount isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.mounts[pIndex].level >= Storage.data.mount.lvlCap)
            {
                Notify.list.Add("Mount already reached max level", "المرافق وصل لاعلي مستوي");
                return;
            }
            if(player.InventoryCountById(feedSlots[SelectedFeed].data.id) < 1)
            {
                Notify.list.Add("Not enough Feed", "الطعام غير كافي");
                return;
            }
            player.CmdMountFeedx1(id, feedSlots[SelectedFeed].data.id);
        }
        public void OnFeedx10()
        {
            if(SelectedFeed < 0 || SelectedFeed > feedSlots.Length)
            {
                Notify.list.Add("Select a Feed", "اختار طعام");
                return;
            }
            int pIndex = player.own.mounts.Has(id);
            if(pIndex == -1)
            {
                Notify.list.Add("Mount isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.mounts[pIndex].level >= Storage.data.mount.lvlCap)
            {
                Notify.list.Add("Mounts already reached max level", "المرافق وصل لاعلي مستوي");
                return;
            }
            uint count = player.InventoryCountById(feedSlots[SelectedFeed].data.id);
            if(count < 1)
            {
                Notify.list.Add("Not enough Feed", "الطعام غير كافي");
                return;
            }
            player.CmdMountFeedx10(id, feedSlots[SelectedFeed].data.id);
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