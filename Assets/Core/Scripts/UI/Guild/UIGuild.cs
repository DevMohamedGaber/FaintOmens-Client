using UnityEngine;
namespace Game.UI
{
    public class UIGuild : MonoBehaviour {
        [SerializeField] GameObject[] pages;
        Player player => Player.localPlayer;
        public void SetAvailableGuildsToJoin(GuildJoinInfo[] data) => pages[7].GetComponent<UIJoinOrCreateGuild>().Set(data);
        public void SetMembersList(GuildMember[] membersList) => pages[1].GetComponent<UIGuildMembers>().Set(membersList);
        public void SetJoinRequestsList(GuildJoinRequest[] data) {

        }
        public void OnJoinedGuild() {
            if(pages[7].activeSelf)
                Home();
        }
        public void Home() => GoToPage(0);
        public void Members() => GoToPage(1);
        public void Skills() => GoToPage(2);
        public void Donation() => GoToPage(3);
        public void Hall() => GoToPage(4);
        public void Shop() => GoToPage(5);
        public void Quests() => GoToPage(6);
        public void JoinGuild() => GoToPage(7);
        public void Show(int page = 0) {
            gameObject.SetActive(true);
            GoToPage(page);
        }
        void GoToPage(int page) {
            if(!player.InGuild()) 
                page = 7;
            if(pages.Length < 1 || page > pages.Length) return;
            for(int i = 0; i < pages.Length; i++)
                pages[i].SetActive(page == i);
        }
        void OnEnable() {
            if(player != null) Home();
            else gameObject.SetActive(false);
        }
        void OnDisable() {
            for(int i = 0; i < pages.Length; i++)
                pages[i].SetActive(false);
        }
    }
}