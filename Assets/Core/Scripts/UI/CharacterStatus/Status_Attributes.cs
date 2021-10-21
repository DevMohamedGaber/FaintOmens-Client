using UnityEngine;
using TMPro;
namespace Game.UI
{
    [System.Serializable]
    public struct Status_Attributes
    {
        [SerializeField] TMP_Text vitality;
        [SerializeField] TMP_Text strength;
        [SerializeField] TMP_Text intelligence;
        [SerializeField] TMP_Text endurance;
        [SerializeField] TMP_Text freepoints;

        public void Set(Player player)
        {
            if(vitality != null)
            {
                vitality.text = player.own.vitality.ToString();
            }

            if(strength != null)
            {
                strength.text = player.own.strength.ToString();
            }

            if(intelligence != null)
            {
                intelligence.text = player.own.intelligence.ToString();
            }

            if(endurance != null)
            {
                endurance.text = player.own.endurance.ToString();
            }

            if(freepoints != null)
            {
                freepoints.text = player.own.freepoints.ToString();
            }
        }
    }
}