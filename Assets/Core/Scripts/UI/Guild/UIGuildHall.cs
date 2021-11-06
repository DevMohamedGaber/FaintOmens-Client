using UnityEngine;
using System;
using TMPro;
namespace Game.UI
{
    /*public class UIGuildHall : MonoBehaviour {
        [SerializeField] float updateInterval = 2f;
        [SerializeField] UIGuildHall_Assets assets;
        [SerializeField] UIGuildHall_Building hall;
        [SerializeField] UIGuildHall_Building academy;
        Player player => Player.localPlayer;
        Guild guild => player.own.guild;
        bool canUpgrade => player.own.guildRank == Storage.data.guild.promoteMinRank;
        void UpdateData() {
            UpdateAssets();
            UpdateHall();
        }
        void UpdateAssets() {
            assets.level.text = $"{guild.level}{(guild.level < Storage.data.guild.maxLevel ? $" ({guild.expPerc}%)" : " (MAX)")}";
            assets.wealth.text = guild.assets.wealth.ToString();
            assets.wood.text = guild.assets.wood.ToString();
            assets.stone.text = guild.assets.stone.ToString();
            assets.iron.text = guild.assets.iron.ToString();
            assets.food.text = guild.assets.food.ToString();
        }
        void UpdateHall() {
            hall.level.text = guild.buildings.hall.ToString();
            hall.variable.text = guild.capacity.ToString();
            bool notMaxed = guild.buildings.hall != Storage.data.guild.maxLevel;
            hall.reqsObj.SetActive(notMaxed);
            hall.maxedObj.SetActive(!notMaxed);
            if(notMaxed) {
                GuildAssets req = hall.reqs[guild.buildings.hall];

                hall.gLevel.text = (guild.buildings.hall + 1).ToString();
                hall.gLevel.color = guild.level >=  guild.buildings.hall + 1 ? Color.white : Color.red;

                hall.wealth.text = req.wealth.ToString();
                hall.wealth.color = guild.assets.wealth >= req.wealth ? Color.white : Color.red;

                hall.wood.text = req.wood.ToString();
                hall.wood.color = guild.assets.wood >= req.wood ? Color.white : Color.red;

                hall.stone.text = req.stone.ToString();
                hall.stone.color = guild.assets.stone >= req.stone ? Color.white : Color.red;
                
                hall.iron.text = req.iron.ToString();
                hall.iron.color = guild.assets.iron >= req.iron ? Color.white : Color.red;
                
                hall.food.text = req.food.ToString();
                hall.food.color = guild.assets.food >= req.food ? Color.white : Color.red;
            }
        }
        public void OnUpgradeHall() {
            if(!canUpgrade) {
                Notifications.list.Add("You don't have permission to upgrade", "ليس لديك الصلاحية لتطوير");
                return;
            }
            if(guild.buildings.hall == Storage.data.guild.maxLevel) {
                Notifications.list.Add("Hall already reached max level", "وصلت القاعة لاعلي مستوي بالفعل");
                return;
            }
            if(guild.level < guild.buildings.hall + 1) {
                Notifications.list.Add($"Guild level {guild.buildings.hall + 1} is required", $"يلزم مستوي النقابة {guild.buildings.hall + 1}");
                return;
            }
            GuildAssets req = hall.reqs[guild.buildings.hall];
            if(guild.assets.wealth < req.wealth) {
                Notifications.list.Add("Didn't meet the Wealth required to upgrade the Hall", "لم تحقق الثروة المطلوبة لتطوير القاعة");
                return;
            }
            if(guild.assets.wood < req.wood) {
                Notifications.list.Add("Didn't meet the Wood required to upgrade the Hall", "لم تحقق الخشب المطلوبة لتطوير القاعة");
                return;
            }
            if(guild.assets.stone < req.stone) {
                Notifications.list.Add("Didn't meet the Stone required to upgrade the Hall", "لم تحقق الحجارة المطلوبة لتطوير القاعة");
                return;
            }
            if(guild.assets.iron < req.iron) {
                Notifications.list.Add("Didn't meet the Iron required to upgrade the Hall", "لم تحقق الحديد المطلوبة لتطوير القاعة");
                return;
            }
            if(guild.assets.food < req.food) {
                Notifications.list.Add("Didn't meet the Food required to upgrade the Hall", "لم تحقق الطعام المطلوبة لتطوير القاعة");
                return;
            }
            player.CmdGuildUpgradeHall();
            Invoke("UpdateData", .5f);
        }
        void OnEnable() => InvokeRepeating("UpdateData", 0, updateInterval);
        void OnDisable() => CancelInvoke("UpdateData");
        [Serializable] struct UIGuildHall_Assets {
            public TMP_Text level;
            public TMP_Text wealth;
            public TMP_Text wood;
            public TMP_Text stone;
            public TMP_Text iron;
            public TMP_Text food;
        }
        [Serializable] struct UIGuildHall_Building {
            public GuildAssets[] reqs;
            public TMP_Text level;
            public TMP_Text variable;
            public GameObject reqsObj;
            public GameObject maxedObj;
            public TMP_Text gLevel;
            public TMP_Text wealth;
            public TMP_Text wood;
            public TMP_Text stone;
            public TMP_Text iron;
            public TMP_Text food;
        }
    }*/
}