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
        protected float discount;
        public override void Refresh()
        {
            if(currentShop != null)
            {
                if(noItems != null)
                {
                    noItems.SetActive(currentShop.items.Length == 0);
                }
                if(currentShop.discountable)
                {
                    player.CmdCheckShopDiscount(currentShop.name);
                }
                else
                {
                    RefreshContent();
                }
            }
        }
        public virtual void OnSelect(int index)
        {
            uint maxCount = currentShop.MaxAvailable(index);
            counter.Limits(Math.Min(1, maxCount), maxCount);
            counter.Show(index);
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
        public virtual void SetDiscount(float discount)
        {
            this.discount = discount;
            RefreshContent();
        }
        public virtual void Show(int shopId)
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
        protected void RefreshContent()
        {
            int itemsCount = currentShop.items.Length;
            UIUtils.BalancePrefabs(prefab, itemsCount, content);
            if(itemsCount > 0)
            {
                for (int i = 0; i < itemsCount; i++)
                {
                    UIShopItem item = content.GetChild(i).GetComponent<UIShopItem>();
                    item.Set(currentShop.items[i], discount);
                    item.button.onClick = () => OnSelect(i);
                }
            }
        }
    }
}