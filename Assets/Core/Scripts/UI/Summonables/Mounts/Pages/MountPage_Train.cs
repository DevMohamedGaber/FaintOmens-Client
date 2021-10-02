using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class MountPage_Train : MountPage
    {
        /*[SerializeField] TMP_Text current;
        [SerializeField] TMP_Text p_to_attr;
        [SerializeField] GameObject requirementsObj;
        [SerializeField] GameObject maxedObj;
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        public override void Refresh()
        {
            base.Refresh();
            PetInfo pet = player.own.pets.Get(id);
            if(pet.id != 0)
            {
                current.text = pet.potential.ToString();
                p_to_attr.text = (pet.potential % Storage.data.pet.potentialToAP).ToString();

                bool isMaxed = pet.potential == Storage.data.pet.potentialMax;
                maxedObj.SetActive(isMaxed);
                requirementsObj.SetActive(!isMaxed);
                if(nameTxt != null && slot.IsAssigned())
                {
                    uint count = player.InventoryCountById(Storage.data.pet.trainItemId);
                    nameTxt.text = $"{slot.data.Name} <color={(count > 0 ? "green" : "white")}>{LanguageManger.UseSymbols(count.ToString(), "(", ")")}</color>";
                }
            }
            else
            {
                pets.window.Status();
            }
        }
        public void OnTrain()
        {
            int pIndex = player.own.pets.Has(id);
            if(pIndex == -1)
            {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.pets[pIndex].potential >= Storage.data.pet.potentialMax)
            {
                Notify.list.Add("Pet already reached max potential", "المرافق وصل لاعلي طموح");
                return;
            }
            if(player.InventoryCountById(Storage.data.pet.trainItemId) < 1)
            {
                Notify.list.Add("Not enough trainingItem", "تمرين غير كافي");
                return;
            }
            player.CmdPetTrain(id);
        }
        void Awake()
        {
            slot.Assign(Storage.data.pet.trainItemId);
        }*/
    }
}