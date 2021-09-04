namespace Game
{
    [System.Serializable]
    public struct ShopItem
    {
        public ItemSlot item;
        public ShopCurrency currency;
        public uint cost;
        public uint maxPerCheckout;
    }
}