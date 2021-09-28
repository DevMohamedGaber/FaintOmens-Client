using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class PetPage_StarUp : PetPage
    {
        [Header("Stats")]
        [SerializeField] PetStats stats;
        [SerializeField] GameObject requirementsObj;
        [SerializeField] GameObject maxedObj;
        [SerializeField] TMP_Text currentStarCount;
        [SerializeField] TMP_Text nextStarCount;
        [Header("Requirements")]
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        public override void Refresh()
        {
            base.Refresh();

            PetInfo pet = player.own.pets.Get(id);
            if(pet.id != 0)
            {
                bool isMaxed = pet.stars >= Storage.data.pet.starsCap;
                currentStarCount.text = pet.stars.ToString();
                nextStarCount.text = !isMaxed ? pet.stars.ToString() : pet.stars.ToString();
                maxedObj.SetActive(isMaxed);
                requirementsObj.SetActive(!isMaxed);
                if(!isMaxed)
                {
                    stats.SetWithNextStarBonus(pet);
                    if(nameTxt != null && slot.IsAssigned())
                    {
                        uint count = player.InventoryCountById(Storage.data.pet.starsUpItemId);
                        uint req = Storage.data.pet.starUpReqCount[(int)pet.stars];
                        nameTxt.text = $"{slot.data.Name} <color={(count > req ? "green" : "red")}>{LanguageManger.UseSymbols($"{count} / {req}", "(", ")")}</color>";
                    }
                }
            }
            else
            {
                pets.window.Status();
            }
        }
        public void OnStarUp()
        {
            int pIndex = player.own.pets.Has(id);
            if(pIndex == -1)
            {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            byte currentStar = player.own.pets[pIndex].stars;
            if(currentStar >= Storage.data.pet.starsCap)
            {
                Notify.list.Add("Pet already reached max tier", "المرافق وصل لاعلي سمو");
                return;
            }
            if(player.InventoryCountById(Storage.data.pet.starsUpItemId) < Storage.data.pet.starUpReqCount[currentStar])
            {
                Notify.list.Add("Not enough starsUpItemId", "starsUpItemId غير كافي");
                return;
            }
            player.CmdPetStarUp(id);
        }
        void Awake()
        {
            slot.Assign(Storage.data.pet.starsUpItemId);
        }
    }
}