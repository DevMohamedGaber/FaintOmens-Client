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
            UIPreviewManager.singleton.UpdateLocalPlayer();
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
            UIPreviewManager.singleton.InstantiateLocalPlayer();
        }
        /*[Header("General")]
        [SerializeField, Range(0, 1)] float updateInterval = .3f;
        [SerializeField] UILanguageDefiner lang;
        [Header("Preview")]
        [SerializeField] RTLTMPro.RTLTextMeshPro nameText;
        [SerializeField] Text goldText;
        [SerializeField] Text diamondsText;
        [SerializeField] Text b_diamondsText;
        [SerializeField] GameObject equipSlotsObj;
        [SerializeField] GameObject equipBtn;
        [SerializeField] UIItemSlot[] equipSlots;
        [SerializeField] GameObject accSlotsObj;
        [SerializeField] GameObject accBtn;
        [SerializeField] UIEquipmentSlot[] acctSlots;
        [Header("Info")]
        [SerializeField] Slider expBar;
        [SerializeField] Text expText;
        [SerializeField] Image militaryRankBadge;
        [SerializeField] Image titleImage;
        [Header("Military")]
        [SerializeField] GameObject nextMilitaryRank;
        [SerializeField] GameObject maxMilitaryRank;
        Player player => Player.localPlayer;
        int window = 0; // 0 => equip / 1 => acce
        bool mpShown;
        void UpdateData() {
            goldText.text = player.own.gold.ToString();
            diamondsText.text = player.own.diamonds.ToString();
            b_diamondsText.text = player.own.b_diamonds.ToString();
            UpdatePreview();
            UpdateInfo();
            if(mpShown) UpdateMilitary();
        }
        void UpdateInfo() {
            lang.SetSuffix(2, Colorize(player.id.ToString()));
            lang.SetSuffix(3, Colorize(player.classInfo.Name));
            lang.SetSuffix(4, Colorize(player.level.ToString()));
            lang.SetSuffix(5, Colorize(player.InGuild() ? player.guild.name : "-"));
            lang.SetSuffix(6, Colorize(LanguageManger.GetWord(player.tribeId, LanguageDictionaryCategories.Tribe)));
            lang.SetSuffix(7, Colorize(LanguageManger.GetWord((int)player.own.tribeRank + 5, LanguageDictionaryCategories.Tribe)));
            lang.SetSuffix(8, Colorize(player.own.TodayHonor + " / " + player.MaxHonorPerDay));
            lang.SetSuffix(9, Colorize(player.own.TotalHonor.ToString()));
            lang.SetSuffix(10, Colorize(player.own.killStrike.ToString()));
            lang.SetSuffix(11, Colorize(player.own.MonsterPoints.ToString()));
            lang.SetSuffix(12, Colorize(player.own.militaryRank > -1 ? 
            LanguageManger.GetWord(player.own.militaryRank, LanguageDictionaryCategories.MilitaryRank) : "-", 
            ScriptableMilitaryRank.dict[player.own.militaryRank].hexColor));
            expBar.value = player.own.experience;
            expBar.maxValue = player.own.experienceMax;
            expText.text = player.own.experience + " / " + player.own.experienceMax;
            militaryRankBadge.sprite = ScriptableMilitaryRank.dict[player.own.militaryRank].icon;
            if(player.activeTitle > 0) {
                titleImage.gameObject.SetActive(true);
                titleImage.sprite = ScriptableTitle.dict[player.activeTitle].GetImage();
            }
            else titleImage.gameObject.SetActive(false);
            //attributes
            lang.SetSuffix(17, Colorize(player.healthMax.ToString()));
            lang.SetSuffix(18, Colorize(player.manaMax.ToString()));
            lang.SetSuffix(19, Colorize(player.p_atk.ToString()));
            lang.SetSuffix(20, Colorize(player.p_def.ToString()));
            lang.SetSuffix(21, Colorize(player.m_atk.ToString()));
            lang.SetSuffix(22, Colorize(player.m_def.ToString()));
            lang.SetSuffix(23, Colorize(player.blockChance.ToString("F0") + "%"));
            lang.SetSuffix(24, Colorize(player.untiBlockChance.ToString("F0") + "%"));
            lang.SetSuffix(25, Colorize(player.critRate.ToString("F0") + "%"));
            lang.SetSuffix(26, Colorize(player.critDmg.ToString("F0") + "%"));
            lang.SetSuffix(27, Colorize(player.antiCrit.ToString("F0") + "%"));
            lang.SetSuffix(28, Colorize(player.untiStunChance.ToString("F0") + "%"));
            lang.SetSuffix(29, Colorize(player.speed.ToString("F0") + "%"));
            //AP
            lang.SetSuffix(30, Colorize(player.own.vitality.ToString()));
            lang.SetSuffix(31, Colorize(player.own.strength.ToString()));
            lang.SetSuffix(32, Colorize(player.own.intelligence.ToString()));
            lang.SetSuffix(33, Colorize(player.own.endurance.ToString()));
            lang.SetSuffix(34, Colorize(player.own.freepoints.ToString()));
            lang.SetSuffix(35, Colorize(player.healthRecoveryRate + "/" + (LanguageManger.isEn ? "s" : "ث")));
            lang.SetSuffix(36, Colorize(player.manaRecoveryRate + "/" + (LanguageManger.isEn ? "s" : "ث")));

            lang.RefreshRange(1, 36);
        }
        void UpdatePreview() {
            nameText.text = player.name;
            lang.SetSuffix(1, player.battlepower.ToString());
            if(window == 0) {
                for(int i = 0; i < player.equipment.Count; i++)
                    if(player.equipment[i].amount > 0) equipSlots[i].Assign(player.equipment[i], i);
                    else equipSlots[i].Unassign();
            }
            else if(window == 1) {
                for(int i = 0; i < player.own.accessories.Count; i++)
                    if(player.own.accessories[i].amount > 0) acctSlots[i].Assign(player.own.accessories[i], i);
                    else acctSlots[i].Unassign();
            }
        }
        void UpdateMilitary() {
            if(ScriptableMilitaryRank.dict.TryGetValue(player.own.militaryRank, out ScriptableMilitaryRank info)) {
                //current
                lang.SetSuffix(40, Colorize(LanguageManger.GetWord(info.name, LanguageDictionaryCategories.MilitaryRank), info.hexColor));
                MilitaryRankIntBonusHandler(41, info.hp);
                MilitaryRankIntBonusHandler(42, info.mp);
                MilitaryRankIntBonusHandler(43, info.hpRec);
                MilitaryRankIntBonusHandler(44, info.mpRec);
                MilitaryRankIntBonusHandler(45, player.classType == DamageType.Physical ? info.atk : 0);
                MilitaryRankIntBonusHandler(46, player.classType == DamageType.Physical ? info.def : 0);
                MilitaryRankIntBonusHandler(47, player.classType == DamageType.Magical ? info.atk : 0);
                MilitaryRankIntBonusHandler(48, player.classType == DamageType.Magical ? info.def : 0);
                MilitaryRankFloatBonusHandler(49, info.block);
                MilitaryRankFloatBonusHandler(50, info.untiBlock);
                MilitaryRankFloatBonusHandler(51, info.crit);
                MilitaryRankFloatBonusHandler(52, info.critDmg);
                MilitaryRankFloatBonusHandler(53, info.untiCrit);
                MilitaryRankFloatBonusHandler(54, info.untiStun);
                MilitaryRankFloatBonusHandler(55, info.speed);
                lang.RefreshRange(40, 55);
                //next
                nextMilitaryRank.SetActive(info.next != null);
                maxMilitaryRank.SetActive(info.next == null);
                if(info.next != null) {
                    lang.SetSuffix(56, Colorize(LanguageManger.GetWord(info.next.name, LanguageDictionaryCategories.MilitaryRank), info.next.hexColor));
                    MilitaryRankIntBonusHandler(57, info.next.hp);
                    MilitaryRankIntBonusHandler(58, info.next.mp);
                    MilitaryRankIntBonusHandler(59, info.next.hpRec);
                    MilitaryRankIntBonusHandler(60, info.next.mpRec);
                    MilitaryRankIntBonusHandler(61, player.classType == DamageType.Physical ? info.next.atk : 0);
                    MilitaryRankIntBonusHandler(62, player.classType == DamageType.Physical ? info.next.def : 0);
                    MilitaryRankIntBonusHandler(63, player.classType == DamageType.Magical ? info.next.atk : 0);
                    MilitaryRankIntBonusHandler(64, player.classType == DamageType.Magical ? info.next.def : 0);
                    MilitaryRankFloatBonusHandler(65, info.next.block);
                    MilitaryRankFloatBonusHandler(66, info.next.untiBlock);
                    MilitaryRankFloatBonusHandler(67, info.next.crit);
                    MilitaryRankFloatBonusHandler(68, info.next.critDmg);
                    MilitaryRankFloatBonusHandler(69, info.next.untiCrit);
                    MilitaryRankFloatBonusHandler(70, info.next.untiStun);
                    MilitaryRankFloatBonusHandler(71, info.next.speed);
                    lang.RefreshRange(56, 71);
                }
                //requirements
                lang.SetSuffix(73, Colorize(info.level.ToString(), player.level >= info.level ? "FFFFFFFF" : "FF0000"));
                lang.SetSuffix(74, Colorize(info.honor.ToString(), player.own.TotalHonor >= info.honor ? "FFFFFFFF" : "FF0000"));
                lang.SetSuffix(75, Colorize(info.monsterPoints.ToString(), player.own.MonsterPoints >= info.monsterPoints ? "FFFFFFFF" : "FF0000"));
                lang.RefreshRange(73, 75);
            }
        }
        void MilitaryRankIntBonusHandler(int index, int value) {
            lang.SetActive(index, value > 0);
            if(value > 0) lang.SetSuffix(index, ColorizeInc(value.ToString()));
        }
        void MilitaryRankFloatBonusHandler(int index, float value) {
            lang.SetActive(index, value > 0);
            if(value > 0) lang.SetSuffix(index, ColorizeInc(value.ToString("F0") + "%"));
        }
        string Colorize(string str, string hex = "FFFFFFFF") => ": <color=#" + hex + ">" + str + "</color>";
        string ColorizeInc(string str, string hex = "FFFFFFFF") => "+ <color=#" + hex + ">" + str + "</color>";
        public void SwitchStuff(int type) {
            equipSlotsObj.SetActive(type == 0);
            equipBtn.SetActive(type != 0);

            accSlotsObj.SetActive(type == 1);
            accBtn.SetActive(type != 1);

            window = type;
        }
        public void OnIncreaseVitality() {
            if(player.own.freepoints > 0) player.CmdIncreaseVitality();
            else Notifications.list.Add("No available points to use", "لا يوجد نقاط قدرة كافية");
        }
        public void OnIncreaseStrength() {
            if(player.own.freepoints > 0) player.CmdIncreaseStrength();
            else Notifications.list.Add("No available points to use", "لا يوجد نقاط قدرة كافية");
        }
        public void OnIncreaseIntelligence() {
            if(player.own.freepoints > 0) player.CmdIncreaseIntelligence();
            else Notifications.list.Add("No available points to use", "لا يوجد نقاط قدرة كافية");
        }
        public void OnIncreaseEndurance() {
            if(player.own.freepoints > 0) player.CmdIncreaseEndurance();
            else Notifications.list.Add("No available points to use", "لا يوجد نقاط قدرة كافية");
        }
        public void OnPromote() {
            if(ScriptableMilitaryRank.dict.TryGetValue(player.own.militaryRank, out ScriptableMilitaryRank rank)) {
                if(rank.CanPromote(player)) player.CmdPromoteMilitaryRank();
                else Notifications.list.Add("You didn't meet the requirements yet", "لم توافي الشروط بعد");
            }
        }
        public void SetMPShown(bool status) => mpShown = status;
        public void Show() => gameObject.SetActive(true);
        void OnEnable() {
            if(player != null) {
                InvokeRepeating(nameof(UpdateData), 0, updateInterval);
                UIPreviewManager.singleton.InstantiatePlayer(player);
            }
        }
        void OnDisable() {
            CancelInvoke(nameof(UpdateData));
            UIPreviewManager.singleton.Clear();
        }*/
    }
}