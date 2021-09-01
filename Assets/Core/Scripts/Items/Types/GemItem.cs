using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Gem", order=999)]
    public class GemItem : ScriptableItem
    {
        [Header("Gem")]
        public byte level = 1;
        public BonusType bonusType;
        public float bonus;
        public bool isFloated;
        public string bonusText => isFloated ? bonus.ToString("F0") + "%" : ((int)bonus).ToString();
        void OnValidate()
        {
            type = ItemType.Gem;
        }
    }
}