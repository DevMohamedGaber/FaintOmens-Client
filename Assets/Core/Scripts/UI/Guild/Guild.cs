using UnityEngine;
namespace Game.UI
{
    public class Guild : WindowWithBasicCurrencies
    {
        [Header("Guild")]
        [SerializeField] SubWindow[] pages;
        public Game.Guild data;
        public Game.GuildMember myData;
        public GuildJoinOrCreate joinWindow => pages[0].GetComponent<GuildJoinOrCreate>();
        int activePage = -1;
        public override void Refresh()
        {
            pages[activePage].Refresh();
        }
        public void SetAvailableGuildsToJoin(GuildJoinInfo[] data)
        {
            if(activePage == 0)
            {
                joinWindow.Set(data);
            }
        }
        public void SetMembersList(GuildMember[] membersList)
        {
            pages[2].GetComponent<GuildMembers>().Set(membersList);
        }
        public void SetJoinRequestsList(GuildJoinRequest[] data)
        {

        }
        public void OnJoinedGuild()
        {
            if(pages[0].isVisible)
            {
                Home();
            }
        }
        public void JoinGuild() => GoToPage(0);
        public void Home() => GoToPage(1);
        public void Members() => GoToPage(2);
        public void Skills() => GoToPage(3);
        public void Donation() => GoToPage(4);
        public void Hall() => GoToPage(5);
        public void Shop() => GoToPage(6);
        public void Quests() => GoToPage(7);
        void GoToPage(int index)
        {
            if(!player.InGuild())
            {
                index = 7;
            }

            if(pages.Length < 1 || index > pages.Length)
                return;

            for(int i = 0; i < pages.Length; i++)
            {
                if(index == i)
                {
                    pages[i].Show();
                }
                else
                {
                    pages[i].Hide();
                }
            }

            activePage = index;
            if(!isVisible)
            {
                Show();
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();

            if(!player.InGuild())
            {
                JoinGuild();
            }
            else
            {
                player.CmdGetGuildData();

                if(activePage == -1)
                {
                    Home();
                }
            }
        }
        protected override void OnDisable()
        {
            base.OnDisable();

            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].Hide();
            }
            activePage = -1;
        }
    }
}