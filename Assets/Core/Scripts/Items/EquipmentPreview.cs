namespace Game
{
    [System.Serializable]
    public struct EquipmentPreview
    {
        public ushort id;
        public Quality quality;

        public void Add(ItemSlot item)
        {
            id = item.item.id;
            quality = item.item.quality.current;
        }

        public EquipmentItem data
        {
            get
            {
                if (!ScriptableItem.dict.ContainsKey(id))
                    throw new System.Collections.Generic.KeyNotFoundException("There is no ScriptableItem with ID=" + id + ". Make sure that all ScriptableItems are in the Resources folder so they are loaded properly.");
                return (EquipmentItem)ScriptableItem.dict[id];
            }
        }
    }
}