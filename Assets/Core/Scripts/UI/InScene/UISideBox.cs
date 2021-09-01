using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UISideBox : MonoBehaviour {
        [SerializeField] float updateInterval = .5f;
        [SerializeField] GameObject panel;
        [SerializeField] GameObject questsPanel;
        [SerializeField] GameObject partyPanel;
        [SerializeField] UIFlippable showHideBtn;
        Player player => Player.localPlayer;
        void UpdateData() {
            if(panel.activeSelf) {

            }
        }
        public void OnShowQuests() {

        }
        public void OnShowParty() {
            if(partyPanel.activeSelf) return;
            if(player.InTeam()) {
                partyPanel.SetActive(true);
                questsPanel.SetActive(false);
            } else {
                partyPanel.SetActive(false);
                questsPanel.SetActive(true);
            }
        }
        public void OnClickShowHide() {
            if(panel.activeSelf) {
                showHideBtn.horizontal = true;
                panel.SetActive(false);
            } else {
                showHideBtn.horizontal = false;
                panel.SetActive(true);
            }
        }
        void OnEnable() {
            if(player != null) InvokeRepeating("UpdateData", 0, updateInterval);
            else gameObject.SetActive(false);
        }
    }
}