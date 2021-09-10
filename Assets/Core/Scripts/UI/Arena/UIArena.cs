using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class UIArena : Window
    {
        [SerializeField] GameObject[] pages;
        int current = 0;
        public void Refresh() => pages[current].GetComponent<UIArena_Page>().Refresh();
        public void GoToPage(int page) {
            for(int i = 0; i < pages.Length; i++)
                pages[i].SetActive(i == page);
            current = page;
        }
        protected override void OnEnable() {
            base.OnEnable();
            GoToPage(0);
        }
    }
}