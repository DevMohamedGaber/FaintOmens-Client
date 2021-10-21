using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class Inventory : WindowWithBasicCurrencies, IWindowWithEquipments
    {
        [Header("Inventroy")]
        [SerializeField] UIEquipments equipments;
        [SerializeField] Inventory_Bag bag;
        [SerializeField] Inventory_Sell sell;
        public override void Refresh()
        {
            if(bag.isVisible)
            {
                bag.Refresh();
            }
            else if(sell.isVisible)
            {
                sell.Refresh();
            }
        }
        public void RefreshEquipments()
        {
            equipments.Refresh();
            UIPreviewManager.singleton.UpdateLocalPlayer();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            UIPreviewManager.singleton.InstantiateLocalPlayer();
        }
    }
}