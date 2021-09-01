using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace Game.UI
{
    public class UIWardrobe : UIWindowBase {
        public static UIWardrobe singleton;
        [SerializeField] GameObject[] pages;
        [SerializeField] Toggle visibility;
        Player player => Player.localPlayer;
        public void GoToPage(int index) {
            for(int i = 0; i < pages.Length; i++)
                pages[i].SetActive(i == index);
        }
        public void UpdateVisibility() {
            visibility.interactable = player.showWardrop;
            // update preview
            UIPreviewManager.singleton.InstantiatePlayer(player);
        }
        public void OnChangeVisibility() => player.CmdWardrobeSwitchVisibility();
        public void OnSynthesizeDone(bool success = false) {
            pages[0].GetComponent<UIWardrobeSynthesize>().OnSynthesizeDone(success);
        }
        public bool IsVisible() => gameObject.activeSelf;
        void OnEnable() {
            UIPreviewManager.singleton.InstantiatePlayer(player);
            UpdateVisibility();
            GoToPage(0);
        }
        void OnDisable() {
            UIPreviewManager.singleton.Clear();
        }
    }
}