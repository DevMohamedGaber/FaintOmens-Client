using UnityEngine;
using TMPro;
using System.Collections.Generic;
namespace Game.UI
{
    public class Pets : WindowWithBasicCurrencies
    {
        [Header("Pets")]
        [SerializeField] PetPage[] pages;
        //[SerializeField] UnityEngine.UI.Toggle expShare;
        [SerializeField] PetsList pets;
        int activePage = 0;
        public void Status()
        {
            GoToPage(0);
        }
        public void Upgrade()
        {
            GoToPage(1);
        }
        public void StarUp()
        {
            GoToPage(2);
        }
        public void Train()
        {
            GoToPage(3);
        }
        void GoToPage(int page)
        {
            for(int i = 0; i < pages.Length; i++)
            {
                pages[i].gameObject.SetActive(i == page);
            }
            activePage = page;
            pages[page].Refresh();
        }
        public void Show(ushort petId)
        {
            gameObject.SetActive(true);
            pets.SelectId(petId);
        }
        public void OnPetUpdated(PetInfo pet)
        {
            pets.UpdatePet(pet);
            pages[activePage].Refresh();
        }
        public void UpdateExpShare()
        {
            //expShare.interactable = player.own.shareExpWithPet;
        }
        public void OnExpShareChanged()
        {
            //player.CmdPetChangeExpShare();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            Status();
        }
    }
}