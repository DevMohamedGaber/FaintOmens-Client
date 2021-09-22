using UnityEngine;
namespace Game.UI
{
    public class Workshop_Craft : SubWindowBase
    {
        [Header("List")]
        [SerializeField] Transform categoryContent;
        [SerializeField] GameObject categoryPrefab;
        [SerializeField] GameObject _recipePrefab;
        [SerializeField] UIToggleGroup _toggleGroup;
        [Header("Operation")]
        [SerializeField] UIItemSlot resultSlot;
        [SerializeField] UICountableItemSlot[] ingredientSlots;
        [SerializeField] UICountSwitch countSwitch;
        [SerializeField] TMPro.TMP_Text costTxt;
        public GameObject recipePrefab
        {
            get
            {
                return _recipePrefab;
            }
        }
        public UIToggleGroup toggleGroup
        {
            get
            {
                return _toggleGroup;
            }
        }
        ScriptableRecipe current = null;
        public override void Refresh()
        {
            if(current != null)
            {
                for (int i = 0; i < current.ingredients.Length; i++)
                {
                    ingredientSlots[i].SetAmount(player.InventoryCountById(current.ingredients[i].item.id), current.ingredients[i].amount * countSwitch.count);
                }
                countSwitch.SetMax(current.MaxCraftable());
                uint cost = current.cost * countSwitch.count;
                costTxt.text = cost.ToString();
                costTxt.color = player.own.gold >= cost ? Color.white : Color.red;
            }
        }
        public void OnSelect(int recipeId)
        {
            if(ScriptableRecipe.dict.TryGetValue(recipeId, out current))
            {
                resultSlot.Assign(current.result);
                for (int i = 0; i < ingredientSlots.Length; i++)
                {
                    if(i < current.ingredients.Length)
                    {
                        ingredientSlots[i].Assign(current.ingredients[i].item, player.InventoryCountById(current.ingredients[i].item.id), current.ingredients[i].amount);
                        if(!ingredientSlots[i].gameObject.activeSelf)
                        {
                            ingredientSlots[i].gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        ingredientSlots[i].gameObject.SetActive(false);
                    }
                }
                countSwitch.Limits(1, current.MaxCraftable());
                costTxt.text = current.cost.ToString();
                costTxt.color = player.own.gold >= current.cost ? Color.white : Color.red;
            }
        }
        public void OnCraft()
        {
            if(current == null)
            {
                Notifications.list.Add("Please choose a Recipe to craft.");
                return;
            } 
            if(countSwitch.count == 0)
            {
                Notifications.list.Add("You don't have the nessisary requirments.");
                return;
            }
            if(!current.CanCraft(countSwitch.count))
            {
                Notifications.list.Add("You can't craft this item.");
                return;
            }
            player.CmdCraft(current.name, countSwitch.count);
        }
        void Awake()
        {
            int[] tgArr = (int[]) System.Enum.GetValues(typeof(CraftCategory));
            UIUtils.BalancePrefabs(categoryPrefab, tgArr.Length, categoryContent);
            for (int i = 0; i < tgArr.Length; i++)
            {
                CraftCategoryList cat = categoryContent.GetChild(i).GetComponent<CraftCategoryList>();
                if(cat != null)
                {
                    cat.Set((CraftCategory)tgArr[i], this);
                }
            }
            toggleGroup.UpdateTogglesList(true);
        }
    }
}