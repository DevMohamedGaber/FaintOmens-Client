using UnityEngine;
namespace Game.UI
{
    public class ActionMenu : MonoBehaviour
    {
        [SerializeField] BasicButton addFriendBtn;
        [SerializeField] BasicButton removeFriendBtn;
        [SerializeField] BasicButton inviteGuildBtn;
        [SerializeField] BasicButton pmBtn;
        [SerializeField] BasicButton inviteTeamBtn;
        uint id;
        Player player => Player.localPlayer;
        bool isFriend => id > 0 ? player.own.friends.Has(id) : false;
        public void Set(uint id, bool show = true)
        {
            this.id = id;
            SetFriend();
            SetGuild();
            SetPM();
            SetTeam();
            if(show)
            {
                Show();
            }
        }
        // Friends
        void SetFriend()
        {
            if(isFriend)
            {
                addFriendBtn.Hide();
                removeFriendBtn.Show();
            }
            else
            {
                addFriendBtn.Show();
                removeFriendBtn.Hide();
            }
        }
        public void OnAddFriend()
        {
            if(!Server.IsPlayerIdWithInServer(id))
            {
                Notify.InvalidTargetId();
                return;
            }
            if(isFriend)
            {
                Notify.AlreadyFriends();
                return;
            }
            player.CmdSendFriendRequest(id);
            Hide();
        }
        public void OnRemoveFriend()
        {
            if(!Server.IsPlayerIdWithInServer(id))
            {
                Notify.InvalidTargetId();
                return;
            }
            if(!isFriend)
            {
                Notify.TargetNotFriend();
                return;
            }
            player.CmdRemoveFriend(id);
            Hide();
        }
        // Guild Invite
        void SetGuild()
        {
            if(player.InGuild() && player.own.guildRank >= Storage.data.guild.inviteMinRank)
            {
                inviteGuildBtn.Show();
            }
            else
            {
                inviteGuildBtn.Hide();
            }
        }
        public void OnInviteGuild()
        {
            if(!Server.IsPlayerIdWithInServer(id))
            {
                Notify.InvalidTargetId();
                return;
            }
            if(!player.InGuild())
            {
                Notify.NotInGuild();
                return;
            }
            if(player.own.guildRank < Storage.data.guild.inviteMinRank)
            {
                Notify.list.Add("You don't have the permision to invite", "لا تملك الصلاحيه للارسال");
                return;
            }
            if(!IsTargetOnline())
            {
                Notify.TargetOffline();
                return;
            }
            if(IsTargetInGuild())
                return;
            player.CmdSendGuildInvitation(id);
            Hide();
        }
        protected virtual bool IsTargetInGuild()
        {
            return false;
        }
        // PM
        void SetPM()
        {
            if(IsTargetOnline())
            {
                pmBtn.Show();
            }
            else
            {
                pmBtn.Hide();
            }
        }
        public void OnPM()
        {
            if(!Server.IsPlayerIdWithInServer(id))
            {
                Notify.InvalidTargetId();
                return;
            }
            if(!IsTargetOnline())
            {
                Notify.TargetOffline();
                return;
            }
            UIManager.data.chat.OpenPrivateChatWith(id);
            Hide();
            OnPM();
        }
        protected virtual void OnSuccessfulPM() {}
        // Team
        void SetTeam()
        {
            if(player.InTeam() && !player.own.team.IsFull)
            {
                inviteTeamBtn.Show();
            }
            else
            {
                inviteTeamBtn.Hide();
            }
        }
        public void OnInviteTeam()
        {
            if(!Server.IsPlayerIdWithInServer(id))
            {
                Notify.InvalidTargetId();
                return;
            }
            if(!player.InTeam())
            {
                Notify.NotInTeam();
                return;
            }
            if(player.own.team.IsFull)
            {
                Notify.list.Add("Team is full", "الفريق مكتمل");
                return;
            }
            if(!IsTargetOnline())
            {
                Notify.TargetOffline();
                return;
            }
            if(IsTargetInTeam())
            {
                Notify.list.Add("Target already in a Team");
                return;
            }
            player.CmdSendTeamInvitation(id);
            Hide();
        }
        public virtual bool IsTargetInTeam()
        {
            return false;
        }

        // helpers
        protected virtual bool IsTargetOnline()
        {
            return true;
        }
        public virtual void Show()
        {
            if(id != 0)
            {
                gameObject.SetActive(true);
            }
        }
        public virtual void Hide()
        {
            gameObject.SetActive(false);
            id = 0;
        }
    }
}