using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UISideBox : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] GameObject panel;
        [SerializeField] UIFlippable showHideBtn;
        [Header("Quests")]
        [SerializeField] GameObject questsPanel;
        [SerializeField] GameObject questPrefab;
        [Header("Team")]
        [SerializeField] GameObject teamPanel;
        [SerializeField] GameObject noTeamObj;
        [SerializeField] UISideBox_ActionMenu teamActionMenu;
        [SerializeField] GameObject teamMemberPrefab;
        [SerializeField] Transform teamMembersContent;
        UISideBox_TeamMember[] teamMembers;
        Player player => Player.localPlayer;
        public void Refresh()
        {
            if(panel.activeSelf)
            {
                if(questsPanel.activeSelf)
                {
                    RefreshQuests();
                }
                else if(teamPanel.activeSelf)
                {
                    RefreshTeam();
                }
            }
        }
        public void OnShowQuests()
        {
            if(questsPanel.activeSelf)
                return;
            if(!panel.activeSelf)
            {
                OnSwitch();
            }
            teamPanel.SetActive(false);
            questsPanel.SetActive(true);
            RefreshQuests();
        }
        public void OnShowTeam()
        {
            if(teamPanel.activeSelf)
                return;
            if(!panel.activeSelf)
            {
                OnSwitch();
            }
            teamPanel.SetActive(true);
            questsPanel.SetActive(false);
            if(player.InTeam())
            {
                RefreshTeam();
                noTeamObj.SetActive(false);
            }
            else
            {
                noTeamObj.SetActive(true);
            }
        }
        public void OnSwitch()
        {
            if(panel.activeSelf)
            {
                showHideBtn.horizontal = true;
                panel.SetActive(false);
            }
            else
            {
                showHideBtn.horizontal = false;
                Refresh();
                panel.SetActive(true);
            }
        }
        public void OnSelectTeamMember(TeamMember member)
        {
            if(!player.InTeam())
            {
                Notify.NotInTeam();
                return;
            }
            teamActionMenu.Set(member);
        }
        void RefreshQuests()
        {

        }
        void RefreshTeam()
        {
            int i;
            Team team = player.own.team;
            for (i = 0; i < team.members.Length; i++)
            {
                teamMembers[i].Set(team.members[i], team.members[i].id == team.leaderId);
            }
            for (i = i; i < teamMembers.Length; i++)
            {
                teamMembers[i].Hide();
            }
        }
        void OnEnable()
        {
            Refresh();
        }
        void Awake()
        {
            if(teamMembersContent != null &&
               teamMemberPrefab != null &&
               Storage.data.team.capacity > 0)
            {
                teamMembers = new UISideBox_TeamMember[Storage.data.team.capacity];
                for (int i = 0; i < Storage.data.team.capacity; i++)
                {
                    UISideBox_TeamMember member = Instantiate(teamMemberPrefab, teamMembersContent, false).GetComponent<UISideBox_TeamMember>();
                    teamMembers[i] = member;
                }
            }
        }
    }
}