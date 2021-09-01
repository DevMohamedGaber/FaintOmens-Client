using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Game.UI;
namespace Game
{
    public class PlayerOwnData : NetworkBehaviourNonAlloc
    {
        [SerializeField] Player own;
    #region Basic Informations
        [Header("Basic")]
        [SyncVar(hook="OnGoldChanged")] public uint gold;
        [SyncVar(hook="OnDiamondsChanged")] public uint diamonds;
        [SyncVar(hook="OnBDiamondsChanged")] public uint b_diamonds;
        [SyncVar] public uint popularity;
        [SyncVar] public double createdAt;
        [SyncVar] public double reviveTime;
        [SyncVar(hook="OnInventorySizeChanged")] public byte inventorySize;
        [SyncVar] public double nextRiskyActionTime;
        [SyncVar(hook = "OnOccupationChanged")] public PlayerOccupation occupation;
        public SyncListItemSlot inventory = new SyncListItemSlot();
        public SyncListItemSlot accessories = new SyncListItemSlot();
        public SyncListUShort wardrobe = new SyncListUShort();
    #endregion
    #region Attribute Points
        [Header("Attributes")]
        [SyncVar] public ushort strength;
        [SyncVar] public ushort intelligence;
        [SyncVar] public ushort vitality;
        [SyncVar] public ushort endurance;
        [SyncVar] public ushort freepoints;
    #endregion
    #region Daily
        [Header("Dailys")]
        [SyncVar] public byte dailyQuests;
        public SyncListByte dsDays = new SyncListByte();
        public SyncListByte dsRewards = new SyncListByte();
    #endregion
    #region Military
        [Header("Military")]
        [SyncVar] public ushort killStrike = 0;
        [SyncVar] public ushort MonstersKillCount = 0;
        [SyncVar] public ushort TodayHonor;
        [SyncVar] public uint TotalHonor;
        [SyncVar] public uint MonsterPoints = 0;
        [SyncVar] public byte militaryRank = 0;
    #endregion
    #region Guild
        [Header("Guild")]
        [SyncVar] public Guild guild;
        [SyncVar] public GuildRank guildRank;
        [SyncVar] public uint guildContribution;
        public SyncListByte guildSkills = new SyncListByte();
    #endregion
    #region Tribe
        [Header("Tribe")]
        [SyncVar] public Tribe tribe;
        [SyncVar] public TribeRank tribeRank;
        [SyncVar] public byte tribeQuests;
        [SyncVar] public uint tribeGoldContribution;
        [SyncVar] public uint tribeDiamondContribution;
    #endregion
    #region Social
        [Header("Social")]
        [SyncVar] public Marriage marriage;
        [SyncVar] public Team team;
        public SyncListFriend friends = new SyncListFriend();
    #endregion
    #region Arena
        [SyncVar] public ushort arena1v1WinsToday;
        [SyncVar] public ushort arena1v1LossesToday;
        [SyncVar] public ushort arena1v1Points;
    #endregion
    #region Character Related
        [SyncVar(hook = "OnPetExpShareChanged")] public bool shareExpWithPet;
        [SyncVar] public VIP vip;
        [SyncVar] public Archive archive;
        [SyncVar] public AutoMode auto;
        public SyncListPets pets = new SyncListPets();
        public SyncListMounts mounts = new SyncListMounts();
        public SyncListQuest quests = new SyncListQuest();
        public SyncListMail mailBox = new SyncListMail();
        public SyncListUShort titles = new SyncListUShort();
        public SyncListAchievements achievements = new SyncListAchievements();
        //public SyncListHotEventsProgress HotEventsProgress;
        public SyncDictionaryIntDouble itemCooldowns = new SyncDictionaryIntDouble();
    #endregion
    #region Invitations
        public SyncListTeamInvitations teamInvitations = new SyncListTeamInvitations();
        public SyncListFriendRequest friendRequests = new SyncListFriendRequest();
        public SyncListMarriageProposals marriageProposals = new SyncListMarriageProposals();
        public SyncListTradeInvitations tradeInvitations = new SyncListTradeInvitations();
    #endregion
    #region Leveling
        [Header("Experience")]
        [SyncVar(hook = "OnExperienceChanged")] uint _experience = 0;
        public uint experience => _experience;
        public uint experienceMax => Storage.data.player.expMax.Get(own.level);
        #endregion
        public override void OnStartClient()
        {
            inventory.Callback += OnInventoryChanged;
            mailBox.Callback += OnMailBoxChanged;
            achievements.Callback += OnAchievementsChanged;
            pets.Callback += OnPetsChanged;
            mounts.Callback += OnMountsChanged;
            tradeInvitations.Callback += OnTradeInvitationsChanged;
        }
    #region on lists changed
        void OnInventoryChanged(SyncListItemSlot.Operation operation, int index, ItemSlot oldItem, ItemSlot newItem) {
            RefreshCurrentWindow();
        }
        void OnMailBoxChanged(SyncListMail.Operation op, int index, Mail oldMail, Mail newMail) {
            if(op == SyncListMail.Operation.OP_ADD || op == SyncListMail.Operation.OP_INSERT)
                UIManager.data.notifiyIconsList.ShowNewMail();
        }
        void OnAchievementsChanged(SyncListAchievements.Operation op, int index, Achievement oldAch, Achievement newAch) {
            if(op == SyncListAchievements.Operation.OP_ADD)
                UIManager.data.achievementNotice.Show(newAch);
        }
        void OnPetsChanged(SyncListPets.Operation op, int index, PetInfo oldPet, PetInfo newPet) {
            if(op == SyncListPets.Operation.OP_ADD && UIManager.data.pages.pets.IsVisible())
                UIManager.data.pages.pets.OnPetUpdated(newPet);
        }
        void OnMountsChanged(SyncListMounts.Operation op, int index, Mount oldValue, Mount newValue) {
            if(op == SyncListMounts.Operation.OP_ADD && UIManager.data.pages.mounts.IsVisible())
                UIManager.data.pages.mounts.OnMountUpdated(newValue);
        }
        void OnTradeInvitationsChanged(SyncListTradeInvitations.Operation op, int index, TradeInvitation oldValue, TradeInvitation newValue) {
            
        }
    #endregion
    #region on variables changed
        void OnGoldChanged(uint oldGold, uint newGold) {
            OnCurrencyChangedUpdateWindow();
        }
        void OnDiamondsChanged(uint oldGold, uint newGold) {
            OnCurrencyChangedUpdateWindow();
        }
        void OnBDiamondsChanged(uint oldGold, uint newGold) {
            OnCurrencyChangedUpdateWindow();
        }
        void OnPetExpShareChanged(bool oldShare, bool newShare) {
            if(oldShare != newShare && UIManager.data.pages.pets.IsVisible())
                UIManager.data.pages.pets.UpdateExpShare();
        }
        void OnExperienceChanged(uint oldExp, uint newExp) {
            UIManager.data.inScene.expBar.UpdateData();
        }
        void OnInventorySizeChanged(byte oldSize, byte newSize)
        {
            if(UIManager.data.currenOpenWindow != null)
            {
                if(UIManager.data.currenOpenWindow is UIInventory window)
                {
                    window.Refresh();
                }
            }
        }
        void OnOccupationChanged(PlayerOccupation oldOcc, PlayerOccupation newOcc) {
            // update UI
            UIManager.data.inScene.changableInterface.UpdateView(oldOcc, newOcc);
            // pages
            if(UIManager.data.currenOpenWindow != null)
            {
                if(UIManager.data.currenOpenWindow is UIArena arenaWindow)
                {
                    arenaWindow.Refresh();
                }
            }
            // counter
            if(newOcc == PlayerOccupation.RegisteredArena1v1)
            {
                UIManager.data.inScene.counter.StartCounter("Waiting for Arena 1v1", "في انتظار الحلبة 1 ضد 1");
            }
            else
            {
                UIManager.data.inScene.counter.StopCounter();
            }
        }
    #endregion
    #region Helpers
        public float ExperiencePercent() => (experience != 0 && experienceMax != 0) ? (float)experience / (float)experienceMax : 0;
        void OnCurrencyChangedUpdateWindow() {
            if(UIManager.data.currenOpenWindow != null) {
                UIManager.data.currenOpenWindow.UpdateCurrency();
            }
        }
        bool RefreshCurrentWindow() {
            if(UIManager.data.currenOpenWindow != null) {
                UIManager.data.currenOpenWindow.Refresh();
                return true;
            }
            return false;
        }
    #endregion
    }
}