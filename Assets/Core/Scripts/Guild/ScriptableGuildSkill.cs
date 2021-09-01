using UnityEngine;
using System.Linq;
using System.Collections.Generic;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Guild/Skill", order = 999)]
    public class ScriptableGuildSkill : ScriptableObjectNonAlloc
    {
        public Sprite icon;
        public uint[] cost;
        public int[] points;
        public string Name => LanguageManger.GetWord(name);
        public int Get(byte level) => level > 0 && level < points.Length ? points[level - 1] : 0;
        static Dictionary<int, ScriptableGuildSkill> cache;
        public static Dictionary<int, ScriptableGuildSkill> dict
        {
            get
            {
                if(cache == null)
                {
                    ScriptableGuildSkill[] items = Resources.LoadAll<ScriptableGuildSkill>("");
                    List<int> duplicates = items.ToList().FindDuplicates(item => item.name);
                    if(duplicates.Count == 0)
                    {
                        cache = items.ToDictionary(item => item.name, item => item);
                    }
                    else
                    {
                        foreach(int duplicate in duplicates)
                            Debug.LogError("Resources folder contains multiple ScriptableGuildSkill with the name " + duplicate + ". If you are using subfolders like 'Warrior/Ring' and 'Archer/Ring', then rename them to 'Warrior/(Warrior)Ring' and 'Archer/(Archer)Ring' instead.");
                    }
                }
                return cache;
            }
        }
    }
}