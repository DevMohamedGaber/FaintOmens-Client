using UnityEngine;
using TMPro;
namespace Game.UI
{
    [System.Serializable]
    public struct Stat
    {
        [SerializeField] GameObject gameObject;
        [SerializeField] TMP_Text valueTxt;
        [SerializeField] TMP_Text bonusTxt;
        [SerializeField] string bonusColor;
        public void Set(int value, int bonus = 0)
        {
            if(value <= 0) // guard
                return;
            if(valueTxt != null)
            {
                valueTxt.text = value.ToString();
                if(bonus > 0)
                {
                    if(bonusTxt != null)
                    {
                        bonusTxt.text = $"<color={bonusColor}>{LanguageManger.UseSymbols($"+{bonus}", "(", ")")}</color>";
                    }
                    else
                    {
                        valueTxt.text += $" <color={bonusColor}>{LanguageManger.UseSymbols($"+{bonus}", "(", ")")}</color>";
                    }
                }
                else
                {
                    if(bonusTxt != null)
                    {
                        bonusTxt.text = string.Empty;
                    }
                }
            }
            else
            {
                Debug.Log("[valueTxt] is missing");
            }
            // show if hidden
            if(!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }
        public void Set(float value, float bonus = 0f)
        {
            if(value <= 0f) // guard
                return;
            if(valueTxt != null)
            {
                valueTxt.text = $"{value}%";
                if(bonus > 0f)
                {
                    if(bonusTxt != null)
                    {
                        bonusTxt.text = $"<color={bonusColor}>{LanguageManger.UseSymbols($"+{bonus}%", "(", ")")}</color>";
                    }
                    else
                    {
                        valueTxt.text += $" <color={bonusColor}>{LanguageManger.UseSymbols($"+{bonus}%", "(", ")")}</color>";
                    }
                }
                else
                {
                    if(bonusTxt != null)
                    {
                        bonusTxt.text = string.Empty;
                    }
                }
            }
            else
            {
                Debug.Log("[valueTxt] is missing");
            }
            // show if hidden
            if(!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }
        public void Clear()
        {
            if(gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}