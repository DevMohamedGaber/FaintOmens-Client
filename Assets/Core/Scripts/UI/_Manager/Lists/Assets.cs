using UnityEngine;
namespace Game.UI.ManagerLists
{
    [System.Serializable]
    public struct Assets
    {
        public Sprite[] classTypeIcons;
        public Sprite[] tiers;
        public Sprite[] stars;
        public Color[] tierColor;
        public Sprite[] avatars;
        public Sprite[] frames;
        public Sprite[] gender;
        public Sprite[] currency;
        public Sprite[] discountArrows;
        public Assets_Slots itemSlots;
        public Sprite[] socketSlot; // 0 => locked; 1 => empty
        public Sprite defaultItem;

        [System.Serializable]
        public struct Assets_Slots
        {
            public GameObject normal;
            public GameObject workshop;
            public GameObject selectable;
            public GameObject countable;
        }
    }
}