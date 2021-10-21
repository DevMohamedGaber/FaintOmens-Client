using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIMarriageProposal : MonoBehaviour
    {
        public Image avatar;
        public Image classIcon;
        public RTLTMPro.RTLTextMeshPro nameTxt;
        public UILanguageDefinerSingle level;
        public UILanguageDefinerSingle guild;
        public UILanguageDefinerSingle br;
        int index = -1;
        Player player => Player.localPlayer;
        public void Set(int index)
        {
            this.index = index;
            MarriageProposal prop = player.own.marriageProposals[index];
            avatar.sprite = UIManager.data.assets.avatars[prop.avatar];
            classIcon.sprite = prop.classInfo.data.icon;
            nameTxt.text = prop.name;
            level.SetSuffix($": {prop.level}");
            guild.SetSuffix($": {prop.guildName}");
            br.SetSuffix($": {prop.br}");
        }
        public void OnAccept() {
            if(index < 0 || index >= player.own.marriageProposals.Count)
            {
                Notify.SomethingWentWrong();
            }
            else if(player.IsMarried())
            {
                Notify.AlreadyMarried();
            }
            else
            {
                player.CmdAcceptMarriageProposal(index);
            }
        }
        public void OnRefuse()
        {
            if(index < 0 || index >= player.own.marriageProposals.Count)
            {
                Notify.SomethingWentWrong();
            }
            else
            {
                player.CmdRefuseMarriageProposal(index);
            }
        }
    }
}