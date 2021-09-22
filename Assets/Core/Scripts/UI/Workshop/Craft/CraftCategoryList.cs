using System.Collections.Generic;
using UnityEngine;
namespace Game.UI
{
    public class CraftCategoryList : MonoBehaviour
    {
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] Transform content;
        public void Set(CraftCategory category, Workshop_Craft craftWindow)
        {
            List<ScriptableRecipe> recipes = ScriptableRecipe.GetByCategory(category);
            if(recipes.Count > 0)
            {
                nameTxt.text = LanguageManger.GetWord((int)category, LanguageDictionaryCategories.CraftCategory);
                UIUtils.BalancePrefabs(craftWindow.recipePrefab, recipes.Count, content);
                for (int i = 0; i < recipes.Count; i++)
                {
                    CraftRecipeButton btn = content.GetChild(i).GetComponent<CraftRecipeButton>();
                    if(btn != null)
                    {
                        btn.Set(recipes[i]);
                        btn.toggle.onSelect = () => craftWindow.OnSelect(recipes[i].name);
                    }
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}