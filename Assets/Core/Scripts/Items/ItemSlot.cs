namespace Game
{
    [System.Serializable]
    public struct ItemSlot
    {
        public Item item;
        public uint amount;
        public bool isEmpty => amount < 1 || item.id < 1;
        public bool isEquipment => !isEmpty && item.data is EquipmentItem;
        // constructors
        public ItemSlot(Item item, uint amount = 1)
        {
            this.item = item;
            this.amount = amount;
        }
        public uint SellPrice()
        {
            return 0;
        }
    }
}