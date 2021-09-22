using UnityEngine;
namespace Game.UI
{
    public class CraftRecipeButton : MonoBehaviour
    {
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] GameObject craftableObj;
        [SerializeField] TMPro.TMP_Text craftableTxt;
        [SerializeField] UIToggle _toggle;
        public UIToggle toggle
        {
            get
            {
                return _toggle;
            }
        }
        ScriptableRecipe recipe;
        public void Set(ScriptableRecipe recipe)
        {
            this.recipe = recipe;
            nameTxt.text = recipe.Name;
        }
        public void Refresh()
        {
            uint maxCraftable = recipe.MaxCraftable();
            craftableObj.SetActive(maxCraftable > 0);
            if(maxCraftable > 0)
            {
                craftableTxt.text = maxCraftable.ToString();
            }
        }
        void OnEnable() {
            Refresh();
        }
    }
}