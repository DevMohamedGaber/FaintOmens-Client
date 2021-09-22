using UnityEngine;
namespace Game.UI.ManagerLists
{
    [System.Serializable]
    public struct Assets
    {
        public Sprite[] tiers;
        public Sprite[] stars;
        public Color[] tierColor;
        public Sprite[] avatars;
        public Sprite[] gender;
        public Sprite[] currency;
        public Sprite[] discountArrows;
        public GameObject itemSlotPrefab;
        public GameObject workshopSlotPrefab;
        public Sprite[] socketSlot; // 0 => locked; 1 => empty
        public Sprite defaultItem;
    }
}