using System.Collections.Generic;
using System;
using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Language", order = 999)]
    public class ScriptableLanguage : ScriptableObject
    {
        public LanguageDictionaryCodeNameDesc[] items;
        public LanguageDictionaryCodeWord[] words;
        public LanguageDictionaryCodeWord[] itemTypes;
        public LanguageDictionaryCodeWord[] monsters;
        public LanguageDictionaryCodeWord[] pets;
        public LanguageDictionaryCodeWord[] mounts;
        public LanguageDictionaryCodeWord[] cities;
        public LanguageDictionaryCodeWord[] classes;
        public LanguageDictionaryCodeWord[] tribes;
        public LanguageDictionaryCodeWord[] militaryRanks;
        public LanguageDictionaryCodeWord[] wardrop;
        public LanguageDictionaryCodeNameDesc[] achievements;
        public LanguageDictionaryCodeWord[] networkErrors;

        public void Fetch()
        {
            SetNameDescToDict(items, LanguageDictionaryCategories.ItemName, LanguageDictionaryCategories.ItemDesc);
            SetWordsToDict(words, LanguageDictionaryCategories.Word);
            SetWordsToDict(itemTypes, LanguageDictionaryCategories.ItemTypes);
            SetWordsToDict(cities, LanguageDictionaryCategories.City);
            SetWordsToDict(monsters, LanguageDictionaryCategories.Monster);
            SetWordsToDict(pets, LanguageDictionaryCategories.Pet);
            SetWordsToDict(mounts, LanguageDictionaryCategories.Mount);
            SetWordsToDict(classes, LanguageDictionaryCategories.Class);
            SetWordsToDict(tribes, LanguageDictionaryCategories.Tribe);
            SetWordsToDict(militaryRanks, LanguageDictionaryCategories.MilitaryRank);
            SetWordsToDict(wardrop, LanguageDictionaryCategories.Wardrop);
            SetWordsToDict(networkErrors, LanguageDictionaryCategories.NetworkError);
            SetNameDescToDict(achievements, LanguageDictionaryCategories.AchievementName, LanguageDictionaryCategories.AchievementRequirement);
        }
        void SetWordsToDict(LanguageDictionaryCodeWord[] data, LanguageDictionaryCategories type)
        {
            if(data.Length > 0)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    LanguageManger.dictionary[type][data[i].code] = data[i].name;
                }
            }
        }
        void SetNameDescToDict(LanguageDictionaryCodeNameDesc[] data, LanguageDictionaryCategories nameType, LanguageDictionaryCategories descType)
        {
            if(data.Length > 0)
            {
                for(int i = 0; i < data.Length; i++)
                {
                    LanguageManger.dictionary[nameType][data[i].code] = data[i].Name;
                    LanguageManger.dictionary[descType][data[i].code] = data[i].desc;
                }
            }
        }
        [Serializable] public struct LanguageDictionaryCodeWord
        {
            public new string name;
            public int code;
        }
        [Serializable] public struct LanguageDictionaryCodeNameDesc
        {
            public new int code;
            public string Name;
            [TextArea(1, 30)] public string desc;
        }
    }
}