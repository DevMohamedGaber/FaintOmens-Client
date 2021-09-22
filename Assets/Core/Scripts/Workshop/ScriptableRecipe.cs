using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace Game
{
    [CreateAssetMenu(fileName="New Recipe", menuName="Custom/Recipe", order=999)]
    public class ScriptableRecipe : ScriptableObjectNonAlloc
    {
        public CraftCategory category;
        public Item result; // item out
        public uint cost;
        public ItemSlot[] ingredients; // items in
        public string Name
        {
            get
            {
                return LanguageManger.GetWord(name, LanguageDictionaryCategories.CraftRecipeName);
            }
        }

        public virtual bool CanCraft(uint count)
        {
            if(Player.localPlayer.own.gold < cost)
            {
                return false;
            }
            for(int i = 0; i < ingredients.Length; i++)
            {
                if(!ingredients[i].isEmpty)
                {
                    if(Player.localPlayer.InventoryCountById(ingredients[i].item.id) < ingredients[i].amount)
                    {
                        return false; 
                    }
                }
            }
            return true;
        }
        public virtual uint MaxCraftable()
        {
            uint smallest = 0;
            for(int i = 0; i < ingredients.Length; i++)
            {
                uint itemCount = Player.localPlayer.InventoryCountById(ingredients[i].item.id);
                if(itemCount > 0)
                {
                    uint needed = (uint)(itemCount - (itemCount % ingredients[i].amount));
                    if((needed / ingredients[i].amount) < smallest || smallest == 0)
                    {
                        smallest = (uint)(needed / ingredients[i].amount);
                    }
                }
            }
            return smallest;
        }
        public static List<ScriptableRecipe> GetByCategory(CraftCategory targetCategory)
        {
            List<ScriptableRecipe> result = new List<ScriptableRecipe>();
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
        static Dictionary<int, ScriptableRecipe> cache;
        public static Dictionary<int, ScriptableRecipe> dict
        { 
            get
            {
                if (cache == null)
                {// not loaded yet?
                    ScriptableRecipe[] recipes = Resources.LoadAll<ScriptableRecipe>("");// get all ScriptableRecipes in resources
                    List<int> duplicates = recipes.ToList().FindDuplicates(recipe => recipe.name); // check for duplicates, then add to cache
                    if (duplicates.Count == 0)
                    {
                        cache = recipes.ToDictionary(recipe => recipe.name, recipe => recipe);
                    }
                    else
                    {
                        foreach (int duplicate in duplicates)
                            Debug.LogError("Resources folder contains multiple ScriptableRecipes with the name " + duplicate + ". If you are using subfolders like 'Warrior/Ring' and 'Archer/Ring', then rename them to 'Warrior/(Warrior)Ring' and 'Archer/(Archer)Ring' instead.");
                    }
                }
                return cache;
            }
        }
    }
}