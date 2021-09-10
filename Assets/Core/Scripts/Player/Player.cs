using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Mirror;
using RTLTMPro;
using Game.Components;
using Game.Network;
using Game.UI;
namespace Game
{
    public class Player : Entity
    {
        public static Player localPlayer;
        public static Dictionary<uint, Player> onlinePlayers = new Dictionary<uint, Player>();
    #region Static Variables
        [Header("Player Static")]
        public ChatComponent chat;
        public PlayerOwnData own;
        [SerializeField] NetworkNavMeshAgentRubberbanding rubberbanding;
        [SerializeField] FaceCamera onPlayerInfoOverlay;
        [SerializeField] SpriteRenderer tribeOverlay;
        [SerializeField] SpriteRenderer titleOverlay;
        [SerializeField] RTLTextMeshPro3D nameText;
        [SerializeField] RTLTextMeshPro3D guildText;
        public Transform bodyHolder;
        Transform rWeaponHolder;
        Transform lWeaponHolder;
        Transform wingsHolder;
        Transform spiritHolder;
        [Header("Interaction")]
        public float interactionRange = 4;
        public KeyCode targetNearestKey = KeyCode.Tab;
        public bool localPlayerClickThrough = true; // click selection goes through localplayer. feels best.
        public KeyCode cancelActionKey = KeyCode.Escape;
        public Transform meshToOffsetWhenMounted;
        public float seatOffsetY = -1;
    #endregion
    #region Sync
        [Header("Player Synced")]
        [SyncVar] public uint id;
        [SyncVar(hook ="OnStateChanged")] public EntityState state = EntityState.Idle;
        /*[SyncVar(hook ="OnLevelChanged")] byte _level;
        public int level {
            get => (int)_level;
            set => _level = (byte)value;
        }*/
        [SyncVar] public PlayerClassData classInfo;
        [SyncVar(hook = "OnGenderChanged")] public Gender gender;
        [SyncVar(hook = "OnCityChanged")] public byte cityId;
        [SyncVar] public PrivacyLevel privacy;
        [SyncVar(hook = "OnAvatarChanged")] public byte avatar;
        [SyncVar(hook = "OnFrameChanged")] public byte frame;
        [SyncVar] public byte tribeId;
        [SyncVar(hook = "OnGuildInfoChanged")] public GuildPublicInfo guild;
        [SyncVar(hook = "OnTeamChanged")] public uint teamId;
        [SyncVar(hook = "OnTitleChanged")] public ushort activeTitle;
        [SyncVar(hook = "OnWardrobeVisibilityChanged")] public bool showWardrop;
        [SyncVar(hook = "OnActiveMountChanged")] public ActiveMount mount;
        [SyncVar] GameObject _nextTarget;
        [SyncVar] GameObject _activePet;
        public SyncListItemSlot equipment = new SyncListItemSlot();
        public SyncListWardrop wardrobe = new SyncListWardrop();
    #endregion
    #region local
        public IAnimationEventEffects animationEventEffects;
        ScriptableTotalGemLevels totalGemLevelBonus;
        ScriptableTotalPlusLevels totalPlusLevelBonus;
        public Transform mainWeaponHolder => classInfo.type == PlayerClass.Archer ? lWeaponHolder : rWeaponHolder;
        public Transform secondaryWeaponHolder => classInfo.type == PlayerClass.Archer ? rWeaponHolder : lWeaponHolder;
        MountBody mountObj;
        GameObject indicator;
        public ScribtableCity city => ScribtableCity.dict[cityId];

