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
        [SerializeField] GameObject teamMemberPrefab;
        [SerializeField] Transform teamMembersContent;
        [SerializeField] UISideBox_TeamMember[] teamMembers;
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
        void RefreshQuests()
        {

        }
        void RefreshTeam()
        {
            
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
                    GameObject go = Instantiate(teamMemberPrefab, teamMembersContent, false);
                    teamMembers[i] = teamMembersContent.GetChild(i).GetComponent<UISideBox_TeamMember>();
                }
            }
        }
    }
}