using UnityEngine;
namespace Game.UI
{
    public class UISideBox_TeamMember : MonoBehaviour
    {
        [SerializeField] UnityEngine.UI.Image avatar;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] TMPro.TMP_Text levelTxt;
        [SerializeField] UIProgressBar health;
        [SerializeField] GameObject offlineOverlay;
        [SerializeField] GameObject leaderObj;
        [SerializeField] BasicButton button;
        public void Set(TeamMember member, bool isLeader)
        {
            avatar.sprite = UIManager.data.assets.avatars[(int)member.avatar];
            nameTxt.text = member.name;
            levelTxt.text = member.level.ToString();
            health.fillAmount = member.online ? 1 : 0;
            offlineOverlay.SetActive(!member.online);
            leaderObj.SetActive(isLeader);
            if(member.id != Player.localPlayer.id)
            {
                button.onClick = () => UIManager.data.inScene.sideBox.OnSelectTeamMember(member);
            }
        }
        public void Hide()
        {
            gameObject.SetActive(false);
            if(button.hasAction)
            {
                button.onClick = null;
            }
        }
    }
}