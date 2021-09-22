/* - MODIFICATIONS TO THE MAIN CLASS -
- remove the original class

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Game.UI
{
    public class UICrafting : MonoBehaviour {
        [SerializeField] float UpdateInterval = 1f;
        [SerializeField] UILanguageDefiner lang;
        [SerializeField] Text playerGold;
        [SerializeField] Transform content;
        [SerializeField] Transform reqContent;
        [SerializeField] UIItemSlot resultItem;
        [SerializeField] GameObject reqItem;
        [SerializeField] GameObject categoryPrefab;
        [SerializeField] GameObject recipePrefab;
        [SerializeField] Text costTxt;
        [SerializeField] Text countTxt;
        [SerializeField] Button inCountBtn;
        [SerializeField] Button deCountBtn;
        [SerializeField] Button maxCountBtn;
        [SerializeField] Button minCountBtn;
        Player player => Player.localPlayer;
        List<UICollapsableCategory> categories = new List<UICollapsableCategory>();
        ushort current;
        int currentCategory = -1;
        uint count = 0;
        uint maxCount = 0;

        void UpdateData() {
            playerGold.text = player.own.gold.ToString();
            if(current > 0 && ScriptableRecipe.dict.TryGetValue(current, out ScriptableRecipe recipe)) {
                UpdateCostColor();
                for(int i = 0; i < reqContent.childCount; i++) {
                    uint amount = player.InventoryCountById(recipe.ingredients[i].item.name);
                    reqContent.GetChild(i).GetComponent<UICountableItemSlot>().SetAmount(amount);
                }
            }
            if(currentCategory > -1) {
                for(int i = 0; i < categories[currentCategory].content.childCount; i++) {
                    UICraftingRecipeButton recipeBtn = categories[currentCategory].content.GetChild(i).GetComponent<UICraftingRecipeButton>();
                    recipeBtn.count.text = ScriptableRecipe.dict[recipeBtn.id].MaxCraftable(player).ToString();
                }
            }
        }
        public void OnSelectCategory(int index) {
            currentCategory = currentCategory != index ? index : -1;
            for(int i = 0; i < categories.Count; i++)
                categories[i].content.gameObject.SetActive(i == currentCategory);
        }
        public void OnSelectRecipe(int recipeId) {
            if(ScriptableRecipe.dict.TryGetValue(recipeId, out ScriptableRecipe recipe)) {
                current = (ushort)recipeId;
                costTxt.text = recipe.cost.ToString();
                resultItem.Assign(recipe.result);
                UIUtils.BalancePrefabs(reqItem, recipe.ingredients.Length, reqContent);
                for(int i = 0; i < recipe.ingredients.Length; i++) {
                    uint amount = player.InventoryCountById(recipe.ingredients[i].item.name);
                    UICountableItemSlot slot = reqContent.GetChild(i).GetComponent<UICountableItemSlot>();
                    slot.Assign(recipe.ingredients[i].item.name);
                    slot.SetAmount(amount, recipe.ingredients[i].amount);
                }
                maxCount = recipe.MaxCraftable(player);
                count = maxCount >= 1 ? 1u : 0u;
                UpdateCounter();
            }
        }
        void UpdateCostColor() {
            if(current > -1) {
                if(count > 0) costTxt.color = player.own.gold >= (ScriptableRecipe.dict[current].cost * count) ? Color.white : Color.red;
                else costTxt.color = player.own.gold >= ScriptableRecipe.dict[current].cost ? Color.white : Color.red;
            }
        }
        void UpdateCounter() {
            countTxt.text = count.ToString();
            countTxt.color = count > 0 ? Color.white : Color.red;
            costTxt.text = count > 0 ? (ScriptableRecipe.dict[current].cost * count).ToString() : ScriptableRecipe.dict[current].cost.ToString();
            UpdateCostColor();
            inCountBtn.interactable = maxCount > 0 && count < maxCount;
            deCountBtn.interactable = maxCount > 0 && count > 0;
            maxCountBtn.interactable = maxCount > 0 && count < maxCount;
            minCountBtn.interactable = maxCount > 0 && count > 1;
        }
        public void OnIncreaseCount() {
            if(count < maxCount) {
                count = (count + 1) <= maxCount ? count + 1 : maxCount;
                UpdateCounter();
            }
        }
        public void OnDecreaseCount() {
            if(count > 0) {
                count--;
                UpdateCounter();
            }
        }
        public void OnMaximumCount() {
            if(count < maxCount) {
                count = maxCount;
                UpdateCounter();
            }
        }
        public void OnMinimumCount() {
            count = maxCount > 0 && count > 1 ? 1u : 0u;
            UpdateCounter();
        }
        public void OnCraft() {
            if(current < 1) 
                UINotifications.list.Add("Please choose a Recipe to craft.");
            else if(count == 0)
                UINotifications.list.Add("You don't have the nessisary requirments.");
            else if(!ScriptableRecipe.dict[current].CanCraft(player, count))
                UINotifications.list.Add("You can't craft this item.");
            else
                player.CmdCraft(current, count);
        }
        public void Show() => gameObject.SetActive(true);
        void OnEnable() {
            if(player != null) {
                InvokeRepeating(nameof(UpdateData), UpdateInterval, UpdateInterval);
            }
        }
        void OnDisable() {
            CancelInvoke(nameof(UpdateData));
            current = 0;
            currentCategory = -1;
            count = 0;
            maxCount = 0;
            resultItem.Unassign();
            if(reqContent.childCount > 0) {
                for(int i = 0; i < reqContent.childCount; i++)
                    Destroy(reqContent.GetChild(i).gameObject);
            }
        }
        void Awake() {
            List<CraftCategory> categoryList = new List<CraftCategory>();
            foreach(CraftCategory category in Enum.GetValues(typeof(CraftCategory)))
                categoryList.Add(category);
            
            UIUtils.BalancePrefabs(categoryPrefab, categoryList.Count, content);
            for(int i = 0; i < categoryList.Count; i++) {
                UICollapsableCategory catObj = content.GetChild(i).GetComponent<UICollapsableCategory>();
                int iCopy = i;
                lang.Add(new UITextObjectIdentifier {
                    obj = catObj.Name,
                    code = (int)categoryList[iCopy]
                });
                catObj.btn.onClick.SetListener(() => OnSelectCategory(iCopy));
                categories.Add(catObj);
                List<int> keys = new List<int>(ScriptableRecipe.dict.Keys);
                for(int r = 0; r < keys.Count; r++) {
                    if(ScriptableRecipe.dict[keys[r]].category == categoryList[i]) {
                        UICraftingRecipeButton recipe = Instantiate(recipePrefab, catObj.content, false).GetComponent<UICraftingRecipeButton>();
                        lang.Add(new UITextObjectIdentifier {
                            obj = recipe.Name,
                            category = LanguageDictionaryCategories.ItemName,
                            code = ScriptableRecipe.dict[keys[r]].result.data.name
                        });
                        recipe.id = keys[r];
                        recipe.btn.onClick.SetListener(() => OnSelectRecipe(recipe.id));
                    }
                }
            }
            resultItem.Unassign();
            lang.Refresh();
        }
    }
}*/