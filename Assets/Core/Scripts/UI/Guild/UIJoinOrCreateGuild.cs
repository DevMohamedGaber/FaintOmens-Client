using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
namespace Game.UI
{
    public partial class UIJoinOrCreateGuild : MonoBehaviour {
        [SerializeField] Transform content;
        [SerializeField] GameObject NoGuilds;
        [SerializeField] GameObject prefab;
        [SerializeField] TMPro.TMP_InputField GuildName;
        Player player => Player.localPlayer;
        public void Set(GuildJoinInfo[] data) {
            UIUtils.BalancePrefabs(prefab, data.Length, content);
            NoGuilds.SetActive(data.Length == 0);
            if(data.Length > 0) {
                NoGuilds.SetActive(false);
                for(int i = 0; i < data.Length; i++)
                    content.GetChild(i).GetComponent<UIJoinGuildSlot>().Set(data[i]);
            }
        }
        public void OnShowCreateWindow() {
            GuildName.characterLimit = Storage.data.guild.maxNameLength;
            GuildName.text = "";
        }
        public void OnCreate(GameObject window) {
            if(!IsValidGuildName(GuildName.text))
                Notify.InvalidName();
            else if(player.own.gold < Storage.data.guild.creationPriceGold)
                Notify.DontHaveEnoughGold();
            else {
                player.CmdCreateGuild(GuildName.text);
                window.SetActive(false);
            }
        }
        void Check() { // if accepted in guild switch to guild home page
            if(player.InGuild())
                UIManager.data.guildWindow.Show();
        }
        bool IsValidGuildName(string guildName)
        {
            return guildName.Length <= Storage.data.guild.maxNameLength &&
                Regex.IsMatch(guildName, @"^[a-zA-Z0-9_]+$");
        }
        void OnEnable() {
            if(player != null && !player.InGuild()) { // guard
                player.CmdGetAvailableGuildsToJoin();
            }
            else gameObject.SetActive(false);
            InvokeRepeating("Check", 0, 1f);
        }
        void OnDisable() {
            if(content.childCount > 0) {
                for(int i = 0; i < content.childCount; i++)
                    Destroy(content.GetChild(i).gameObject);
            }
            NoGuilds.SetActive(true);
            CancelInvoke("Check");
        }
    }
}