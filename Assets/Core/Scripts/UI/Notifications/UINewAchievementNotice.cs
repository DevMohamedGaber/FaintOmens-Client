using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UINewAchievementNotice : MonoBehaviour {
        [SerializeField] float hideAfter = 3f;
        [SerializeField] UILanguageDefinerSingle nameText;
        [SerializeField] UILanguageDefinerSingle reqText;
        [SerializeField] TMPro.TMP_Text pointsText;
        ushort id;
        Player player => Player.localPlayer;
        public void Show(Achievement achievement) {
            this.id = achievement.id;
            nameText.Set(id);
            reqText.Set(id);
            pointsText.text = achievement.data.points.ToString();
            gameObject.SetActive(true);
            Invoke("Hide", hideAfter);
        }
        public void OnHide() {
            Hide();
            CancelInvoke("Hide");
        }
        void Hide() => gameObject.SetActive(false);
        public void OnCollect() {
            if(player != null && id > 0) {
                player.CmdRecieveAchievementReward(id);
                OnHide();
            }
        }
        void OnDisable() {
            id = 0;
        }
    }
}