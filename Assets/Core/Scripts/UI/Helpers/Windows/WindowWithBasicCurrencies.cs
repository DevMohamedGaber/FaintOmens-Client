using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class WindowWithBasicCurrencies : Window
    {
        [Header("Currency")]
        [SerializeField] TMP_Text goldTxt;
        [SerializeField] TMP_Text diamondsTxt;
        [SerializeField] TMP_Text bDiamondsTxt;

        public override void UpdateCurrency()
        {
            if(goldTxt != null)
            {
                goldTxt.text = player.own.gold.ToString();
            }
            if(diamondsTxt != null)
            {
                diamondsTxt.text = player.own.diamonds.ToString();
            }
            if(bDiamondsTxt != null)
            {
                bDiamondsTxt.text = player.own.b_diamonds.ToString();
            }
        }
    }
}