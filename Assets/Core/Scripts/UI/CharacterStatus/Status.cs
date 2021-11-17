using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class Status : Window, IWindowWithEquipments
    {
        [SerializeField] Status_Info info;
        [SerializeField] Status_Attributes attributes;
        [SerializeField] CharacterStats stats;

        public override void Refresh()
        {
            base.Refresh();
            
            SetInfo();
            SetAttributes();
            if(stats != null)
            {
                stats.Set(player);
            }
        }
        void SetInfo()
        {
            if(info.spouseTxt != null)
            {
                info.spouseTxt.text = player.own.marriage.spouseName;
            }
            if(info.guildTxt != null)
            {
                info.guildTxt.text = player.guild.name;
            }
            if(info.brTxt != null)
            {
                info.brTxt.text = player.battlepower.ToString();
            }
            if(info.lvlTxt != null)
            {
                info.lvlTxt.text = player.level.ToString();
            }
            if(info.honorTxt != null)
            {
                info.honorTxt.text = $"{player.own.TotalHonor} ({player.own.TodayHonor} / {player.MaxHonorPerDay})";
            }
            if(info.expTxt != null)
            {
                info.expTxt.text = $"{player.own.experience} / {player.own.experienceMax}";
            }
            if(info.expBar != null)
            {
                info.expBar.fillAmount = player.own.experience / player.own.experienceMax;
            }
            if(info.vipTxt != null)
            {
                info.vipTxt.text = ((int)player.own.vip.level).ToString();
            }
            if(info.avatar != null)
            {
                info.avatar.sprite = UIManager.data.assets.avatars[(int)player.avatar];
            }
            if(info.title != null)
            {
                if(player.activeTitle > 0 && ScriptableTitle.dict.TryGetValue(player.activeTitle, out ScriptableTitle titleData))
                {
                    info.title.gameObject.SetActive(true);
                    info.title.sprite = titleData.GetImage();
                }
                else
                {
                    info.title.gameObject.SetActive(false);
                }
            }
        }
        void SetAttributes()
        {
            attributes.Set(player);
        }
        public void RefreshEquipments()
        {
            PreviewManager.singleton.UpdateLocalPlayer();
        }
        public void OnClickID()
        {
            UIUtils.AddToClipboard(player.id);
        }
        public void OnIncreaseVitality()
        {
            if(player.own.freepoints > 0)
            {
                player.CmdIncreaseVitality();
            }
            else
            {
                Notifications.list.Add("No available points to use", "لا يوجد نقاط قدرة كافية");
            }
        }
        public void OnIncreaseStrength()
        {
            if(player.own.freepoints > 0)
            {
                player.CmdIncreaseStrength();
            }
            else
            {
                Notifications.list.Add("No available points to use", "لا يوجد نقاط قدرة كافية");
            }
        }
        public void OnIncreaseIntelligence()
        {
            if(player.own.freepoints > 0)
            {
                player.CmdIncreaseIntelligence();
            }
            else
            {
                Notifications.list.Add("No available points to use", "لا يوجد نقاط قدرة كافية");
            }
        }
        public void OnIncreaseEndurance()
        {
            if(player.own.freepoints > 0)
            {
                player.CmdIncreaseEndurance();
            }
            else
            {
                Notifications.list.Add("No available points to use", "لا يوجد نقاط قدرة كافية");
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            if(info.nameTxt != null)
            {
                info.nameTxt.text = player.name;
            }
            if(info.classTxt != null)
            {
                info.classTxt.text = player.classInfo.Name;
                info.classTxt.color = player.classInfo.data.qualityData.color;
            }
            if(info.idTxt != null)
            {
                info.idTxt.text = player.id.ToString();
            }
            PreviewManager.singleton.InstantiateLocalPlayer();
        }
    }
}