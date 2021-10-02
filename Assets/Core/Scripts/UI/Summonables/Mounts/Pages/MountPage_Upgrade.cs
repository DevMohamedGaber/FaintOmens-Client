using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class MountPage_Upgrade : MountPage
    {
        [Header("Stats")]
        [SerializeField] MountStats stats;
        [SerializeField] GameObject requirementsObj;
        [SerializeField] GameObject maxedObj;
        [SerializeField] Image currentTireImage;
        [SerializeField] Image nextTireImage;
        [Header("Requirements")]
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        public override void Refresh()
        {
            base.Refresh();

            Mount info = player.own.mounts.Get(id);
            if(info.id != 0)
            {
                bool isMaxed = info.tier >= info.data.maxTier;
                currentTireImage.sprite = UIManager.data.assets.tiers[(int)info.tier];
                nextTireImage.sprite = UIManager.data.assets.tiers[(int)info.tier + 1];
                requirementsObj.SetActive(!isMaxed);
                maxedObj.SetActive(isMaxed);
                if(!isMaxed)
                {
                    stats.SetWithNextTireBonus(info);
                    if(nameTxt != null && slot.IsAssigned())
                    {
                        uint count = player.InventoryCountById(Storage.data.mount.upgradeItemId);
                        uint req = Storage.data.mount.upgradeReqCount[(int)info.tier];
                        nameTxt.text = $"{slot.data.Name} <color={(count > req ? "green" : "red")}>{LanguageManger.UseSymbols($"{count} / {req}", "(", ")")}</color>";
                    }
                }
            }
            else
            {
                mounts.window.Status();
            }
        }
        public void OnUpgrade()
        {
            int pIndex = player.own.mounts.Has(id);
            if(pIndex == -1)
            {
                Notify.list.Add("Mount isn't active", "المرافق غير مفعل");
                return;
            }
            Tier currentTier = player.own.mounts[pIndex].tier;
            if(currentTier >= player.own.mounts[pIndex].data.maxTier)
            {
                Notify.list.Add("Mount already reached max tier", "المرافق وصل لاعلي سمو");
                return;
            }
            if(player.InventoryCountById(Storage.data.mount.upgradeItemId) < Storage.data.mount.upgradeReqCount[(int)currentTier])
            {
                Notify.list.Add("Not enough UpgradeItemId", "UpgradeItemId غير كافي");
                return;
            }
            player.CmdMountUpgrade(id);
        }
        void Awake()
        {
            slot.Assign(Storage.data.mount.upgradeItemId);
        }
    }
}