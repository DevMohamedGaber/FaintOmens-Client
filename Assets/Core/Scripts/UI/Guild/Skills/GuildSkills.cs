using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class GuildSkills : SubWindow
    {
        [SerializeField] GuildSkillRow[] skills;
        [SerializeField] TMP_Text myGPTxt;
        int currentIndex;
        public override void Refresh()
        {
            
        }
        /*[SerializeField] float updateInterval = 1f;
        [SerializeField] UIGuildSkillRow[] skills;
        [SerializeField] TMP_Text myGPTxt;
        Player player => Player.localPlayer;
        byte academy => player.own.guild.buildings.academy;
        void UpadteData() {
            myGPTxt.text = player.own.guildContribution.ToString();
            for(int i = 0; i < player.own.guildSkills.Count; i++)
                skills[i].Set(i, player.own.guildSkills[i], academy, player.own.guildContribution);
        }
        public void OnLearn(int skill) {
            if(skill < 0 || skill > player.own.guildSkills.Count) {
                Notify.SomethingWentWrong();
                return;
            }
            byte lvl = player.own.guildSkills[skill];
            if(lvl == Storage.data.guild.maxLevel) {
                Notifications.list.Add("Skill already reached max level", "المهارة وصلت لاعلي مستوي بالفعل");
                return;
            }
            if(academy < lvl + 1) {
                Notifications.list.Add($"Academy level {lvl + 1} is required", $"مطلوب اكاديمية مستوي {lvl + 1}");
                return;
            }
            player.CmdLearnGuildSkill(skill);
        }
        void OnEnable() {
            InvokeRepeating("UpadteData", 0, updateInterval);
        }
        void OnDisable() {
            CancelInvoke("UpadteData");
        }*/
    }
}