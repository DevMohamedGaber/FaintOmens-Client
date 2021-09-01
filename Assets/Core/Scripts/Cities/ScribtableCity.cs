using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/City")]
    public class ScribtableCity : ScriptableObjectNonAlloc
    {
        public CityStatus status;
        public GameObject prefab;
        public string Name => LanguageManger.GetWord(name, LanguageDictionaryCategories.City);
        // cache
        static Dictionary<byte, ScribtableCity> cache;
        public static Dictionary<byte, ScribtableCity> dict
        { 
            get
            {
                if (cache == null)
                {// not loaded yet?
                    ScribtableCity[] cities = Resources.LoadAll<ScribtableCity>("");// get all ScribtableCity in resources
                    List<byte> duplicates = cities.ToList().FindDuplicates(city => (byte)city.name); // check for duplicates, then add to cache
                    if (duplicates.Count == 0)
                    {
                        cache = cities.ToDictionary(city => (byte)city.name, city => city);
                    }
                    else
                    {
                        for(byte i = 0; i < duplicates.Count; i++)
                            Debug.LogError($"Resources folder contains multiple ScribtableCity with ID: {duplicates[i]}");
                    }
                }
                return cache;
            }
        }
    }
}