using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using Game.UI;
namespace Game
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager data;
        [Header("Data")]
        [SerializeField] GameObject[] GUIs;
        public GameSettings gameSettings;
        public UIManager_Pages pages;
        public UIManager_UIAssets assets;
        public UIManager_InScene inScene;
        public UIManager_Lobby lobby;
        [Header("Management")]
        public UIWindowBase currenOpenWindow;
        public Stack<UIWindowBase> history = new Stack<UIWindowBase>();

        Player player => Player.localPlayer;

        [Header("General")]
        public UIMiniLoading miniLoading;
        public Game.Network.NetworkManagerMMO gameManager;

        [Header("Objects In Game")]
        public GameObject inGame;
        public UIControllers controllers;
        public UIRankingWindow RankingWindow;
        public UIPlayerPreview playerPreviewWindow;
        public UIMiniMap miniMap;
        public UIGuild guildWindow;
        public UIMiniNotifyIconsList notifiyIconsList;
        public UIGuildRecallMsg guildRecallMsg;
        public UIMiniChat miniChat;
        public UIChat chat;
        public GameObject success;
        public GameObject failure;
        public UI7DaysSignUp signUp7days;
        public UIWardrobe wardrobe;
        public UINewAchievementNotice achievementNotice;

        [Header("Lobby")]
        public UIChooseLanguage chooseLangPage;
        public UIGameUpdater gameUpdater;

        public void ClearWindows() {
            if(currenOpenWindow != null) currenOpenWindow.Close();
            if(history.Count > 0) history.Clear();
        }
        public void OnChangeLanguage(Languages lang) {
            gameSettings.currentLang = lang;
            if(GUIs.Length > 1) {
                for(int i = 0; i < GUIs.Length; i++)
                    GUIs[i].SetActive(i == (int)lang);
            }
            else GUIs[0].SetActive(true);
        }
        public void OnLocalPlayerNotFound() {
            ClearWindows();
        }
        
        void Awake() {
            if(data == null)
                data = this;
            SaveSystem.Load();
            Application.targetFrameRate = (int)gameSettings.fps;
        }
        void Update() {
            inGame.SetActive(player != null);
        }
        void OnDestroy() {
            SaveSystem.Save();
        }
        [Serializable]
        public struct UIManager_Pages {
            public UIPets pets;
            public UIMounts mounts;
            public UIArena arena;
            public UITrade trade;
        }
        [Serializable]
        public struct UIManager_UIAssets {
            public Sprite[] tiers;
            public Sprite[] stars;
            public Color[] tierColor;
            public Sprite[] avatars;
            public Sprite[] gender;
            public Sprite[] currency;
            public Sprite[] discountArrows;
            public GameObject itemSlotPrefab;
            public Sprite[] socketSlot; // 0 => locked; 1 => empty
            public Sprite defaultItem;
        }
        [Serializable] public struct UIManager_InScene {
            public UIExperienceBar expBar;
            public UIEventCounter counter;
            public UILevelUpNotice levelUpNotice;
            public UIArenaMatchNotify arenaNotify;
            public UIArenaMatchResult arenaMatchResult;
            public UIChangableInterface changableInterface;
            public UIRespawn respawn;
            public UIToolTip tooltip;
            public UISideBox sideBox;
        }
        [Serializable] public struct UIManager_Lobby {
            public UILobbyWindow current;
            public UILogin login;
            public UICharacterCreation create;
            public UICharacterSelection select;
        }
    }
}