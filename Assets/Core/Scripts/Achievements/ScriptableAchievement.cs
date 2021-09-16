using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
namespace Game
{
    public abstract class ScriptableAchievement : ScriptableObjectNonAlloc
    {
        public AchievementTypes type;
        public AchievementCategory category;
        public ScriptableAchievement successor;
        public byte points;
        public ItemSlot[] rewards;
        public virtual string GetDescription()
        {
            return String.Empty;
        }
        public virtual string GetProgress()
        {
            return String.Empty;
        }
        public virtual float GetProgressPercentage()
        {
            return float.NegativeInfinity;
        }
        // statics
        public static List<ScriptableAchievement> GetByCategory(AchievementCategory targetCategory)
        {
            List<ScriptableAchievement> result = new List<ScriptableAchievement>();
            if(dict.Count > 0)
            {
                for (int i = 0; i < dict.Count; i++)
                {
                    if(cache[i].category == targetCategory)
                    {
                        result.Add(cache[i]);
                    }
                }
            }
            return result;
        }
        public static int GetCategoryCount()
        {
            return Enum.GetValues(typeof(AchievementCategory)).Length;
        }
        // points
        public static ushort totalPoints = 0;
        public static ushort[] categoryTotalPoints;
        static void CalculateCachedPoints()
        {
            if(cache.Count > 0)
            {
                for (int i = 0; i < cache.Count; i++)
                {
                    totalPoints += (ushort)cache[i].points;
                }
            }
        }
        //cash
        static Dictionary<int, ScriptableAchievement> cache;
        public static Dictionary<int, ScriptableAchievement> dict
        {
            get
            {
                if (cache == null)
                {
                    ScriptableAchievement[] items = Resources.LoadAll<ScriptableAchievement>("");
                    List<int> duplicates = items.ToList().FindDuplicates(item => item.name);
                    if (duplicates.Count == 0)
                    {
                        cache = items.ToDictionary(item => item.name, item => item);
                        CalculateCachedPoints();
                    }
                    else
                    {
                        foreach (int duplicate in duplicates)
                            Debug.LogError("Resources folder contains multiple ScriptableAchievement with the name " + duplicate + ". If you are using subfolders like 'Warrior/Ring' and 'Archer/Ring', then rename them to 'Warrior/(Warrior)Ring' and 'Archer/(Archer)Ring' instead.");
                    }
                }
                return cache;
            }
        }
    }
}