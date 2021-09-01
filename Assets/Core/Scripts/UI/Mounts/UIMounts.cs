using UnityEngine;
using TMPro;
using System.Collections.Generic;
namespace Game.UI
{
    public class UIMounts : MonoBehaviour {
        [SerializeField] GameObject[] pages;
        [SerializeField] TMP_Text diamonds;
        [SerializeField] TMP_Text b_diamonds;
        [SerializeField] UIMountsList mounts;
        WaitForSeconds updateInterval = new WaitForSeconds(1);
        Player player => Player.localPlayer;
        int activePage = 0;
        public void Status(int index = 0) => GoToPage(0);
        public void Feed() => GoToPage(1);
        public void Upgrade() => GoToPage(2);
        public void StarUp() => GoToPage(3);
        void GoToPage(int page) {
            for(int i = 0; i < pages.Length; i++)
                pages[i].SetActive(i == page);

            activePage = page;
            pages[page].GetComponent<UIMountPage>().Refresh();
        }
        public void Show(ushort mId) {
            gameObject.SetActive(true);
            mounts.SelectId(mId);
        }
        public void OnMountUpdated(Mount mount) {
            mounts.UpdateMount(mount);
            pages[activePage].GetComponent<UIMountPage>().Refresh();
        }
        public bool IsVisible() => gameObject.activeSelf;
        IEnumerator<WaitForSeconds> UpdateData() {
            diamonds.text = player.own.diamonds.ToString();
            b_diamonds.text = player.own.b_diamonds.ToString();

            yield return updateInterval;
        }
        void OnEnable() {
            if(player != null) {
                //Status();
                StartCoroutine(UpdateData());
            }
            else gameObject.SetActive(false);
        }
        void OnDisable() {
            StopCoroutine(UpdateData());
        }
    }
}