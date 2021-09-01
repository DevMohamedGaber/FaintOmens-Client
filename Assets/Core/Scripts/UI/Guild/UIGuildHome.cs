using UnityEngine;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class UIGuildHome : MonoBehaviour {
        [Header("General")]
        [SerializeField] float updateInterval = 2f;
        [Header("Info")]
        [SerializeField] RTLTextMeshPro guildName;
        [SerializeField] TMP_Text guildLevel;
        [SerializeField] RTLTextMeshPro masterName;
        [SerializeField] TMP_Text membersCount;
        [SerializeField] TMP_Text myContribution;
        [SerializeField] TMP_Text guildWealth;
        [SerializeField] TMP_Text guildWood;
        [SerializeField] TMP_Text guildStone;
        [SerializeField] TMP_Text guildIron;
        [SerializeField] TMP_Text guildFood;
        [SerializeField] RTLTextMeshPro guildNotice;
        [SerializeField] GameObject editNoticeBtn;
        [SerializeField] TMP_Text currentLevel;
        [SerializeField] RTLTextMeshPro nextLevel;
        [SerializeField] UnityEngine.UI.Slider expBar;
        [SerializeField] TMP_Text expText;
        Player player => Player.localPlayer;
        Guild guild => player.own.guild;
        void UpdateData() {
            guildLevel.text = guild.level.ToString();
            membersCount.text = $"{guild.membersCount} / {guild.capacity}";
            myContribution.text = player.own.guildContribution.ToString();
            guildWealth.text = guild.assets.wealth.ToString();
            guildWood.text = guild.assets.wood.ToString();
            guildStone.text = guild.assets.stone.ToString();
            guildIron.text = guild.assets.iron.ToString();
            guildFood.text = guild.assets.food.ToString();
            guildNotice.text = guild.notice != "" ? guild.notice : LanguageManger.Decide("No Further Notice", "لا يوجد اعلان");
            editNoticeBtn.SetActive(player.own.guildRank >= Storage.data.guild.notifyMinRank);
            currentLevel.text = guild.level.ToString();
            bool notMax = guild.level + 1 < Storage.data.guild.maxLevel;
            nextLevel.text = notMax ? (guild.level - 1).ToString() : LanguageManger.Decide("Max", "اعلي مستوي");
            expBar.maxValue = notMax ? guild.expMax : 1;
            expBar.value = notMax ? guild.exp : 1;
            expText.text = notMax ? $"{guild.exp} / {guild.expMax}" : "";
        }
        public void OnLeave() {
            if(player.own.guildRank == GuildRank.Master)
                player.CmdTerminateGuild();
            else 
                player.CmdLeaveGuild();
        }
        void OnEnable() {
            guildName.text = guild.Name;
            masterName.text = guild.masterName;
            InvokeRepeating("UpdateData", 0, updateInterval);
        }
        void OnDisable() {
            CancelInvoke("UpdateData");
        }
    }
}