        public Entity nextTarget => _nextTarget != null  ? _nextTarget.GetComponent<Entity>() : null;
        public Pet activePet => _activePet != null  ? _activePet.GetComponent<Pet>() : null;
        public Vector3 petDestination => transform.position - transform.right * collider.bounds.size.x;
        public double allowedLogoutTime => lastCombatTime + ((NetworkManagerMMO)NetworkManager.singleton).combatLogoutDelay;
        public double remainingLogoutTime => NetworkTime.time < allowedLogoutTime ? (allowedLogoutTime - NetworkTime.time) : 0;
        public int MaxHonorPerDay => 6000 + own.vip.data.bonusHonor;
        int respawnRequested = -1;
        int useSkillWhenCloser = -1;
        Dictionary<string, Transform> skinBones = new Dictionary<string, Transform>();
        Camera cam; // Camera.main calls FindObjectWithTag each time. cache it!
        int pendingSkill = -1;
        Vector3 pendingDestination;
        bool pendingDestinationValid;
        Vector3 pendingVelocity;
        bool pendingVelocityValid;
        #endregion
    #region Attributes
        #region Basics(Health/Mana/Speed)
        public override int healthMax {
            get {
                int i, result = own.vitality * Storage.data.AP_Vitality + classInfo.hp;
                for(i = 0; i < equipment.Count; i++) {
                    if(equipment[i].amount > 0 && equipment[i].item.data != null)
                        result += (int)equipment[i].item.GetSocketOfType(BonusType.hp) + equipment[i].item.health;
                }
                for(i = 0; i < own.accessories.Count; i++) {
                    if(own.accessories[i].amount > 0 && own.accessories[i].item.data != null)
                        result += (int)own.accessories[i].item.GetSocketOfType(BonusType.hp) + own.accessories[i].item.health;
                }
                result += activePet != null ? activePet.healthMax : 0;
                if(own.pets.Count > 0) {
                    for(i = 0; i < own.pets.Count; i++) {
                        if(own.pets[i].status == SummonableStatus.Saved)
                            result += own.pets[i].healthMax / 2;
                    }
                }
                if(own.mounts.Count > 0) {
                    for(i = 0; i < own.mounts.Count; i++)
                        result += own.mounts[i].status == SummonableStatus.Saved ? own.mounts[i].healthMax / 2 : own.mounts[i].healthMax;
                }
                result += own.militaryRank > 0 ? ScriptableMilitaryRank.dict[own.militaryRank].hp : 0;
                if(own.titles.Count > 0) {
                    for(i = 0; i < own.titles.Count; i++) {
                        if(ScriptableTitle.dict.TryGetValue(own.titles[i], out ScriptableTitle title))
                            result += own.titles[i] == activeTitle ? title.hp.active : title.hp.notActive;
                    }
                }
                result += InGuild() ? ScriptableGuildSkill.dict[0].Get(own.guildSkills[0]) * Storage.data.AP_Vitality : 0;
                return base.healthMax + result;
            }
        }
        public override int manaMax {
            get {
                int i, result = own.intelligence * Storage.data.AP_Intelligence_MANA + classInfo.mp;
                for(i = 0; i < equipment.Count; i++) {
                    if(equipment[i].amount > 0 && equipment[i].item.data != null)
                        result += equipment[i].item.mana;
                }
                for(i = 0; i < own.accessories.Count; i++) {
                    if(own.accessories[i].amount > 0 && own.accessories[i].item.data != null)
                        result += own.accessories[i].item.mana;
                }
                result += activePet != null ? activePet.manaMax : 0;
                if(own.pets.Count > 0) {
                    for(i = 0; i < own.pets.Count; i++) {
                        if(own.pets[i].status == SummonableStatus.Saved)
                            result += own.pets[i].manaMax / 2;
                    }
                }
                if(own.mounts.Count > 0) {
                    for(i = 0; i < own.mounts.Count; i++)
                        result += own.mounts[i].status == SummonableStatus.Saved ? own.mounts[i].manaMax / 2 : own.mounts[i].manaMax;
                }
                result += own.militaryRank > 0 ? ScriptableMilitaryRank.dict[own.militaryRank].mp : 0;
                if(own.titles.Count > 0) {
                    for(i = 0; i < own.titles.Count; i++) {
                        if(ScriptableTitle.dict.TryGetValue(own.titles[i], out ScriptableTitle title))
                            result += own.titles[i] == activeTitle ? title.mp.active : title.mp.notActive;
                    }
                }
                result += InGuild() ? ScriptableGuildSkill.dict[2].Get(own.guildSkills[2]) * Storage.data.AP_Intelligence_MANA : 0;
                return base.manaMax + result;
            }
        }
        public override float speed {
            get {
                return base.speed + (IsMounted() ? own.mounts[mountObj.dataIndex].speed : 0);
            }
        }
        #endregion
        #region Attack
        public override int p_atk {
            get {
                int i, result = own.strength * Storage.data.AP_Strength_ATK + classInfo.pAtk;
                for(i = 0; i < equipment.Count; i++) {
                    if(equipment[i].amount > 0 && equipment[i].item.data != null)
                        result += (int)equipment[i].item.GetSocketOfType(BonusType.pAtk) + equipment[i].item.pAtk;
                }
                for(i = 0; i < own.accessories.Count; i++) {
                    if(own.accessories[i].amount > 0 && own.accessories[i].item.data != null)
                        result += (int)own.accessories[i].item.GetSocketOfType(BonusType.pAtk) + own.accessories[i].item.pAtk;
                }
                result += activePet != null ? activePet.p_atk : 0;
                if(own.pets.Count > 0) {
                    for(i = 0; i < own.pets.Count; i++) {
                        if(own.pets[i].status == SummonableStatus.Saved)
                            result += own.pets[i].p_atk / 2;
                    }
                }
                if(own.mounts.Count > 0) {
                    for(i = 0; i < own.mounts.Count; i++)
                        result += own.mounts[i].status == SummonableStatus.Saved ? own.mounts[i].p_atk / 2 : own.mounts[i].p_atk;
                }
                result += own.militaryRank > 0 && classType == DamageType.Physical ? ScriptableMilitaryRank.dict[own.militaryRank].atk : 0;
                if(classType == DamageType.Physical && own.titles.Count > 0) {
                    for(i = 0; i < own.titles.Count; i++)
                        if(ScriptableTitle.dict.TryGetValue(own.titles[i], out ScriptableTitle title))
                            result += own.titles[i] == activeTitle ? title.pAtk.active : title.pAtk.notActive;
                }
                result += InGuild() ? ScriptableGuildSkill.dict[1].Get(own.guildSkills[1]) * Storage.data.AP_Strength_ATK : 0;
                return base.p_atk + result;
            }
        }
        public override int m_atk {
            get {
                int i, result = own.intelligence * Storage.data.AP_Intelligence_ATK + classInfo.mAtk;
                for(i = 0; i < equipment.Count; i++) {
                    if(equipment[i].amount > 0 && equipment[i].item.data != null)
                        result += (int)equipment[i].item.GetSocketOfType(BonusType.mAtk) + equipment[i].item.mAtk;
                }
                for(i = 0; i < own.accessories.Count; i++) {
                    if(own.accessories[i].amount > 0 && own.accessories[i].item.data != null)
                        result += (int)own.accessories[i].item.GetSocketOfType(BonusType.mAtk) + own.accessories[i].item.mAtk;
                }
                result += activePet != null ? activePet.m_atk : 0;
                if(own.pets.Count > 0) {
                    for(i = 0; i < own.pets.Count; i++) {
                        if(own.pets[i].status == SummonableStatus.Saved)
                            result += own.pets[i].m_atk / 2;
                    }
                }
                if(own.mounts.Count > 0) {
                    for(i = 0; i < own.mounts.Count; i++)
                        result += own.mounts[i].status == SummonableStatus.Saved ? own.mounts[i].m_atk / 2 : own.mounts[i].m_atk;
                }
                result += own.militaryRank > 0 && classType == DamageType.Magical ? ScriptableMilitaryRank.dict[own.militaryRank].atk : 0;
                if(classType == DamageType.Magical && own.titles.Count > 0) {
                    for(i = 0; i < own.titles.Count; i++)
                        if(ScriptableTitle.dict.TryGetValue(own.titles[i], out ScriptableTitle title))
                            result += own.titles[i] == activeTitle ? title.mAtk.active : title.mAtk.notActive;
                }
                result += InGuild() ? ScriptableGuildSkill.dict[2].Get(own.guildSkills[2]) * Storage.data.AP_Intelligence_ATK : 0;
                return base.m_atk + result;
            }
        }
        #endregion
        #region Defense
        public override int p_def {
            get {
                int i, result = own.endurance * Storage.data.AP_Endurance + own.strength * Storage.data.AP_Strength_DEF
                            + classInfo.pDef;
                for(i = 0; i < equipment.Count; i++) {
                    if(equipment[i].amount > 0 && equipment[i].item.data != null)
                        result += (int)equipment[i].item.GetSocketOfType(BonusType.pDef) + equipment[i].item.pDef;
                }
                for(i = 0; i < own.accessories.Count; i++) {
                    if(own.accessories[i].amount > 0 && own.accessories[i].item.data != null)
                        result += (int)own.accessories[i].item.GetSocketOfType(BonusType.pDef) + own.accessories[i].item.pDef;
                }
                result += activePet != null ? activePet.p_def : 0;
                if(own.pets.Count > 0) {
                    for(i = 0; i < own.pets.Count; i++) {
                        if(own.pets[i].status == SummonableStatus.Saved)
                            result += own.pets[i].p_def / 2;
                    }
                }
                if(own.mounts.Count > 0) {
                    for(i = 0; i < own.mounts.Count; i++)
                        result += own.mounts[i].status == SummonableStatus.Saved ? own.mounts[i].p_def / 2 : own.mounts[i].p_def;
                }
                result += own.militaryRank > 0 && classType == DamageType.Physical ? ScriptableMilitaryRank.dict[own.militaryRank].def : 0;
                if(classType == DamageType.Physical && own.titles.Count > 0) {
                    for(i = 0; i < own.titles.Count; i++)
                        if(ScriptableTitle.dict.TryGetValue(own.titles[i], out ScriptableTitle title))
                            result += own.titles[i] == activeTitle ? title.pDef.active : title.pDef.notActive;
                }
                result += InGuild() ? ScriptableGuildSkill.dict[1].Get(own.guildSkills[1]) * Storage.data.AP_Strength_DEF +
                            ScriptableGuildSkill.dict[3].Get(own.guildSkills[3]) * Storage.data.AP_Endurance : 0;
                return base.p_def + result;
            }
        }
        public override int m_def {
            get {
                int i, result = own.endurance * Storage.data.AP_Endurance + own.intelligence * Storage.data.AP_Intelligence_DEF
                            + classInfo.mDef;
                for(i = 0; i < equipment.Count; i++) {
                    if(equipment[i].amount > 0 && equipment[i].item.data != null)
                        result += (int)equipment[i].item.GetSocketOfType(BonusType.mDef) + equipment[i].item.mDef;
                }
                for(i = 0; i < own.accessories.Count; i++) {
                    if(own.accessories[i].amount > 0 && own.accessories[i].item.data != null)
                        result += (int)own.accessories[i].item.GetSocketOfType(BonusType.mDef) + own.accessories[i].item.mDef;
                }
                result += activePet != null ? activePet.m_def : 0;
                if(own.pets.Count > 0) {
                    for(i = 0; i < own.pets.Count; i++) {
                        if(own.pets[i].status == SummonableStatus.Saved)
                            result += own.pets[i].m_def / 2;
                    }
                }
                if(own.mounts.Count > 0) {
                    for(i = 0; i < own.mounts.Count; i++)
                        result += own.mounts[i].status == SummonableStatus.Saved ? own.mounts[i].m_def / 2 : own.mounts[i].m_def;
                }
                result += own.militaryRank > 0 && classType == DamageType.Magical ? ScriptableMilitaryRank.dict[own.militaryRank].def : 0;
                if(classType == DamageType.Magical && own.titles.Count > 0) {
                    for(i = 0; i < own.titles.Count; i++)
                        if(ScriptableTitle.dict.TryGetValue(own.titles[i], out ScriptableTitle title))
                            result += own.titles[i] == activeTitle ? title.mDef.active : title.mDef.notActive;
                }
                result += InGuild() ? ScriptableGuildSkill.dict[2].Get(own.guildSkills[2]) * Storage.data.AP_Intelligence_DEF +
                            ScriptableGuildSkill.dict[3].Get(own.guildSkills[3]) * Storage.data.AP_Endurance : 0;
                return base.m_def + result;
            }
        }
        #endregion
        #region Block
        public override float blockChance {
            get {
                int i;
                float result = classInfo.block;
                for(i = 0; i < equipment.Count; i++) {
                    if(equipment[i].amount > 0 && equipment[i].item.data != null)
                        result += equipment[i].item.GetSocketOfType(BonusType.block) + equipment[i].item.block;
                }
                for(i = 0; i < own.accessories.Count; i++) {
                    if(own.accessories[i].amount > 0 && own.accessories[i].item.data != null)
                        result += own.accessories[i].item.GetSocketOfType(BonusType.block) + own.accessories[i].item.block;
                }
                result += activePet != null ? activePet.blockChance : 0;
                if(own.pets.Count > 0) {
                    for(i = 0; i < own.pets.Count; i++) {
                        if(own.pets[i].status == SummonableStatus.Saved)
                            result += own.pets[i].block / 2;
                    }
                }
                if(own.mounts.Count > 0) {
                    for(i = 0; i < own.mounts.Count; i++)
                        result += own.mounts[i].status == SummonableStatus.Saved ? own.mounts[i].blockChance / 2 : own.mounts[i].blockChance;
                }
                result= own.militaryRank > 0 ? ScriptableMilitaryRank.dict[own.militaryRank].block : 0;
                if(own.titles.Count > 0) {
                    for(i = 0; i < own.titles.Count; i++)
                        if(ScriptableTitle.dict.TryGetValue(own.titles[i], out ScriptableTitle title))
                            result += own.titles[i] == activeTitle ? title.block.active : title.block.notActive;
                }
                result += InGuild() ? (float)ScriptableGuildSkill.dict[5].Get(own.guildSkills[5]) * .1f : 0;
                return base.blockChance + result;
            }
        }
        public override float untiBlockChance {
            get {
                int i;
                float result = classInfo.untiBlock;
                result += activePet != null ? activePet.untiBlockChance : 0;
                if(own.pets.Count > 0) {
                    for(i = 0; i < own.pets.Count; i++) {
                        if(own.pets[i].status == SummonableStatus.Saved)
                            result += own.pets[i].antiBlock / 2;
                    }
                }
                if(own.mounts.Count > 0) {
                    for(i = 0; i < own.mounts.Count; i++)
                        result += own.mounts[i].status == SummonableStatus.Saved ? own.mounts[i].untiBlockChance / 2 : own.mounts[i].untiBlockChance;
                }
                result += own.militaryRank > 0 ? ScriptableMilitaryRank.dict[own.militaryRank].untiBlock : 0;
                if(own.titles.Count > 0) {
                    for(i = 0; i < own.titles.Count; i++)
                        if(ScriptableTitle.dict.TryGetValue(own.titles[i], out ScriptableTitle title))
                            result += own.titles[i] == activeTitle ? title.antiBlock.active : title.antiBlock.notActive;
                }
                return base.untiBlockChance + result;
            }
        }
        #endregion
        #region Critical
        public override float critRate {
            get {
                int i;
                float result = classInfo.crit;
                for(i = 0; i < equipment.Count; i++) {
                    if(equipment[i].amount > 0 && equipment[i].item.data != null)
                        result += equipment[i].item.GetSocketOfType(BonusType.crit) + equipment[i].item.critRate;
                }
                for(i = 0; i < own.accessories.Count; i++) {
                    if(own.accessories[i].amount > 0 && own.accessories[i].item.data != null)
                        result += own.accessories[i].item.GetSocketOfType(BonusType.crit) + own.accessories[i].item.critRate;
                }
                result += activePet != null ? activePet.critRate : 0;
                if(own.pets.Count > 0) {
                    for(i = 0; i < own.pets.Count; i++) {
                        if(own.pets[i].status == SummonableStatus.Saved)
                            result += own.pets[i].critRate / 2;
                    }
                }
                if(own.mounts.Count > 0) {
                    for(i = 0; i < own.mounts.Count; i++)
                        result += own.mounts[i].status == SummonableStatus.Saved ? own.mounts[i].criticalChance / 2 : own.mounts[i].criticalChance;
                }
                result += own.militaryRank > 0 ? ScriptableMilitaryRank.dict[own.militaryRank].crit : 0;
                if(own.titles.Count > 0) {
                    for(i = 0; i < own.titles.Count; i++)
                        if(ScriptableTitle.dict.TryGetValue(own.titles[i], out ScriptableTitle title))
                            result += own.titles[i] == activeTitle ? title.critRate.active : title.critRate.notActive;
                }
                result += InGuild() ? (float)ScriptableGuildSkill.dict[4].Get(own.guildSkills[4]) * .1f : 0;
                return base.critRate + result;
            }
        }
        public override float critDmg {
            get {
                int i;
                float result = classInfo.critDmg;
                result += activePet != null ? activePet.critDmg : 0;
                if(own.pets.Count > 0) {
                    for(i = 0; i < own.pets.Count; i++) {
                        if(own.pets[i].status == SummonableStatus.Saved)
                            result += own.pets[i].critDmg / 2;
                    }
                }
                if(own.mounts.Count > 0) {
                    for(i = 0; i < own.mounts.Count; i++)
                        result += own.mounts[i].status == SummonableStatus.Saved ? own.mounts[i].criticalRate / 2 : own.mounts[i].criticalRate;
                }
                result += own.militaryRank > 0 ? ScriptableMilitaryRank.dict[own.militaryRank].critDmg : 0;
                if(own.titles.Count > 0) {
                    for(i = 0; i < own.titles.Count; i++)
                        if(ScriptableTitle.dict.TryGetValue(own.titles[i], out ScriptableTitle title))
                            result += own.titles[i] == activeTitle ? title.critDmg.active : title.critDmg.notActive;
                }
                return base.critDmg + result;
            }
        }
        public override float antiCrit {
            get {
                int i;
                float result = classInfo.untiCrit;
                result += activePet != null ? activePet.antiCrit : 0;
                if(own.pets.Count > 0) {
                    for(i = 0; i < own.pets.Count; i++) {
                        if(own.pets[i].status == SummonableStatus.Saved)
                            result += own.pets[i].antiCrit / 2;
                    }
                }
                if(own.mounts.Count > 0) {
                    for(i = 0; i < own.mounts.Count; i++)
                        result += own.mounts[i].status == SummonableStatus.Saved ? own.mounts[i].untiCriticalChance / 2 : own.mounts[i].untiCriticalChance;
                }
                result += own.militaryRank > 0 ? ScriptableMilitaryRank.dict[own.militaryRank].untiCrit : 0;
                if(own.titles.Count > 0) {
                    for(i = 0; i < own.titles.Count; i++)
                        if(ScriptableTitle.dict.TryGetValue(own.titles[i], out ScriptableTitle title))
                            result += own.titles[i] == activeTitle ? title.antiCrit.active : title.antiCrit.notActive;
                }
                return base.antiCrit + result;
            }
        }
        #endregion
        #region Helpers
        public uint battlepower => Convert.ToUInt32(healthMax + manaMax + m_atk + p_atk + m_def + p_def + 
        (blockChance + untiBlockChance + critRate + critDmg + antiCrit + untiStunChance) * 100);
        #endregion
    #endregion //Attributes
    #region Basic Functions and Helpers
        public override async void OnStartLocalPlayer() {
            localPlayer = this;// set singleton
            cam = Camera.main;// setup camera targets
            cam.GetComponent<CameraMMO>().target = transform;
            await LoadMap();
            //onPlayerInfoOverlay.enabled = true;
            // tribe flag
            tribeOverlay.sprite = ScriptableTribe.dict[tribeId].flag;
            CmdSetCurrentLanguage(UIManager.data.gameSettings.currentLang);
            UIManager.data.inScene.expBar.Refresh();
        }
        int NextSkill() {
            for (int i = 1; i < skills.Count; i++) {
                if(CastCheckSelf(skills[i])) return i;
            }
            return 0;
        }
        #endregion //Basic Functions
    #region Client
        protected override void Awake() {
            base.Awake();
            foreach (SkinnedMeshRenderer skin in GetComponentsInChildren<SkinnedMeshRenderer>())
                foreach (Transform bone in skin.bones)
                    skinBones[bone.name] = bone;
        }
        protected override void Start() {
            if (!isServer && !isClient)
                return; // do nothing if not spawned (=for character selection previews)
            base.Start();
            onlinePlayers[id] = this;
        }
        void OnDestroy() {
            if (onlinePlayers.TryGetValue(id, out Player entry) && entry == this)
                onlinePlayers.Remove(id);
                
            if (!isServer && !isClient)
                return; // do nothing if not spawned (=for character selection previews)

            if (isLocalPlayer) { // requires at least Unity 5.5.1 bugfix to work
                Destroy(indicator);
                localPlayer = null;
            }
        }
        void OnStateChanged(EntityState oldState, EntityState newState) {
            //UpdateAnimation();
        }
        private void LateUpdate() {
            UpdateAnimation();
        }
        void UpdateAnimation() {
            foreach (Animator anim in GetComponentsInChildren<Animator>()) {
                anim.SetBool("FIGHTING", IsFighting());
                anim.SetBool("MOVING", IsMoving() && state != EntityState.Casting && !IsMounted());
                anim.SetBool("CASTING", state == EntityState.Casting);
                anim.SetBool("STUNNED", state == EntityState.Stunned);
                anim.SetBool("MOUNTED", IsMounted()); // for seated animation
                anim.SetBool("DEAD", state == EntityState.Dead);
                foreach(Skill skill in skills) {
                    if(skill.level > 0 && !(skill.data is PassiveSkill))
                        anim.SetBool(skill.id.ToString(), skill.CastTimeRemaining() > 0);
                }
            }
            ApplyMountSeatOffset(); // org: LateUpdate
            if(IsMounted()) {
                mountObj.Moving(IsMoving() && state != EntityState.Casting);
            }
        }
        public override void ResetMovement() => agent.ResetMovement();
        [Client]
        async Task LoadMap()
        {
            if(Storage.data.currentLoadedMap != null)
            {
                Destroy(Storage.data.currentLoadedMap);
            }
            Storage.data.currentLoadedMap = Instantiate(city.prefab);
            Camera minimapCam = Storage.data.currentLoadedMap.GetComponentInChildren<Camera>();
            if(minimapCam != null)
            {
                minimapCam.GetComponent<CopyPosition>().target = transform;
                UIManager.data.inScene.miniMap.OnMapChanged(minimapCam);
            }
        }
        [Client] public void JoystickHandling(Vector2 newMovement) {
            float horizontal = newMovement.x;
            float vertical = newMovement.y;

            if (horizontal != 0 || vertical != 0) {
                // create input vector, normalize in case of diagonal movement
                Vector3 input = new Vector3(horizontal, 0, vertical);
                if (input.magnitude > 1) input = input.normalized;

                // get camera rotation without up/down angle, only left/right
                Vector3 angles = cam.transform.rotation.eulerAngles;
                angles.x = 0;
                Quaternion rotation = Quaternion.Euler(angles); // back to quaternion

                // calculate input direction relative to camera rotation
                Vector3 direction = rotation * input;

                // draw direction for debugging
                Debug.DrawLine(transform.position, transform.position + direction, Color.green, 0, false);

                // clear indicator if there is one, and if it's not on a target
                // (simply looks better)
                if (direction != Vector3.zero)
                    ClearIndicatorIfNoParent();

                // cancel path if we are already doing click movement, otherwise
                // we will slide
                agent.ResetMovement();

                // casting? then set pending velocity
                if (state == EntityState.Casting)
                {
                    pendingVelocity = direction * speed;
                    pendingVelocityValid = true;
                }
                else
                {
                    // set velocity
                    agent.velocity = direction * speed;

                    // moving with velocity doesn't look at the direction, do it manually
                    LookAtY(transform.position + direction);
                }

                // clear requested skill in any case because if we clicked
                // somewhere else then we don't care about it anymore
                useSkillWhenCloser = -1;
            }
        }
        [Client] void WASDHandling() {
            // don't move if currently typing in an input
            // we check this after checking h and v to save computations
            if (!UIUtils.AnyInputActive()) {
                // get horizontal and vertical input
                // note: no != 0 check because it's 0 when we stop moving rapidly
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                if (horizontal != 0 || vertical != 0) {
                    // create input vector, normalize in case of diagonal movement
                    Vector3 input = new Vector3(horizontal, 0, vertical);
                    if (input.magnitude > 1) input = input.normalized;

                    // get camera rotation without up/down angle, only left/right
                    Vector3 angles = cam.transform.rotation.eulerAngles;
                    angles.x = 0;
                    Quaternion rotation = Quaternion.Euler(angles); // back to quaternion

                    // calculate input direction relative to camera rotation
                    Vector3 direction = rotation * input;

                    // draw direction for debugging
                    Debug.DrawLine(transform.position, transform.position + direction, Color.green, 0, false);

                    // clear indicator if there is one, and if it's not on a target
                    // (simply looks better)
                    if (direction != Vector3.zero)
                        ClearIndicatorIfNoParent();

                    // cancel path if we are already doing click movement, otherwise
                    // we will slide
                    agent.ResetMovement();

                    // casting? then set pending velocity
                    if (state == EntityState.Casting) {
                        pendingVelocity = direction * speed;
                        pendingVelocityValid = true;
                    }
                    else {
                        // set velocity
                        agent.velocity = direction * speed;

                        // moving with velocity doesn't look at the direction, do it manually
                        LookAtY(transform.position + direction);
                    }

                    // clear requested skill in any case because if we clicked
                    // somewhere else then we don't care about it anymore
                    useSkillWhenCloser = -1;
                }
            }
        }
        [Client] public void UseSkill(int SkillIndex, bool reqTarget = true) {
            if(target == null) {
                FindNearestTarget();
            }
            if(!reqTarget || target != null) {
                TryUseSkill(SkillIndex);
            }
            else Notify.list.Add("This skill require a target", "هذه المهارة تتطلب هدف");
        }
        [Client] void SelectionHandling()
        {
            if (Input.GetMouseButtonDown(0) && !Utils.IsCursorOverUserInterface() && Input.touchCount <= 1)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool cast = localPlayerClickThrough ? Utils.RaycastWithout(ray, out hit, gameObject) : Physics.Raycast(ray, out hit);
                
                if(cast)
                {
                    useSkillWhenCloser = -1;
                    Entity entity = hit.transform.GetComponent<Entity>();
                    if(entity)
                    {
                        SetIndicatorViaParent(hit.transform);
                        CmdSetTarget(entity.netIdentity);

                        if (entity == target && entity != this && entity != activePet)
                        {
                            if (CanAttack(entity) && skills.Count > 0)
                            {
                                TryUseSkill(0);
                            }
                            else if (entity is Npc && entity.health > 0)
                            {
                                if(Utils.ClosestDistance(this, entity) <= interactionRange) 
                                {
                                    //UINpcDialogue.singleton.Show();
                                }
                                else
                                {
                                    Navigate(entity.collider.ClosestPoint(transform.position), interactionRange);
                                }
                            }
                            else
                            {
                                Navigate(entity.collider.ClosestPoint(transform.position), interactionRange);
                            }
                        }
                    }
                    else
                    { // otherwise it's a movement target
                        Loot loot = hit.transform.GetComponent<Loot>();
                        if(loot) {
                            UILoot.singleton.Show(loot);
                        }
                        Vector3 bestDestination = agent.NearestValidDestination(hit.point);
                        SetIndicatorViaPosition(bestDestination);
                        if(state == EntityState.Casting) {
                            pendingDestination = bestDestination;
                            pendingDestinationValid = true;
                        }
                        else Navigate(bestDestination, 0);
                        //target = null; // custom
                        if(own.auto.on) own.auto.on = false;
                    }
                }
            }
        }
        [Client] void TargetNearest()
        {
            if(Input.GetKeyDown(targetNearestKey))
            {
                FindNearestTarget();
            }
        }
        void FindNearestTarget()
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Monster");
            List<Monster> monsters = objects.Select(go => go.GetComponent<Monster>()).Where(m => m.health > 0).ToList();
            List<Monster> sorted = monsters.OrderBy(m => Vector3.Distance(transform.position, m.transform.position)).ToList();
            if (sorted.Count > 0)
            {
                SetIndicatorViaParent(sorted[0].transform);
                CmdSetTarget(sorted[0].netIdentity);
            }
        }
        [Client] public void onCastingStarted()
        {
            // hide wing
            if(wingsHolder != null)
            {
                wingsHolder.gameObject.SetActive(false);
            }
        }
        [Client] public void onCastingFinished()
        {
            if(animationEventEffects != null)
            {
                animationEventEffects.DestroySelf();
            }
            // show wing
            if(wingsHolder != null)
            {
                wingsHolder.gameObject.SetActive(true);
            }
        }
        [Client] void OnSkillCastFinished(Skill skill)
        {
            if(!isLocalPlayer)
                return;
            // tried to click move somewhere?
            if (pendingDestinationValid)
            {
                Navigate(pendingDestination, 0);
            }
            // tried to wasd move somewhere?
            else if (pendingVelocityValid)
            {
                agent.velocity = pendingVelocity;
            }
            // user pressed another skill button?
            else if (pendingSkill != -1)
            {
                TryUseSkill(pendingSkill, true);
            }
            // otherwise do follow up attack if no interruptions happened
            else if (skill.followupDefaultAttack)
            {
                TryUseSkill(0, true);
            }
            // clear pending actions in any case
            pendingSkill = -1;
            pendingDestinationValid = false;
            pendingVelocityValid = false;
        }
        protected override void UpdateClient()
        {
            if (state == EntityState.Idle || state == EntityState.Moving)
            {
                if (isLocalPlayer)
                {
                    // simply accept input
                    SelectionHandling();
                    WASDHandling();
                    TargetNearest();
                    // cancel action if escape key was pressed
                    if(Input.GetKeyDown(cancelActionKey))
                    {
                        agent.ResetPath(); // reset locally because we use rubberband movement
                        CmdCancelAction();
                    }
                    // trying to cast a skill on a monster that wasn't in range? then check if we walked into attack range by now
                    if (useSkillWhenCloser != -1)
                    {
                        // can we still attack the target? maybe it was switched.
                        if (CanAttack(target))
                        {
                            float range = skills[useSkillWhenCloser].castRange * Storage.data.attackToMoveRangeRatio;
                            if (Utils.ClosestDistance(this, target) <= range)
                            {
                                // then stop moving and start attacking
                                CmdUseSkill((sbyte)useSkillWhenCloser);
                                // reset
                                useSkillWhenCloser = -1;
                            }
                            else
                            {
                                //Debug.Log("walking closer to target...");
                                Navigate(Utils.ClosestPoint(target, transform.position), range);
                            }
                        }
                        // otherwise reset
                        else useSkillWhenCloser = -1;
                    }
                }
            }
            else if (state == EntityState.Casting)
            {
                // keep looking at the target for server & clients (only Y rotation)
                if (target)
                {
                    LookAtY(target.transform.position);
                }
                if (isLocalPlayer)
                {
                    // simply accept input and reset any client sided movement
                    SelectionHandling();
                    WASDHandling(); // still call this to set pendingVelocity for after cast
                    TargetNearest();
                    ResetMovement();

                    // cancel action if escape key was pressed
                    if (Input.GetKeyDown(cancelActionKey))
                    {
                        CmdCancelAction();
                    }
                }
            }
            else if (state == EntityState.Stunned)
            {
                if (isLocalPlayer)
                {
                    // simply accept input and reset any client sided movement
                    SelectionHandling();
                    TargetNearest();
                    ResetMovement();
                    // cancel action if escape key was pressed
                    if (Input.GetKeyDown(cancelActionKey))
                    {
                        CmdCancelAction();
                    }
                }
            }
            else if(state == EntityState.Dead)
            {
                ResetMovement();
            }
            else
            {
                Debug.LogError("invalid state:" + state);
            }
        }
        public override void OnStartClient() 
        {
            base.OnStartClient();
            // 3d model
            equipment.Callback += OnEquipmentChanged;
            wardrobe.Callback += OnWardrobeChanged;
            RefreshAllLocation();
            //overlays
            if(onPlayerInfoOverlay != null)
            {
                onPlayerInfoOverlay.enabled = true;
            }
            if(nameText != null)
            {
                nameText.text = name;
            }
            if(guildText != null && InGuild())
            {
                guildText.text = guild.name;
            }
            if(tribeOverlay != null)
            {
                tribeOverlay.sprite = ScriptableTribe.dict[tribeId].flag;
            }
            if(activeTitle > 0)
            {
                UpdateTitle();
            }
            if(!isLocalPlayer && localPlayer != null)
            {
                if(nameText != null)
                {
                    if(InTeam() && localPlayer.InTeam() && own.team.Contains(id))
                    {
                        nameText.color = Color.green;
                    }
                    else if(localPlayer.tribeId != tribeId)
                    {
                        nameText.color = Color.red;
                    }
                }
                if(guildText != null && InGuild() && localPlayer.InGuild() && localPlayer.guild.id == guild.id)
                {
                    guildText.color = Storage.data.guildMemberColor;
                }
                if(mount.canMount && mount.mounted)
                {
                    mountObj = Instantiate(mount.prefab).GetComponent<MountBody>();
                    mountObj.Set(transform, speed);
                }
            }
        }
        public void SetIndicatorViaParent(Transform parent)
        {
            if (!indicator)
            {
                indicator = Instantiate(Storage.data.player.indicator);
            }
            indicator.transform.SetParent(parent, true);
            indicator.transform.position = parent.position;
        }
        public void SetIndicatorViaPosition(Vector3 position)
        {
            if (!indicator)
            {
                indicator = Instantiate(Storage.data.player.indicator);
            }
            indicator.transform.parent = null;
            indicator.transform.position = position;
        }
        // clear indicator if there is one, and if it's not on a target
        public void ClearIndicatorIfNoParent()
        {
            if (indicator != null && indicator.transform.parent == null)
            {
                Destroy(indicator);
            }
        }
    #endregion //Client Commands
    #region BodyModels
        void RefreshBodyLocation() {
            if(bodyHolder == null)
                return;
            int index = (int)ClothingCategory.Body;
            if(wardrobe[index].isUsed && showWardrop) {
                if(bodyHolder.childCount > 0) Destroy(bodyHolder.GetChild(0).gameObject);
                GameObject go = Instantiate(ScriptableWardrobe.dict[wardrobe[index].id].modelPrefab[(int)gender], bodyHolder, false);
                OnRefreshBodyLocation(go);
            }
            else {
                index = (int)EquipmentsCategory.Armor;
                if(bodyHolder.childCount > 0)
                    Destroy(bodyHolder.GetChild(0).gameObject);
                if(equipment.Count >= index && equipment[index].item.id > 0 && equipment[index].amount == 1) {
                    EquipmentItem itemData = (EquipmentItem)equipment[index].item.data;
                    if(itemData != null && itemData.modelPrefab[(int)gender] != null) {
                        GameObject go = Instantiate(itemData.modelPrefab[(int)gender], bodyHolder, false);
                        OnRefreshBodyLocation(go);
                    }
                } else {
                    GameObject go = Instantiate(Storage.data.basicBody[(int)gender], bodyHolder, false);
                    OnRefreshBodyLocation(go);
                }
            }
        }
        void OnRefreshBodyLocation(GameObject go) {
            ResetMeshAndAnimation(go);
            BodyPlaceholders bp = go.GetComponentInChildren<BodyPlaceholders>();
            if(bp != null) {
                effectMount = bp.rightHand;
                rWeaponHolder = bp.rightWeapon;
                lWeaponHolder = bp.leftWeapon;
                wingsHolder = bp.wing;
                animator.avatar = bp.avatar;
                if(classInfo.type == PlayerClass.Archer) {
                    Instantiate(Storage.data.player.arrowPrefab, rWeaponHolder, false);
                }
            }
            if(wardrobe[(int)ClothingCategory.Weapon].isUsed)
                RefreshMainWeaponLocation();// RefreshWeaponsLocation(); // after adding artifacts/second weapon
            if(wardrobe[(int)ClothingCategory.Wings].isUsed)
                RefreshWingsLocation();
        }
        void RefreshMainWeaponLocation()
        {
            Transform mainHand = mainWeaponHolder;
            if(mainHand == null)
                return;
            int index = (int)ClothingCategory.Weapon;
            if(wardrobe[index].isUsed && showWardrop &&
                ScriptableWardrobe.dict.TryGetValue(wardrobe[index].id, out ScriptableWardrobe weapon))
            {
                if(weapon.category != ClothingCategory.Weapon)
                    return;
                if(mainHand.childCount > 0)
                {
                    Destroy(mainHand.GetChild(0).gameObject);
                }
                Instantiate(weapon.modelPrefab[(int)classInfo.type - 1], mainHand, false);
            }
            else
            {// armor index => 7
                index = (int)EquipmentsCategory.Weapon;
                if(equipment.Count < index || equipment[index].item.id == 0 || equipment[index].amount != 1) 
                    return;
                EquipmentItem itemData = (EquipmentItem)equipment[index].item.data;
                if(itemData != null && itemData.category == EquipmentsCategory.Weapon && itemData.modelPrefab[0] != null)
                {
                    if(mainHand.childCount > 0)
                    {
                        Destroy(mainHand.GetChild(0).gameObject);
                    }
                    Instantiate(itemData.modelPrefab[0], mainHand, false);
                }
            }
            if(classInfo.type == PlayerClass.Archer && mainHand.childCount > 0) {
                BowInfo bowInfo = mainHand.GetChild(0).GetComponent<BowInfo>();
                if(bowInfo != null) {

                }
            }
        }
        void RefreshWingsLocation() {
            if(wingsHolder == null)
                return;
            if(wingsHolder.childCount > 0)
                Destroy(wingsHolder.GetChild(0).gameObject);
            int index = (int)ClothingCategory.Wings;
            if(wardrobe[index].isUsed && showWardrop)
                Instantiate(ScriptableWardrobe.dict[wardrobe[index].id].modelPrefab[0], wingsHolder, false);
            UIManager.data.controllers.UpdateFlyButton();
        }
        void RefreshSoulLocation() {
            if(spiritHolder == null)
                return;
            if(spiritHolder.childCount > 0)
                Destroy(spiritHolder.GetChild(0).gameObject);
            int index = (int)ClothingCategory.Spirit;
            if(wardrobe[index].isUsed && showWardrop)
                Instantiate(ScriptableWardrobe.dict[wardrobe[index].id].modelPrefab[0], spiritHolder, false);
        }
        public void RefreshAllLocation() {
            RefreshBodyLocation();
            RefreshMainWeaponLocation();// RefreshWeaponsLocation(); // after adding artifacts/second weapon
            RefreshWingsLocation();
            RefreshSoulLocation();
        }
        // callbacks
        void OnWardrobeVisibilityChanged(bool oldValue = false, bool newValue = false) { 
            RefreshAllLocation();
            if(UIManager.data.wardrobe.IsVisible())
                UIManager.data.wardrobe.UpdateVisibility();
        }
        void OnWardrobeChanged(SyncListWardrop.Operation op, int index, WardrobeItem oldcloth, WardrobeItem newCloth) {
            RefreshAllLocation();
            //UIManager.data.wardrob.UpdatePreview();
        }
        void OnEquipmentChanged(SyncListItemSlot.Operation op, int index, ItemSlot oldcloth, ItemSlot newCloth) {
            RefreshAllLocation();
            //UIManager.data.wardrob.UpdatePreview();
            if(UIManager.data.currenOpenWindow != null) {
                if(UIManager.data.currenOpenWindow is UIInventory invWin) {
                    invWin.RefreshEquipments();
                }
            }
        }
        void OnGenderChanged(Gender oldValue, Gender newValue) {
            RefreshAllLocation();
        }
        // helpers
        void ResetMeshAndAnimation(GameObject go) {
            SkinnedMeshRenderer equipmentSkin = go.GetComponentInChildren<SkinnedMeshRenderer>();
            if (equipmentSkin != null && CanReplaceAllBones(equipmentSkin))
                ReplaceAllBones(equipmentSkin);
            //Animator anims = go.GetComponentInChildren<Animator>();
            //anims.runtimeAnimatorController = animator.runtimeAnimatorController;// assign main animation controller to it
            RebindAnimators();// restart all animators, so that skinned mesh equipment will be in sync with the main animation
        }
        bool CanReplaceAllBones(SkinnedMeshRenderer equipmentSkin) {
            // are all equipment SkinnedMeshRenderer bones in the player bones?
            foreach(Transform bone in equipmentSkin.bones)
                if(!skinBones.ContainsKey(bone.name))
                    return false;
            return true;
        }
        void ReplaceAllBones(SkinnedMeshRenderer equipmentSkin) {
            // get equipment bones
            Transform[] bones = equipmentSkin.bones;
            // replace each one
            for (int i = 0; i < bones.Length; ++i) {
                string boneName = bones[i].name;
                if(!skinBones.TryGetValue(boneName, out bones[i]))
                    Debug.LogWarning(equipmentSkin.name + " bone " + boneName + " not found in original player bones. Make sure to check CanReplaceAllBones before.");
            }
            // reassign bones
            equipmentSkin.bones = bones;
        }
        void RebindAnimators() {
            foreach(Animator anim in GetComponentsInChildren<Animator>())
                anim.Rebind();
        }
    #endregion
    #region Client Callbacks
        protected override void OnTargetChanged(GameObject oldTarget, GameObject newTarget)
        {
            if(newTarget != null)
            {
                UIManager.data.inScene.selectedTargetInfo.Show();
            }
            else
            {
                UIManager.data.inScene.selectedTargetInfo.Hide();
            }
        }
        protected override void OnHealthChanged(int oldHp, int newHp)
        {
            UILocalPlayerInfo.singleton?.UpdateHealth();
        }
        protected override void OnManaChanged(int oldMp, int newMp)
        {
            UILocalPlayerInfo.singleton?.UpdateMana();
        }
        protected override void OnLevelChanged(byte oldLevel, byte newLevel)
        {
            UILocalPlayerInfo.singleton?.UpdateLevel();
        }
        void OnGuildInfoChanged(GuildPublicInfo oldInfo, GuildPublicInfo newInfo)
        {
            if(guildText != null)
            {
                guildText.gameObject.SetActive(InGuild());
                guildText.text = InGuild() ? newInfo.name : "";
            }
        }
        void OnTitleChanged(ushort oldTitle, ushort newTitle)
        {
            UpdateTitle();
        }
        void OnFrameChanged(byte oldFrame, byte newFrame)
        {
            UILocalPlayerInfo.singleton?.UpdateFrame();
        }
        void OnAvatarChanged(byte oldValue, byte newValue)
        {
            UILocalPlayerInfo.singleton?.UpdateAvatar();
        }
    #endregion
    // features' commands
    #region Skills
        public bool IsFighting() => NetworkTime.time - lastCombatTime < 20d;
        void InstantiateEffect(int EffectNumber) {
            if(animationEventEffects != null)
                animationEventEffects.InstantiateEffect(EffectNumber);
            else Debug.Log("animationEventEffects not found");
        }
        [Command] public void CmdSetTarget(NetworkIdentity ni) {}
        [Command] public void CmdCancelAction() {}
        public override bool CanAttack(Entity entity) => base.CanAttack(entity) && 
                                    (entity is Monster || entity is Player || (entity is Pet && entity != activePet));
        [Command] public void CmdUseSkill(sbyte skillIndex) {}
        [Client] public void TryUseSkill(int skillIndex, bool ignoreState = false) {
            // only if not casting already (might need to ignore that when coming from pending skill where CASTING is still true)
            if(state != EntityState.Casting || ignoreState) {
                Skill skill = skills[skillIndex];
                if (CastCheckSelf(skill) && CastCheckTarget(skill)) {
                    // check distance between self and target
                    Vector3 destination;
                    if (CastCheckDistance(skill, out destination)) {
                        // cast
                        CmdUseSkill((sbyte)skillIndex);
                        Debug.Log("Used " + skillIndex);
                    }
                    else {
                        // move to the target first (use collider point(s) to also work with big entities)
                        Navigate(destination, skill.castRange * Storage.data.attackToMoveRangeRatio);
                        // use skill when there
                        useSkillWhenCloser = skillIndex;
                    }
                }
            }
            else {
                pendingSkill = skillIndex;
            }
        }
    #endregion
    #region Class Promotion
        [Command] public void CmdStartPromotionQuest() {}
        [Command] public void CmdPromoteClass() {}
    #endregion
    #region Daily Sign & 7days
        //signIn
        [Command] public void CmdSignInToday() {}
        [Command] public void CmdCollectDailySignReward(int index) {}
        //7days => signup
        [Command] public void CmdGet7DaysSignedDays() {}
        [TargetRpc] public void TargetSet7DaysSignUp(int[] signedList) {
            UIManager.data.signUp7days.Set(signedList);
        }
        [Command] public void CmdCollect7DaysSignUpReward(int day) {}
        //7days => recharge
        [Command] public void CmdCollect7DaysRechargeReward(int day) {}
    #endregion
    #region Hot Events
        [Command] public void CmdClaimHotEventReward(int eventIndex, int objectiveIndex) {}
    #endregion
    #region Tribe
        [Command] public void CmdDonateToTribe(long goldAmount, int diamondsAmount) { }
    #endregion
    #region Guild
        public bool InGuild() => guild.id > 0;
        [Command] public void CmdGetAvailableGuildsToJoin() {}
        [Command] public void CmdSendJoinRequestToGuild(uint guildId) {}
        [Command] public void CmdCreateGuild(string guildName) {}
        [Command] public void CmdSendGuildInvitationToTarget() {}
        [Command] public void CmdSendGuildInvitation(uint playerId) {}
        [Command] public void CmdTerminateGuild() {}
        [Command] public void CmdLeaveGuild() {}
        [Command] public void CmdGetGuildMembersList() {}
        [Command] public void CmdGuildInviteAccept(int index) {}
        [Command] public void CmdGuildInviteDecline(int index) {}
        [Command] public void CmdGuildDonateGold(uint amount) {}
        [Command] public void CmdGuildDonateDiamonds(uint amount) {}
        [Command] public void CmdBuyItemsFromGuildShop(int index, int count) {}
        [Command] public void CmdGuildKick(uint member) {}
        [Command] public void CmdGuildPromote(uint member) {}
        [Command] public void CmdGuildDemote(uint member) {}
        [Command] public void CmdLearnGuildSkill(int index) {}
        [Command] public void CmdTransfarMasterRank(uint newMaster) {}
        [Command] public void CmdAnswerGuildRecall(bool answer) {}
        [Command] public void CmdGuildJoinRequests() {}
        [Command] public void CmdAcceptGuildJoinRequest(uint requesterId, bool answer) {}
        [Command] public void CmdGuildUpgradeHall() {}
        [Command] public void CmdSetGuildNotice(string notice) {}
        [TargetRpc] public void TargetSetAvailableGuildsToJoin(GuildJoinInfo[] data) {
            UIManager.data.guildWindow.SetAvailableGuildsToJoin(data);
        }
        [TargetRpc] public void TargetJoinedGuild() {
            UIManager.data.guildWindow.OnJoinedGuild();
        }
        [TargetRpc] public void TargetShowGuildInvitationNotification() {
            UIManager.data.notifiyIconsList.ShowGuildInvitation();
        }
        [TargetRpc] public void TargetSetGuildMembersListIntoWindow(GuildMember[] membersList) {
            UIManager.data.guildWindow.SetMembersList(membersList);
        }
        [TargetRpc] public void TargetShowGuildRecall(GuildRecallRequest request) {
            UIManager.data.guildRecallMsg.Show(request);
        }
        [TargetRpc] public void TargetSetGuildJoinRequests(GuildJoinRequest[] data) {
            UIManager.data.guildWindow.SetJoinRequestsList(data);
        }
    #endregion
    #region Team
        public bool InTeam() => teamId > 0;
        public List<Player> GetTeamMembersInProximity() {
            List<Player> players = new List<Player>();
            foreach(NetworkConnection conn in netIdentity.observers.Values) {
                Player player = conn.identity.GetComponent<Player>();
                if(player != null && player.teamId == teamId)
                    players.Add(player);
            }
            return players;
        }
        void OnTeamChanged(uint oldTeam, uint newTeam) {
            // change ui
        }
        [Command] public void CmdFormTeam() {}
        [Command] public void CmdSendTeamInvitation(uint otherId) {}
        [Command] public void CmdSendTeamInvitationToTarget() {}
        [Command] public void CmdTeamInvitationAccept(int index) {}
        [Command] public void CmdTeamInvitationRefuse(int index) {}
        [Command] public void CmdTeamKickMember(uint memberId) {}
        [Command] public void CmdLeaveTeam() {}
        [Command] public void CmdDisbandTeam() {}
    #endregion
    #region Warkshop
    //plus
        [Command] public void CmdWorkshopPlus(int index, WorkshopOperationFrom from, int luckItem) {}
        //socket
        [Command] public void CmdWorkshopUnlockSocket(int index, WorkshopOperationFrom from, int socketIndex) {}
        [Command] public void CmdWorkshopRemoveGemFromSocket(int index, WorkshopOperationFrom from, int socketIndex) {}
        [Command] public void CmdWorkshopAddGemInSocket(int index, WorkshopOperationFrom from, int socketIndex, int gemIndex) {}
        // quality
        [Command] public void CmdWorkshopUpgradeItemQuality(int index, WorkshopOperationFrom from, int feedItem) {}
        // craft
        [Command] public void CmdCraft(int recipeId, uint amount) {}
        [TargetRpc] public void TargetItemEnhanceSuccess() {
            UIManager.data.success.SetActive(true);
            //UIManager.data.enhancment.OnEnhanceUpdate();
        }
        [TargetRpc] public void TargetItemEnhanceFailure() {
            UIManager.data.failure.SetActive(true);
            //UIManager.data.enhancment.OnEnhanceUpdate();
        }
    #endregion
    #region Inventory
        public int InventorySlotsFree() {
            int free = 0;
            foreach (ItemSlot slot in own.inventory)
                if (slot.amount == 0)
                    ++free;
            return free;
        }
        public int InventorySlotsOccupied() {
            int occupied = 0;
            for(int i = 0; i < own.inventorySize; i++)
                if(own.inventory[i].amount > 0)
                    occupied++;
            return occupied;
        }
        public uint InventoryCount(Item item) {
            uint amount = 0;
            foreach (ItemSlot slot in own.inventory)
                if (slot.amount > 0 && slot.item.Equals(item))
                    amount += slot.amount;
            return amount;
        }
        public uint InventoryCountById(int itemId) {
            uint amount = 0;
            for(int i = 0; i < own.inventory.Count; i++) {
                if (own.inventory[i].amount > 0 && own.inventory[i].item.id == itemId)
                    amount += own.inventory[i].amount;
            }
            return amount;
        }
        public float GetItemCooldown(string cooldownCategory) {
            // get stable hash to reduce bandwidth
            int hash = cooldownCategory.GetStableHashCode();
            // find cooldown for that category
            if (own.itemCooldowns.TryGetValue(hash, out double cooldownEnd)) {
                return NetworkTime.time >= cooldownEnd ? 0 : (float)(cooldownEnd - NetworkTime.time);
            }
            // none found
            return 0;
        }
        [Command] public void CmdUseInventoryItem(int index) {}
        [Command] public void CmdSortInventory() {}
        [Command] public void CmdOpenInventorySlots(int slotsCount) {}
        [Command] public void CmdSellInventoryItemsForGold(int[] items) {}
        [Command] public void CmdBuyItemsFromInventoryShop(int index, int count) {}
        [ClientRpc] public void RpcUsedItem(Item item) {
            // validate
            if(item.data is UsableItem usable) {
                usable.OnUsed();
            }
        }
    #endregion
    #region Equipments

        /*void OnEquipmentChanged(SyncListItemSlot.Operation op, int index, ItemSlot oldSlot, ItemSlot newSlot) {
            if(index == 1) RefreshBodyLocation();
            else if(index == 7) RefreshMainWeaponLocation();
        }*/
        public bool IsEquipedWithWeapon(WeaponType type)
        {
            if( equipment.Count < (int)EquipmentsCategory.Weapon ||
                !equipment[(int)EquipmentsCategory.Weapon].isEquipment ||
                ((WeaponItem)equipment[(int)EquipmentsCategory.Weapon].item.data).weaponType != type)
            {
                return false;
            }
            return true;
        }
        public bool HasWeapon()
        {
            return  (rWeaponHolder != null && rWeaponHolder.childCount > 0) || 
                    (lWeaponHolder != null && lWeaponHolder.childCount > 0);
        }
    #endregion
    #region Wardrobe
        public bool IsWinged()
        {
            return wardrobe[(int)ClothingCategory.Wings].isUsed;
        }
        [Command] public void CmdWardrobeEquip(ushort wardropId) {}
        [Command] public void CmdWardrobeUnEquip(int index) {}
        [Command] public void CmdWardrobeSwitchVisibility() {}
        [Command] public void CmdWardrobeSynthesize(int mainIndex, bool isEquiped, int otherIndex, int blessIndex) {}
    #endregion
    #region Mail
        [Command] public void CmdMarkAsSeen(int index) {}
        [Command] public void CmdCollectMail(int index) {}
        [Command] public void CmdCollectAllMails() {}
        [Command] public void CmdDeleteMail(int index) {}
        [Command] public void CmdDeleteAllMails() {}
        [TargetRpc] public void TargetUpdateMailItems(int index, bool[] recievedItems) {
            for(int i = 0; i < own.mailBox[index].items.Length; i++)
                own.mailBox[index].items[i].recieved = recievedItems[i];
        }
        
    #endregion
    #region VIP
        [Command] public void CmdGetFirstReward(int level) {}
    #endregion
    #region Market
        [Command] public void CmdGetMarketItems() {}
        [Command] public void CmdBuyMarketItem(int id) {}
        [Command] public void CmdAddMarketItem(int slot, int amount, int price, int hoursIndex) {}
    #endregion
    #region Teleportation
        public bool CanTeleportTo(int mapIndex) => mapIndex > -1 && mapIndex < Storage.data.cities.Count;
        public async void OnCityChanged(byte oldCity, byte newCity)
        {
            await Task.Run(() => LoadMap());
            CmdConfirmTeleport();
        }
        [Command] public void CmdTeleportTo(byte targetCity, Vector3 targetLocation) {}
        [Command] public void CmdConfirmTeleport() {}
        [Command] public void CmdNpcTeleport(int index) {}
        [Command] public void CmdWorldBossTeleport() {}
        [Command] public void CmdTeleportToQuestLocation(Vector3 questLocation) {}
        [TargetRpc] public void TargetLoadEventMap(EventMaps eventType, Vector3 mapPos)
        {
            if(Storage.data.currentLoadedMap != null)
            {
                Destroy(Storage.data.currentLoadedMap);
            }
            Storage.data.currentLoadedMap = Instantiate(Storage.data.eventMaps[(int)eventType], mapPos, Quaternion.identity);
            Camera minimapCam = Storage.data.currentLoadedMap.GetComponentInChildren<Camera>();
            if(minimapCam != null)
            {
                minimapCam.GetComponent<CopyPosition>().target = transform;
                UIManager.data.inScene.miniMap.OnMapChanged(minimapCam);
            }
            CmdConfirmTeleport();
        }
    #endregion
    #region Quests
        public int GetQuestIndexByName(int questName) {
            for(int i = 0; i < own.quests.Count; ++i)
                if (own.quests[i].id == questName)
                    return i;
            return -1;
        }
        public bool HasCompletedQuest(int questName) {
            foreach (Quest quest in own.quests)
                if (quest.id == questName && quest.completed)
                    return true;
            return false;
        }
        public int CountIncompleteQuests() {
            int count = 0;
            foreach (Quest quest in own.quests)
                if (!quest.completed)
                    ++count;
            return count;
        }
        public bool HasActiveQuest(int questName) {
            foreach (Quest quest in own.quests)
                if (quest.id == questName && !quest.completed)
                    return true;
            return false;
        }
        public bool CanAcceptQuest(ScriptableQuest quest) {
            // not too many quests yet?
            // has required level?
            // not accepted yet?
            // has finished predecessor quest (if any)?
            return level >= quest.requiredLevel &&          // has required level?
                GetQuestIndexByName(quest.name) == -1 && // not accepted yet?
                (quest.predecessor == null || HasCompletedQuest(quest.predecessor.name));
        }
        public bool CanCompleteQuest(int questName) {
            // has the quest and not completed yet?
            int index = GetQuestIndexByName(questName);
            if(index != -1 && !own.quests[index].completed) {
                return own.quests[index].IsFulfilled();
            }
            return false;
        }
        [Command] public void CmdAcceptQuest(int npcQuestIndex) {}
        [Command] public void CmdCompleteQuest(int name) {}
    #endregion
    #region Mall
        [Command] public void CmdBuyItemsFromMall(int categoryIndex, int itemIndex, int amount, bool usingBound) {}
    #endregion
    #region Pet
        public bool CanUnsummonPet() {
            return activePet != null && (state == EntityState.Idle || state == EntityState.Moving) &&
                (activePet.state == EntityState.Idle || activePet.state == EntityState.Moving);
        }
        [Command] public void CmdPetSummon(ushort petId) {}
        [Command] public void CmdPetUnsummon() {}
        [Command] public void CmdPetFeedx1(ushort petId, ushort selectedFeed) {}
        [Command] public void CmdPetFeedx10(ushort petId, ushort selectedFeed) {}
        [Command] public void CmdPetActivate(int itemName) {}
        [Command] public void CmdPetUpgrade(ushort petId) {}
        [Command] public void CmdPetStarUp(ushort petId) {}
        [Command] public void CmdPetTrain(ushort petId) {}
        [Command] public void CmdPetChangeExpShare() {}
    #endregion
    #region Mount
        public bool IsMounted() => mount.canMount && mount.mounted;
        void OnActiveMountChanged(ActiveMount oldInfo, ActiveMount newInfo) {
            if(mountObj != null && (newInfo.mounted == false || oldInfo.id != newInfo.id))
                Destroy(mountObj.gameObject);

            if(IsMounted()) {
                mountObj = Instantiate(newInfo.prefab).GetComponent<MountBody>();
                mountObj.dataIndex = own.mounts.FindIndex(m => m.id == newInfo.id);
                mountObj.Set(transform, speed);
            }
            
            if(oldInfo.id != newInfo.id || oldInfo.mounted != newInfo.mounted) {
                UIManager.data.controllers.UpdateMountButton();
            }
            UpdateAnimation();
        }
        [Command] public void CmdMountActivate(int itemName) {}
        [Command] public void CmdMountDeploy(ushort mountId) {}
        [Command] public void CmdMountRecall() {}
        [Command] public void CmdMountFeedx1(ushort mountId, ushort selectedFeed) {}
        [Command] public void CmdMountFeedx10(ushort mountId, ushort selectedFeed) {}
        [Command] public void CmdMountUpgrade(ushort mountId) {}
        [Command] public void CmdMountStarUp(ushort mountId) {}
        [Command] public void CmdMountSummon() {}
        [Command] public void CmdMountUnsummon() {}

        void ApplyMountSeatOffset() {
            if (meshToOffsetWhenMounted != null) {// apply seat offset if on mount (not a dead one), reset otherwise
                if(IsMounted() && mountObj != null) {
                    meshToOffsetWhenMounted.transform.position = mountObj.seat.position + Vector3.up * seatOffsetY;
                    Debug.Log(mountObj.seat.position + Vector3.up * seatOffsetY);
                }
                else meshToOffsetWhenMounted.transform.localPosition = Vector3.zero;
            }
        }
    #endregion
    #region Auto Fight

    #endregion
    #region Titles
    [Command] public void CmdActivateTitle(int itemIndex) {}
    [Command] public void CmdSetActiveTitle(int titleIndex) {}
    [Client] void UpdateTitle() {
        if(titleOverlay != null) {
            if(activeTitle == 0)
                titleOverlay.sprite = null;
            else if(ScriptableTitle.dict.TryGetValue(activeTitle, out ScriptableTitle titleData)) {
                titleOverlay.sprite = titleData.image[(int)LanguageManger.current];
            }
        }
    }
    #endregion
    #region Preview
    [Command] public void CmdPreviewPlayerInfo(uint playerId) {}
    [Command] public void CmdPreviewTargetPlayerInfo() {}
    [TargetRpc] public void TargetShowPlayerPreview(PreviewPlayerData info) {
        UIManager.data.playerPreviewWindow.Show(info);
    }
    #endregion
    #region Ranking
    [Command] public void CmdGetRankingData(RankingCategory category) {}
    [TargetRpc] public void TargetSetBasicRankingValue(RankingBasicData[] data) {
        UIManager.data.RankingWindow.ShowBasicList(data);
        Debug.Log(data);
    }
    [TargetRpc] public void TargetSetSummonableRankingValue(SummonableRankingData[] data) {
        UIManager.data.RankingWindow.ShowSummonableList(data);
    }
    #endregion
    #region Friends
        [Command] public void CmdRefreshOnlineFriends() {}
        [Command] public void CmdSendFriendRequest(uint fId) {}
        [Command] public void CmdAcceptFriendRequest(int index) {}
        [Command] public void CmdRefuseFriendRequest(int index) {}
        [Command] public void CmdRemoveFriend(uint fId) {}
    #endregion
    #region Military Rank
        [Command] public void CmdPromoteMilitaryRank() {}
    #endregion
    #region Achievements
        [Command] public void CmdRecieveAchievementReward(ushort achId) {}
    #endregion
    #region Marriage
        public bool IsMarried() => own.marriage.spouse > 0;
        [Command] public void CmdSendMarriageProposal(int sId, MarriageType type) {}
        [Command] public void CmdRefuseMarriageProposal(int index) {}
        [Command] public void CmdAcceptMarriageProposal(int index) {}
        [TargetRpc] public void TargetAnnounceMarriage(string husband, string wife) {
            
        }
    #endregion
    #region Purchase
    [Command] public void CmdValidatePurchasePackage(string receipt) {}
    [TargetRpc] public void TargetConfirmPurchase() {

    }
    #endregion
    #region Arena
        [Command] public void CmdRegisterInArena1v1() {}
        [Command] public void CmdUnRegisterInArena1v1() {}
        [Command] public void CmdAcceptChallengeArena1v1() {}
        [Command] public void CmdRefuseChallengeArena1v1() {}
        [Command] public void CmdLeaveArena1v1() {}
        [TargetRpc] public void TargetNotifiyArenaMatch1v1() {
            UIManager.data.ClearWindows();
            UIManager.data.inScene.arenaNotify.Show1v1();
        }
        [TargetRpc] public void TargetHideNotifiyArenaMatch1v1() {
            UIManager.data.inScene.arenaNotify.Hide();
        }
        [TargetRpc] public void TargetRefusedArenaMatch1v1() {
            UIManager.data.inScene.arenaNotify.Hide();
        }
        [TargetRpc] public void TargetCanceledArenaMatch1v1() {
            UIManager.data.inScene.arenaNotify.Hide();
        }
        [TargetRpc] public void TargetShowResultArena1v1(bool win, int dmg, int opponentDmg) {
            UIManager.data.inScene.arenaMatchResult.Show1v1(win, dmg, opponentDmg);
        }
    #endregion
    #region Trade
        [Command] public void CmdTradeInvite(uint playerId) {}
        [Command] public void CmdTradeAcceptInvitation(int index) {}
        [Command] public void CmdTradeRefuseInvitation(int index) {}
        [Command] public void CmdTradeConfirmOffer(TradeOfferContent offerContent) {}
        [Command] public void CmdTradeAcceptOffer() {}
        [TargetRpc] public void TargetInitiateTrade() {
            UIManager.data.pages.trade.Show();
        } 
        [TargetRpc] public void TargetShowConfirmedTradeOffer(ItemSlot[] offeredItems, uint offeredGold, uint offeredDiamonds) {
            if(UIManager.data.currenOpenWindow != null && UIManager.data.currenOpenWindow is Trade trade)
            {
                trade.UpdateConfirmedOffer(offeredItems, offeredGold, offeredDiamonds);
            }
            else Notify.list.Add("You're not trading");
        }
        [TargetRpc] public void TargetAcceptTradeOffer() {
            if(UIManager.data.currenOpenWindow != null && UIManager.data.currenOpenWindow is Trade trade)
            {
                trade.OtherAccepted();
            }
            else Notify.list.Add("You're not trading");
        }
        [TargetRpc] public void TargetAcceptedMyTradeOffer() {
            if(UIManager.data.currenOpenWindow != null && UIManager.data.currenOpenWindow is Trade trade)
            {
                trade.MineAccepted();
            }
            else Notify.list.Add("You're not trading");
        }
        [TargetRpc] public void TargetCloseTrade() {
            if(UIManager.data.currenOpenWindow != null && UIManager.data.currenOpenWindow is Trade trade)
            {
                trade.Close();
            }
            else Notify.list.Add("You're not trading");
        }
    #endregion
    #region Shops
        [Command] public void CmdCheckShopDiscount(int shopId) {}
    #endregion
    #region General
        [TargetRpc] public void TargetNotify(string msg) => Notify.list.Add(msg);
        [TargetRpc] public void TargetNotifySuccess(NotifySuccessType type) {
            if(type == NotifySuccessType.Wardrobe)
                UIManager.data.wardrobe.OnSynthesizeDone(true);
            // default
            UIManager.data.success.SetActive(true);
        }
        [TargetRpc] public void TargetNotifyFailure(NotifySuccessType type) {
            Debug.Log(type);
            if(type == NotifySuccessType.Wardrobe)
                UIManager.data.wardrobe.OnSynthesizeDone();
            // default
            UIManager.data.failure.SetActive(true);
        }
        [TargetRpc] public void TargetOnLevelUp() {
            UILocalPlayerInfo.singleton.UpdateLevel();
            UIManager.data.inScene.levelUpNotice.Show();
        }
        [TargetRpc] public void TargetShowRespawn(byte freeRespawn) => UIManager.data.inScene.respawn.Show(freeRespawn);
        [TargetRpc] public void TargetHideRespawn() => UIManager.data.inScene.respawn.Hide();
        [Command] public void CmdRespawn(int choice) {}
        [Command] public void CmdSetCurrentLanguage(Languages language) {}
        [Command] public void CmdNotifyIfPlayerOffline(uint pId) {}
        [Command] public void CmdIncreaseStrength() {}
        [Command] public void CmdIncreaseIntelligence() {}
        [Command] public void CmdIncreaseVitality() {}
        [Command] public void CmdIncreaseEndurance() {}
    #endregion
    #region Trigger
        protected override void OnTriggerEnter(Collider col) {
            // call base function too
            base.OnTriggerEnter(col);
            if(col.CompareTag("Loot")) {
                // show in ui
            }
        }
        // validation //////////////////////////////////////////////////////////////
        void OnValidate() {
            // make sure that the NetworkNavMeshAgentRubberbanding component is
            // ABOVE the player component, so that it gets updated before Player.cs.
            // -> otherwise it overwrites player's WASD velocity for local player
            //    hosts
            // -> there might be away around it, but a warning is good for now
            Component[] components = GetComponents<Component>();
            if (Array.IndexOf(components, GetComponent<NetworkNavMeshAgentRubberbanding>()) >
                Array.IndexOf(components, this))
                Debug.LogWarning(name + "'s NetworkNavMeshAgentRubberbanding component is below the Player component. Please drag it above the Player component in the Inspector, otherwise there might be WASD movement issues due to the Update order.");
                    
        }
        [ServerCallback] void OnAggro_Auto(Entity entity) {
            if(own.auto.on) {
                //if(entity == this) target = null;
                FindNearestTarget();
                if(entity != null && CanAttack(entity)) {
                    if (target == null) {
                        //target = entity;
                    }
                } else if (entity != target) // no need to check dist for same target
                {
                    float oldDistance = Vector3.Distance(transform.position, target.transform.position);
                    float newDistance = Vector3.Distance(transform.position, entity.transform.position);
                    //if (newDistance < oldDistance * 0.8) target = entity;
                }
            }
        }
    #endregion   
    }
    #region Sub Classes
    [Serializable]
    public struct AutoMode {
        public bool on;
        public int lastskill;
        public float followDistance;
        public bool collectGold;
        public bool collectitems;
        public double hpRecovery;
        public double manaRecovery;
        public string[] hpRecoveryPotions;
        public string[] manaRecoveryPotions;
    }
    [Serializable]
    public partial struct DailySignRewards {
        public int days;
        public ItemSlot[] rewards;
    }
    #endregion
}