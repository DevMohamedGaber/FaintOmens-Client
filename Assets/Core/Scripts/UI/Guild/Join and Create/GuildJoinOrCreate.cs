using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
namespace Game.UI
{
    public class GuildJoinOrCreate : SubWindow
    {
        [SerializeField] Transform content;
        [SerializeField] UIToggleGroup guildList;
        [SerializeField] GameObject prefab;
        [SerializeField] GameObject NoGuilds;
        [SerializeField] TMPro.TMP_InputField nameInput;
        [SerializeField] RTLTMPro.RTLTextMeshPro noticeTxt;
        GuildJoinInfo[] _data;
        public GuildJoinInfo[] data
        {
            get
            {
                return _data;
            }
        }
        
        public void Set(GuildJoinInfo[] data)
        {
            this._data = data;
            
            UIUtils.BalancePrefabs(prefab, data.Length, content);
            NoGuilds.SetActive(data.Length < 1);

            if(data.Length > 0)
            {
                for(int i = 0; i < data.Length; i++)
                {
                    content.GetChild(i).GetComponent<GuildJoinInfoRow>().Set(i, this);
                }
            }
            guildList.UpdateTogglesList();
        }
        public void SelectChanged()
        {
            noticeTxt.text = _data[guildList.currentIndex].notice;
        }
        public void OnJoin()
        {
            if(data[guildList.currentIndex].id != 0)
            {
                player.CmdSendJoinRequestToGuild(data[guildList.currentIndex].id);
            }
            else
            {
                Notify.SomethingWentWrong();
            }
        }
        public void OnJoinAll()
        {

        }
        public void OnShowCreateWindow()
        {
            nameInput.characterLimit = Storage.data.guild.maxNameLength;
            nameInput.text = "";
        }
        public void OnCreate(GameObject window)
        {
            if(!IsValidGuildName(nameInput.text))
            {
                Notify.InvalidName();
            }
            else if(player.own.gold < Storage.data.guild.creationPriceGold)
            {
                Notify.DontHaveEnoughGold();
            }
            else
            {
                player.CmdCreateGuild(nameInput.text);
                window.SetActive(false);
            }
        }
        void Check()
        { // if accepted in guild switch to guild home page
            if(player.InGuild())
            {
                UIManager.data.pages.guild.Show();
            }
        }
        bool IsValidGuildName(string guildName)
        {
            return  guildName.Length <= Storage.data.guild.maxNameLength &&
                    Regex.IsMatch(guildName, @"^[a-zA-Z0-9_]+$");
        }
        protected override void OnEnable()
        {
            base.OnEnable();

            if(!player.InGuild())
            {
                player.CmdGetAvailableGuildsToJoin();
            }
            else
            {
                UIManager.data.pages.guild.Home();
            }
        }
        void OnDisable()
        {
            if(content.childCount > 0)
            {
                for(int i = 0; i < content.childCount; i++)
                {
                    Destroy(content.GetChild(i).gameObject);
                    _data = new GuildJoinInfo[]{};
                }
            }
            NoGuilds.SetActive(true);
        }
    }
}