using UnityEngine;
using TMPro;
using System.Collections.Generic;
namespace Game.UI
{
    public class UIPets : MonoBehaviour {
        [SerializeField] GameObject[] pages;
        [SerializeField] TMP_Text diamonds;
        [SerializeField] TMP_Text b_diamonds;
        [SerializeField] UnityEngine.UI.Toggle expShare;
        [SerializeField] UIPetsList pets;
        WaitForSeconds updateInterval = new WaitForSeconds(1);
        Player player => Player.localPlayer;
        int activePage = 0;
        public void Status(int index = 0) => GoToPage(0);
        public void Feed() => GoToPage(1);
        public void Train() => GoToPage(2);
        public void Upgrade() => GoToPage(3);
        public void StarUp() => GoToPage(4);
        void GoToPage(int page) {
            for(int i = 0; i < pages.Length; i++)
                pages[i].SetActive(i == page);

            activePage = page;
            pages[page].GetComponent<UIPetPage>().Refresh();
        }
        public void Show(ushort petId) {
            gameObject.SetActive(true);
            pets.SelectId(petId);
        }
        public void OnPetUpdated(PetInfo pet) {
            pets.UpdatePet(pet);
            pages[activePage].GetComponent<UIPetPage>().Refresh();
        }
        public void UpdateExpShare() {
            expShare.interactable = player.own.shareExpWithPet;
        }
        public void OnExpShareChanged() => player.CmdPetChangeExpShare();
        public bool IsVisible() => gameObject.activeSelf;
        IEnumerator<WaitForSeconds> UpdateData() {
            diamonds.text = player.own.diamonds.ToString();
            b_diamonds.text = player.own.b_diamonds.ToString();

            yield return updateInterval;
        }
        void OnEnable() {
            if(player != null) {
                Status();
                StartCoroutine(UpdateData());
            }
            else gameObject.SetActive(false);
        }
        void OnDisable() {
            StopCoroutine(UpdateData());
        }
    }
}