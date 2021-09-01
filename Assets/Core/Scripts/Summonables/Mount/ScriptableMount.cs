using UnityEngine;
using System.Linq;
using System.Collections.Generic;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Summonables/Mount", order = 0)]
    public class ScriptableMount : ScriptableObjectNonAlloc
    {
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
        public LinearFloat speed = new LinearFloat();
        public ushort id => (ushort)name;
        public string Name => LanguageManger.GetWord(name, LanguageDictionaryCategories.Mount);
        public uint brMax => System.Convert.ToUInt32(
                                health.Get(Storage.data.pet.lvlCap) + mana.Get(Storage.data.pet.lvlCap) +
                                pAtk.Get(Storage.data.pet.lvlCap) + pDef.Get(Storage.data.pet.lvlCap) +
                                mAtk.Get(Storage.data.pet.lvlCap) + mDef.Get(Storage.data.pet.lvlCap) + ((
                                block.Get(Storage.data.pet.lvlCap) + untiBlock.Get(Storage.data.pet.lvlCap) +
                                crit.Get(Storage.data.pet.lvlCap) + untiCrit.Get(Storage.data.pet.lvlCap) +
                                critDmg.Get(Storage.data.pet.lvlCap) + speed.Get(Storage.data.pet.lvlCap)
                            ) * 100));
        static Dictionary<int, ScriptableMount> cache;
        public static Dictionary<int, ScriptableMount> dict
        { 
            get
            {
                if (cache == null)// not loaded yet?
                {
                    ScriptableMount[] mounts = Resources.LoadAll<ScriptableMount>("");
                    List<int> duplicates = mounts.ToList().FindDuplicates(mount => mount.name);
                    if (duplicates.Count == 0)
                    {
                        cache = mounts.ToDictionary(mount => mount.name, mount => mount);
                    }
                    else
                    {
                        foreach (int duplicate in duplicates)
                            Debug.LogError("Resources folder contains multiple ScriptableMount with ID:" + duplicate + ". If you are using subfolders like 'Warrior/Ring' and 'Archer/Ring', then rename them to 'Warrior/(Warrior)Ring' and 'Archer/(Archer)Ring' instead.");
                    }
                }
                return cache;
            }
        }
    }
}