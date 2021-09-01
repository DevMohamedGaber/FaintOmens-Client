namespace Game
{
    [System.Serializable]
    public enum ItemType : byte
    {
        Item,
        UsableItem,
        ConsumableItem,
        EquipmentItem,
        WeaponItem,
        FeedItem,
        PetCard,
        MountCard,
        UpgradePetItem,
        UpgradeMountItem,
        Gem,
        Clothing,
        Potion
    }
}