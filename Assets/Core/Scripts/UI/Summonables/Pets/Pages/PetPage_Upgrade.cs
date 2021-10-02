using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class PetPage_Upgrade : PetPage
    {
        [Header("Stats")]
        [SerializeField] PetStats stats;
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

            PetInfo pet = player.own.pets.Get(id);
            if(pet.id != 0)
            {
                bool isMaxed = pet.tier >= pet.data.maxTier;
                currentTireImage.sprite = UIManager.data.assets.tiers[(int)pet.tier];
                nextTireImage.sprite = UIManager.data.assets.tiers[(int)pet.tier + 1];
                requirementsObj.SetActive(!isMaxed);
                maxedObj.SetActive(isMaxed);
                if(!isMaxed)
                {
                    stats.SetWithNextTireBonus(pet);
                    if(nameTxt != null && slot.IsAssigned())
                    {
                        uint count = player.InventoryCountById(Storage.data.pet.upgradeItemId);
                        uint req = Storage.data.pet.upgradeReqCount[(int)pet.tier];
                        nameTxt.text = $"{slot.data.Name} <color={(count > req ? "green" : "red")}>{LanguageManger.UseSymbols($"{count} / {req}", "(", ")")}</color>";
                    }
                }
            }
            else
            {
                pets.window.Status();
            }
        }
        public void OnUpgrade()
        {
            int pIndex = player.own.pets.Has(id);
            if(pIndex == -1)
            {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            Tier currentTier = player.own.pets[pIndex].tier;
            if(currentTier >= player.own.pets[pIndex].data.maxTier)
            {
                Notify.list.Add("Pet already reached max tier", "المرافق وصل لاعلي سمو");
                return;
            }
            if(player.InventoryCountById(Storage.data.pet.upgradeItemId) < Storage.data.pet.upgradeReqCount[(int)currentTier])
            {
                Notify.list.Add("Not enough UpgradeItemId", "UpgradeItemId غير كافي");
                return;
            }
            player.CmdPetUpgrade(id);
        }
        void Awake()
        {
            slot.Assign(Storage.data.pet.upgradeItemId);
        }
    }
}