using UnityEngine;
using System.Linq;
using System.Collections.Generic;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Class", order = 0)]
    public class ScriptableClass : ScriptableObjectNonAlloc
    {
        [Header("Info")]
        public Sprite icon;
        public GameObject prefab;
        public SubclassInfo[] subs;
        public Quality quality;
        public LinearInt healthMax = new LinearInt{baseValue=100};
        public LinearInt manaMax = new LinearInt{baseValue=100};
        public LinearInt pAtk = new LinearInt{baseValue=100};
        public LinearInt mAtk = new LinearInt{baseValue=100};
        public ScriptableQuality qualityData => ScriptableQuality.dict[(int)quality];
        //public bool CanPromote(Player player) => player.level == reqLevel && player.battlepower == reqBR
        //                                        && player.own.militaryRank == reqMilitaryRank;
        public bool CanShowPromote(int playerLevel) => /*playerLevel == reqLevel*/false;
        public static bool ValidateMainClass(byte classId) => classId > 0 && classId < 5;
        //cash
        static Dictionary<PlayerClass, ScriptableClass> cache;
        public static Dictionary<PlayerClass, ScriptableClass> dict
        {
            get
            {
                if(cache == null)
                {
                    ScriptableClass[] items = Resources.LoadAll<ScriptableClass>("");
                    List<int> duplicates = items.ToList().FindDuplicates(item => item.name);
                    if(duplicates.Count == 0)
                    {
                        cache = items.ToDictionary(item => (PlayerClass)item.name, item => item);
                    }
                    else
                    {
                        foreach(int duplicate in duplicates)
                            Debug.LogError("Resources folder contains multiple ScriptableClass with the name " + duplicate + ". If you are using subfolders like 'Warrior/Ring' and 'Archer/Ring', then rename them to 'Warrior/(Warrior)Ring' and 'Archer/(Archer)Ring' instead.");
                    }
                }
                return cache;
            }
        }
    }
}