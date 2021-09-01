using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class UIGuildMemberRow : MonoBehaviour
    {
        [SerializeField] RTLTextMeshPro Name;
        [SerializeField] RTLTextMeshPro rank;
        [SerializeField] RTLTextMeshPro className;
        [SerializeField] TMP_Text level;
        [SerializeField] TMP_Text contribution;
        [SerializeField] TMP_Text br;
        [SerializeField] RTLTextMeshPro status;
        [SerializeField] UIBasicButton btn;
        public void Set(GuildMember member, UnityEngine.Events.UnityAction onClick) {
            Name.text = member.Name;
            className.text = member.classInfo.Name;
            level.text = member.level.ToString();
            contribution.text = member.contribution.ToString();
            br.text = member.br.ToString();
            status.text = member.isOnline ? $"<color=green>{LanguageManger.Decide("Online", "متصل")}</color>" : 
                                        $"<color=grey>{UIUtils.PrettySeconds(member.online)}</color>";
            btn.onClick = onClick;
        }
    }
}