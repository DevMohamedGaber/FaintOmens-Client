using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class Store : Shop
    {
        [Header("Store")]
        [SerializeField] TMP_Text goldTxt;
        [SerializeField] TMP_Text diamondsTxt;
        [SerializeField] TMP_Text bDiamondsTxt;
        public override void UpdateCurrency()
        {
            goldTxt.text = player.own.gold.ToString();
            diamondsTxt.text = player.own.diamonds.ToString();
            bDiamondsTxt.text = player.own.b_diamonds.ToString();
        }
    }
}