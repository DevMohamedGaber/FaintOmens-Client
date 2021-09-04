using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using sd = Game.StorageData;
namespace Game
{
    public class Storage : MonoBehaviour
    {
        public static Storage data;

        public sd.Player player;
        public sd.Wardrobe wardrobe;
        public sd.Guild guild;
        public sd.Pet pet;
        public sd.Mount mount;
        public sd.Arena arena;
        public sd.Item item;
        public sd.Team team;

        public GameObject[] eventMaps;

        public Sprite[] avatars;
        public Sprite[] avatarFrames;
        public Sprite[] currencyIcons; // 0=> gold, 1=> diamonds, 2=> b.diamonds
        public DailySignRewards[] dailySignRewards;
        public ItemSlotArray[] SignUp7DaysEventsRewards;
        public List<First7DaysEventRewards> Recharge7DaysEventsRewards;
        public List<City> cities;
        public Item[] inventoryShopItems;
        public List<MallItemsCategory> ItemMallContent = new List<MallItemsCategory>();
        public Sprite[] socketIcons;
        public Sprite[] classTypeIcons;
        public Sprite lockImage;
        public Sprite[] starsImages;
        // info
        [HideInInspector] public GameObject currentLoadedMap;
        [Header("Player Static Info")]
        public int maxDailyQuests = 20;
        public int BasicFreeRespawn = 3;
        public int ReviveHereCost = 30;
        public int dailyQuestsLimitPerDay = 5;
        public int chatMaxMsgSize = 70;
        public int removeGemCost = 2000;
        public ExponentialLong itemUpgradeCost = new ExponentialLong{ multiplier=5000, baseValue=2f };
        public int maxFriendsCount = 100;
        public int playerClassPromotionCount = 4;
        public Color partyMemberColor;
        public Color guildMemberColor;
        public int charactersPerAccount = 4;
        public GameObject[] basicBody = new GameObject[2];
        [Header("Wardrobe")]
        public int wardropUpgradeItem;
        public ExponentialInt wardropUpgradeStones;
        public ExponentialUInt wardropUpgradeGold;
        [Header("Mounts")]
        public Mount[] mounts;
        public ExponentialUInt mountExpMax = new ExponentialUInt{multiplier=50, baseValue=1.1f};
        public int mountMaxLevel = 100;
        public int mountFirstFeedItem = 7500;
        public long[] mountFeedItemsExp = new long[] {500, 1000, 3000, 5000};
        [Header("Ratios")]
        public int BoundToUnboundRatio = 2;
        public float attackToMoveRangeRatio = 0.8f;
        public int AP_Vitality = 100;
        public int AP_Strength_ATK = 10;
        public int AP_Strength_DEF = 5;
        public int AP_Intelligence_ATK = 10;
        public int AP_Intelligence_DEF = 5;
        public int AP_Intelligence_MANA = 10;
        public int AP_Endurance = 15;
        [Header("Team")]
        public int teamMaxCapacity = 4;
        public float teamMemberBonus = .1f;
        public float teamInviteWaitSeconds = 3;
        [Header("Guild")]
        public ExponentialUInt guildExpMax = new ExponentialUInt{multiplier=1000, baseValue=2f};

        void Awake() {
            data = this;
            item.OnAwake();
        }
    }
    [Serializable] public class MallItemsCategory {
        public string category;
        public List<Item> items = new List<Item>();
        public bool bound;
    }
}