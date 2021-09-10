using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using Game.UI;
using ml = Game.UI.ManagerLists;
namespace Game
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager data;
        [Header("Data")]
        [SerializeField] GameObject[] GUIs;
        public GameSettings gameSettings;
        public ml.Pages pages;
        public ml.Assets assets;
        public ml.InScene inScene;
        public ml.Lobby lobby;
        // Management
        [HideInInspector] public UIWindowBase currenOpenWindow;
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

        public void ClearWindows()
        {
            if(currenOpenWindow != null)
            {
                currenOpenWindow.Close();
            }
            if(history.Count > 0)
            {
                history.Clear();
            }
        }
        public void OnChangeLanguage(Languages lang)
        {
            gameSettings.currentLang = lang;
            if(GUIs.Length > 1)
            {
                for(int i = 0; i < GUIs.Length; i++)
                {
                    GUIs[i].SetActive(i == (int)lang);
                }
            }
            else
            {
                GUIs[0].SetActive(true);
            }
        }
        public void OnLocalPlayerNotFound()
        {
            ClearWindows();
        }
        
        void Awake()
        {
            if(data == null)
            {
                data = this;
            }
            SaveSystem.Load();
            Application.targetFrameRate = (int)gameSettings.fps;
        }
        void Update()
        {
            inGame.SetActive(player != null);
        }
        void OnDestroy()
        {
            SaveSystem.Save();
        }
    }
}