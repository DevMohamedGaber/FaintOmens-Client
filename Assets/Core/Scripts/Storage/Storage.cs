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
        [HideInInspector] public Camera mainCam;
        [HideInInspector] public GameObject currentLoadedMap;

        public sd.Player player;
        public sd.Chat chat;
        public sd.Wardrobe wardrobe;
        public sd.Guild guild;
        public sd.Pet pet;
        public sd.Mount mount;
        public sd.Arena arena;
        public sd.Item item;
        public sd.Team team;
        public sd.Friend friend;
        public sd.DefaultAchievements achievements;
        public sd.Ratios ratios;
        public sd.Utils utils;
        public GameObject[] eventMaps;

        public DailySignRewards[] dailySignRewards;
        public ItemSlotArray[] SignUp7DaysEventsRewards;
        public List<First7DaysEventRewards> Recharge7DaysEventsRewards;

        void Awake() {
            data = this;
            item.OnAwake();
            mainCam = Camera.main;
        }
    }
}