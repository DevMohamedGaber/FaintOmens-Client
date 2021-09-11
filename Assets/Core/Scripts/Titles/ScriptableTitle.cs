using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Title", order = 999)]
    public class ScriptableTitle : ScriptableObjectNonAlloc
    {
        [Header("General")]
        [SerializeField] TitleCaregory category;
        [SerializeField] ItemSource source;
        [SerializeField] Sprite[] image = new Sprite[2];
        public float width = 200;
        public float height = 40;
        [Header("Bonus")]
        public ActivationBonus hp;
        public ActivationBonus mp;
        public ActivationBonus pAtk;
        public ActivationBonus pDef;
        public ActivationBonus mAtk;
        public ActivationBonus mDef;
        public ActivationFloatBonus block;
        public ActivationFloatBonus antiBlock;
        public ActivationFloatBonus critRate;
        public ActivationFloatBonus critDmg;
        public ActivationFloatBonus antiCrit;
        public ActivationFloatBonus antiStun;
        public string GetName()
        {
            return LanguageManger.GetWord(name, LanguageDictionaryCategories.TitleName);
        }
        public string GetSource()
        {
            return LanguageManger.GetWord((int)source, LanguageDictionaryCategories.Source);
        }
        public Sprite GetImage()
        {
            return image[(int)LanguageManger.current];
        }
        public static List<ScriptableTitle> GetByCategory(TitleCaregory targetCategory)
        {
            List<ScriptableTitle> result = new List<ScriptableTitle>();
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
        public int CalculateBR(bool isActive = false)
        {
            return 0;
        }
        /*public bool isActivated() {
            for(int i = 0; i < Player.localPlayer.own.titles.Count; i++) {
                if(Player.localPlayer.own.titles[i] == name)
                    return true;
            }
            return false;
        }*/

        //cash
        static Dictionary<int, ScriptableTitle> cache;
        public static Dictionary<int, ScriptableTitle> dict
        {
            get
            {
                // not loaded yet?
                if (cache == null)
                {
                    // get all ScriptableItems in resources
                    ScriptableTitle[] items = Resources.LoadAll<ScriptableTitle>("");

                    // check for duplicates, then add to cache
                    List<int> duplicates = items.ToList().FindDuplicates(item => item.name);
                    if (duplicates.Count == 0)
                    {
                        cache = items.ToDictionary(item => item.name, item => item);
                    }
                    else
                    {
                        foreach (int duplicate in duplicates)
                            Debug.LogError("Resources folder contains multiple ScriptableItems with the name " + duplicate + ". If you are using subfolders like 'Warrior/Ring' and 'Archer/Ring', then rename them to 'Warrior/(Warrior)Ring' and 'Archer/(Archer)Ring' instead.");
                    }
                }
                return cache;
            }
        }
    }
}