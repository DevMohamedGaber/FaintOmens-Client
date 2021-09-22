using UnityEngine;
using System;
using System.Linq;
using TMPro;
namespace Game.UI
{
    public class UIGuildDonation : MonoBehaviour
    {
        [SerializeField] float updateInterval = 1f;
        [SerializeField] TMP_Text myGold;
        [SerializeField] TMP_Text myDiamonds;
        [SerializeField] TMP_Text myGP;
        [SerializeField] TMP_Text guildContribution;
        [SerializeField] TMP_InputField goldInput;
        [SerializeField] TMP_InputField diamonsInput;
        Player player => Player.localPlayer;
        void UpdateData()
        {
            myGold.text = player.own.gold.ToString();
            myDiamonds.text = player.own.diamonds.ToString();
            myGP.text = player.own.guildContribution.ToString();
            guildContribution.text = player.own.guild.assets.wealth.ToString();
        }
        public void OnDonateGold()
        {
            uint input = Convert.ToUInt32((string)goldInput.text);
            if(input < Storage.data.guild.goldToContribution)
            {
                if(player.own.gold >= Storage.data.guild.goldToContribution)
                    goldInput.text = Storage.data.guild.goldToContribution.ToString();
                Notifications.list.Add($"Minimum of {Storage.data.guild.goldToContribution} gold has to be donated", $"مطلوب {Storage.data.guild.goldToContribution} ذهب كحد ادني");
                return;
            }
            if(player.own.gold < input) {
                Notify.DontHaveEnoughGold();
                return;
            }
            player.CmdGuildDonateGold(input);
            goldInput.text = "";
        }
        public void OnDonateDiamonds() {
            uint input = Convert.ToUInt32((string)diamonsInput.text);
            if(input < Storage.data.guild.diamondToContribution) {
                if(player.own.diamonds >= Storage.data.guild.diamondToContribution)
                    diamonsInput.text = Storage.data.guild.diamondToContribution.ToString();
                Notifications.list.Add($"Minimum of {Storage.data.guild.diamondToContribution} diamonds has to be donated", $"مطلوب {Storage.data.guild.diamondToContribution} الماس كحد ادني");
                return;
            }
            if(player.own.diamonds < input) {
                Notify.DontHaveEnoughDiamonds();
                return;
            }
            player.CmdGuildDonateDiamonds(input);
            diamonsInput.text = "";
        }
        public void GetTodayRanking() {}
        public void GetTotalRanking() {}
        public void SetRankingData() {}
        void OnEnable() {
            InvokeRepeating("UpdateData", 0, updateInterval);
            GetTodayRanking();
        }
        void OnDisable() {
            CancelInvoke("UpdateData");
        }
    }
}