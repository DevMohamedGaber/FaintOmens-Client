using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class UIInventory : Window
    {
        [SerializeField] TMP_Text goldText;
        [SerializeField] TMP_Text diamondsText;
        [SerializeField] TMP_Text b_diamondsText;
        [SerializeField] UIEquipments equipments;
        [SerializeField] UIInventory_Bag bag;
        [SerializeField] UIInventory_Sell sell;
        public override void UpdateCurrency()
        {
            goldText.text = player.own.gold.ToString();
            diamondsText.text = player.own.diamonds.ToString();
            b_diamondsText.text = player.own.b_diamonds.ToString();
        }
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