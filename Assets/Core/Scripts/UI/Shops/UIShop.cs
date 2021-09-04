using UnityEngine;
using System;
namespace Game.UI
{
    public class UIShop : UIWindowBase
    {
        [Header("Shop")]
        [SerializeField] Transform content;
        [SerializeField] GameObject prefab;
        [SerializeField] GameObject noItems;
        [SerializeField] protected UICountSwitch counter;
        protected ScriptableShop currentShop;
        public override void Refresh()
        {
            if(currentShop != null)
            {
                int itemsCount = currentShop.items.Length;
                noItems.SetActive(itemsCount == 0);
                UIUtils.DestroyChildren(content);
                items = new UIShopItem[itemsCount];
                if(itemsCount > 0)
                {
                    for (int i = 0; i < itemsCount; i++)
                    {
                        UIShopItem item = Instantiate(prefab, content, false).transform.GetComponent<UIShopItem>();
                        item.Set(currentShop.items[i]);
                        item.button.onClick = () => OnSelect(i);
                    }
                }
            }
        }
        public virtual void OnConfirm()
        {
            if(counter.index == -1)
            {
                Notify.SelectItemFirst();
                return;
            }
            if(!counter.IsValid())
            {
                Notify.list.Add("Count isn't valid");
                return;
            }
            if(!currentShop.HasCost(counter.index, counter.count))
                return;
        }
        public void Show(int shopId)
        {
            if(ScriptableShop.dict.TryGetValue(shopId, out ScriptableShop shopData))
            {
                currentShop = shopData;
                Refresh();
                base.Show();
            }
            else
            {
                Notify.list.Add("Shop not found", "لم نجد هذا المتجر");
            }
        }
        void OnSelect(int index)
        {
            currenItem = index;
            uint maxCount = currentShop.MaxAvailable(index);
            counter.Limits(Math.Min(1, maxCount), maxCount);
            counter.Show(index);
        }
    }
}