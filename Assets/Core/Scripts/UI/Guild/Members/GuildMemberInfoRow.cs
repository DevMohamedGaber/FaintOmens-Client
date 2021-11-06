using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class GuildMemberInfoRow : MonoBehaviour
    {
        [SerializeField] Image avatar;
        [SerializeField] RTLTextMeshPro nameTxt;
        [SerializeField] RTLTextMeshPro rank;
        [SerializeField] RTLTextMeshPro className;
        [SerializeField] TMP_Text level;
        [SerializeField] TMP_Text contribution;
        [SerializeField] TMP_Text br;
        [SerializeField] RTLTextMeshPro status;
        [SerializeField] UIToggle toggle;
        public void Set(GuildMember member, GuildMembers window)
        {
            if(avatar != null)
            {
                avatar.sprite = UIManager.data.assets.avatars[member.avatar];
            }
            if(nameTxt != null)
            {
                nameTxt.text = member.Name;
            }
            if(className != null)
            {
                className.text = member.classInfo.Name;
            }
            if(level != null)
            {
                level.text = member.level.ToString();
            }
            if(contribution != null)
            {
                contribution.text = member.contribution.ToString();
            }
            if(br != null)
            {
                br.text = member.br.ToString();
            }
            if(status != null)
            {
                status.text = member.isOnline ? (LanguageManger.Decide("Online", "متصل")) : UIUtils.PrettySeconds(member.online);
                status.color = member.isOnline ? Color.green : Color.grey;
            }
            if(Player.localPlayer.id != member.id && toggle != null)
            {
                toggle.onSelect = () => window.ShowActionMenu(member.id);
            }
        }
    }
}