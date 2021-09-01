using UnityEngine;
using System.Linq;
using System.Collections.Generic;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Summonables/Pet", order = 0)]
    public class ScriptablePet : ScriptableObjectNonAlloc
    {
        public string Name => LanguageManger.GetWord(name, LanguageDictionaryCategories.Pet);
        public int ActivateItemId;
        public GameObject prefab;
        public Sprite avatar;
        public DamageType classType;
        public Tier maxTier;
        public LinearInt health = new LinearInt{ baseValue=100 };
        public LinearInt mana = new LinearInt{ baseValue=100 };
        public LinearInt pAtk = new LinearInt{ baseValue=5 };
        public LinearInt pDef = new LinearInt{ baseValue=1 };
        public LinearInt mAtk = new LinearInt{ baseValue=5 };
        public LinearInt mDef = new LinearInt{ baseValue=1 };
        public LinearFloat block = new LinearFloat();
        public LinearFloat untiBlock = new LinearFloat();
        public LinearFloat crit = new LinearFloat();
        public LinearFloat critDmg = new LinearFloat();
        public LinearFloat untiCrit = new LinearFloat();
        public uint brMax => System.Convert.ToUInt32(
            health.Get(lvlCap) + mana.Get(lvlCap) + pAtk.Get(lvlCap) + pDef.Get(lvlCap) + mAtk.Get(lvlCap) + mDef.Get(lvlCap)
            + (( block.Get(lvlCap) + untiBlock.Get(lvlCap) + crit.Get(lvlCap) + untiCrit.Get(lvlCap) + critDmg.Get(lvlCap)) * 100)
        );
        byte lvlCap => Storage.data.pet.lvlCap;
        //cache
        static Dictionary<int, ScriptablePet> cache;
        public static Dictionary<int, ScriptablePet> dict
        { 
            get
            {
                if (cache == null)
                {// not loaded yet?
                    ScriptablePet[] pets = Resources.LoadAll<ScriptablePet>("");// get all ScriptablePet in resources
                    List<int> duplicates = pets.ToList().FindDuplicates(pet => pet.name); // check for duplicates, then add to cache
                    if (duplicates.Count == 0)
                    {
                        cache = pets.ToDictionary(pet => pet.name, pet => pet);
                    }
                    else
                    {
                        foreach (int duplicate in duplicates)
                            Debug.LogError("Resources folder contains multiple ScriptablePet with ID:" + duplicate + ". If you are using subfolders like 'Warrior/Ring' and 'Archer/Ring', then rename them to 'Warrior/(Warrior)Ring' and 'Archer/(Archer)Ring' instead.");
                    }
                }
                return cache;
            }
        }
    }
}