using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/General", order=0)]
    public class ScriptableItem : ScriptableObjectNonAlloc
    {
        [Header("Base Stats")]
        public Sprite image;
        public Quality quality;
        public int maxStack = 999;
        public uint buyPrice;
        public uint sellPrice;
        public uint itemMallPrice;
        public bool sellable = true;
        public bool tradable = true;
        public bool destroyable = true;

        [Header("Requirments")]
        public PlayerClassData reqClass = new PlayerClassData(PlayerClass.Any);
        public ItemType type = ItemType.Item;
        public byte minLevel = 1;
        public string Name => LanguageManger.GetWord(name, LanguageDictionaryCategories.ItemName);

        // cache
        static Dictionary<int, ScriptableItem> cache;
        public static Dictionary<int, ScriptableItem> dict
        {
            get
            {
                // not loaded yet?
                if (cache == null)
                {
                    // get all ScriptableItems in resources
                    ScriptableItem[] items = Resources.LoadAll<ScriptableItem>("");

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

        void OnValidate()
        {
            sellPrice = Math.Min(sellPrice, buyPrice);
        }
    }
}