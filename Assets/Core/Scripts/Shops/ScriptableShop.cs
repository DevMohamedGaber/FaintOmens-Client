using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
namespace Game
{
    [CreateAssetMenu(menuName = "Custom/Shop", order = 0)]
    public class ScriptableShop : ScriptableObjectNonAlloc
    {
        public ShopItem[] items;
        Player player => Player.localPlayer;

        public bool HasCost(int itemIndex, uint count)
        {
            if(itemIndex < 0 || itemIndex >= items.Length || count < 1)
            {
                Notify.SelectItemFirst();
                return false;
            }
            ShopCurrency currency = items[itemIndex].currency;
            uint totalCost = items[itemIndex].cost * count;
            if(currency == ShopCurrency.Gold && player.own.gold < totalCost)
            {
                Notify.DontHaveEnoughGold();
                return false;
            }
            else if(currency == ShopCurrency.Diamonds && player.own.diamonds < totalCost)
            {
                Notify.DontHaveEnoughDiamonds();
                return false;
            }
            else if(currency == ShopCurrency.BDiamonds && player.own.b_diamonds < totalCost)
            {
                Notify.DontHaveEnoughBDiamonds();
                return false;
            }

            return true;
        }
        public uint MaxAvailable(int itemIndex)
        {
            if(itemIndex < 0 || itemIndex >= items.Length)
            {
                Notify.SelectItemFirst();
                return false;
            }
            ShopItem item = items[itemIndex];
            if(currency == ShopCurrency.Gold)
            {
                return Math.Min((uint)Math.Floor(player.own.gold / item.cost), item.maxPerCheckout) ;
            }
            else if(currency == ShopCurrency.Diamonds)
            {
                return Math.Min((uint)Math.Floor(player.own.diamonds / item.cost), item.maxPerCheckout);
            }
            else if(currency == ShopCurrency.BDiamonds)
            {
                return Math.Min((uint)Math.Floor(player.own.b_diamonds / item.cost), item.maxPerCheckout);
            }
        }
        static Dictionary<int, ScriptableShop> cache;
        public static Dictionary<int, ScriptableShop> dict
        { 
            get
            {
                if (cache == null) // not loaded yet?
                {
                    ScriptableShop[] shops = Resources.LoadAll<ScriptableShop>("Shops");// get all ScriptablePet in resources
                    List<int> duplicates = shops.ToList().FindDuplicates(shop => shop.name); // check for duplicates, then add to cache
                    if (duplicates.Count == 0)
                    {
                        cache = shops.ToDictionary(shop => shop.name, shop => shop);
                    }
                    else
                    {
                        foreach (int duplicate in duplicates)
                        {
                            Debug.LogError("Resources folder contains multiple ScriptablePet with ID:" + duplicate + ". If you are using subfolders like 'Warrior/Ring' and 'Archer/Ring', then rename them to 'Warrior/(Warrior)Ring' and 'Archer/(Archer)Ring' instead.");
                        }
                    }
                }
                return cache;
            }
        }
    }
}