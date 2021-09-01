using UnityEngine;
namespace Game.UI
{
    public class UIMiniNotifyIconsList : MonoBehaviour {
        [SerializeField] GameObject guildInvitationIcon;
        [SerializeField] GameObject partyInvitationIcon;
        [SerializeField] GameObject friendInvitationIcon;
        [SerializeField] GameObject marriageProposalIcon;
        [SerializeField] GameObject newMailIcon;
        public void ShowGuildInvitation() {
            guildInvitationIcon.SetActive(true);
        }
        public void NewMarriageProposal() => marriageProposalIcon.SetActive(true);
        public void ShowNewMail() => newMailIcon.SetActive(true);
    }
}