using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/ScriptableWardrop", order = 0)]
    public class ScriptableWardrobe : ScriptableObjectNonAlloc
    {
        [Header("General")]
        public ClothingCategory category;
        public ItemSource source;
        public Quality quality;
        public ushort itemId;
        public string Name => itemId > 0 ? ScriptableItem.dict[itemId].Name : LanguageManger.Decide("Unknown", "غير معروف");
        public Sprite image => itemId > 0 ? ScriptableItem.dict[itemId].GetImage() : UIManager.data.assets.defaultItem;
        public GameObject[] modelPrefab;
        [Header("Bonus")]
        public LinearInt hp;
        public LinearInt mp;
        public LinearInt atk;
        public LinearInt def;

        //static
        public static List<ScriptableWardrobe> Get(ClothingCategory cat)
        {
            List<ScriptableWardrobe> results = new List<ScriptableWardrobe>();
            if(dict.Count > 0)
            {
                foreach(ScriptableWardrobe item in dict.Values)
                {
                    if(item.category == cat)
                    {
                        results.Add(item);
                    }
                }
            }
            return results;
        }
        public static bool HasWardropItem(SyncListClothing clothing, short itemId)
        {
            if(clothing.Count > 0)
            {
                for(int i = 0; i < clothing.Count; i++)
                {
                    if(clothing[i].id == itemId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //cash
        static Dictionary<int, ScriptableWardrobe> cache;
        public static Dictionary<int, ScriptableWardrobe> dict
        {
            get
            {
                if(cache == null)
                {
                    ScriptableWardrobe[] items = Resources.LoadAll<ScriptableWardrobe>("");
                    List<int> duplicates = items.ToList().FindDuplicates(item => item.name);
                    if(duplicates.Count == 0)
                    {
                        cache = items.ToDictionary(item => item.name, item => item);
                    }
                    else
                    {
                        foreach(int duplicate in duplicates)
                            Debug.LogError("Resources folder contains multiple ScriptableWardrobe with the name " + duplicate + ". If you are using subfolders like 'Warrior/Ring' and 'Archer/Ring', then rename them to 'Warrior/(Warrior)Ring' and 'Archer/(Archer)Ring' instead.");
                    }
                }
                return cache;
            }
        }
    }
}