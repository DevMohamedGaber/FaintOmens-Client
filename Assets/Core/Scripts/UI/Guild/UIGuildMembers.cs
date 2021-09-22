using UnityEngine;
namespace Game.UI
{
    public class UIGuildMembers : MonoBehaviour {
        [Header("general")]
        [SerializeField] GameObject prefab;
        [SerializeField] Transform content;
        [SerializeField] TMPro.TMP_Text online;
        Player player => Player.localPlayer;

        [Header("Dropdown")]
        [SerializeField] GameObject dropdown;
        [SerializeField] Transform dropdownPanel;
        [SerializeField] GameObject addFriendBtn;
        [SerializeField] GameObject friendReqSent;
        [SerializeField] GameObject promoteBtn;
        [SerializeField] GameObject demoteBtn;
        uint selected;
        Vector3 pos;
        bool isValid => selected == 0 || !Server.IsPlayerIdWithInServer(selected);
        public void Set(GuildMember[] data) {
            UIUtils.BalancePrefabs(prefab, data.Length, content);
            int onlineCount = 0;
            for(int i = 0; i < data.Length; i++) {
                Transform row = content.GetChild(i);
                int iCopy = i;
                row.GetComponent<UIGuildMemberRow>().Set(data[i], () => OnSelect(data[iCopy].id, data[iCopy].rank, row.position.y));
                if(data[i].isOnline)
                    onlineCount++;
            }
            online.text = onlineCount + " / " + data.Length;
        }
        void OnSelect(uint id, GuildRank rank, float y) {
            if(id == player.id)
                return;
            selected = id;
            // set position
            pos.y = y;
            dropdownPanel.position = pos;
            // show dropdown
            addFriendBtn.SetActive(player.own.friends.Has(id));
            friendReqSent.SetActive(false);
            promoteBtn.SetActive(player.own.guildRank >= Storage.data.guild.promoteMinRank && rank < Storage.data.guild.promoteMinRank);
            demoteBtn.SetActive(player.own.guildRank >= Storage.data.guild.promoteMinRank && rank > GuildRank.Member);
            dropdown.SetActive(true);
        }
        public void CloseDropdown() {
            selected = 0;
            dropdown.SetActive(false);
        }
        public void OnAddFriend() {
            if(!isValid) {
                Notify.SomethingWentWrong();
                return;
            }
            if(player.own.friends.Has(selected)) {
                Notify.AlreadyFriends();
                return;
            }
            player.CmdSendFriendRequest(selected);
            CloseDropdown();
        }
        public void OnPM() {
            if(!isValid) {
                Notify.SomethingWentWrong();
                return;
            }
            UIManager.data.chat.OpenPrivateChatWith(selected);
            CloseDropdown();
        }
        public void OnPreview() {
            if(!isValid) {
                Notify.SomethingWentWrong();
                return;
            }
            player.CmdPreviewPlayerInfo(selected);
            CloseDropdown();
        }
        public void OnTeamInvite() {
            if(!isValid) {
                Notify.SomethingWentWrong();
                return;
            }
            player.CmdSendTeamInvitation(selected);
            CloseDropdown();
        }
        public void OnPromote() {
            if(!isValid) {
                Notify.SomethingWentWrong();
                return;
            }
            if(player.own.guildRank < Storage.data.guild.promoteMinRank) {
                Notifications.list.Add("You don't have permission to promote members", "ليس لديك صلاحية ترقية الاعضاء");
                return;
            }
            player.CmdGuildPromote(selected);
        }
        public void OnDemote() {
            if(!isValid) {
                Notify.SomethingWentWrong();
                return;
            }
            if(player.own.guildRank < Storage.data.guild.promoteMinRank) {
                Notifications.list.Add("You don't have permission to demote members", "ليس لديك صلاحية خفض رتبة الاعضاء");
                return;
            }
            player.CmdGuildDemote(selected);
        }
        void OnEnable() {
            player.CmdGetGuildMembersList();
        }
        void Awake() {
            pos = new Vector3(dropdownPanel.position.x, 0, 0);
        }
    }
}