namespace Game
{
    [System.Serializable]
    public struct PlayerModelData
    {
        public Gender gender;
        public PlayerModelPart body;
        public PlayerModelPart weapon;
        public ushort wing;
        public ushort soul;

        public void AddTo(ClothingCategory category, ushort id)
        {
            if(category == ClothingCategory.Body)
            {
                body.type = PlayerModelPartType.Clothing;
                body.id = id;
            }
            else if(category == ClothingCategory.Weapon)
            {
                weapon.type = PlayerModelPartType.Clothing;
                weapon.id = id;
            }
            else if(category == ClothingCategory.Wings)
            {
                wing = id;
            }
            else if(category == ClothingCategory.Soul)
            {
                soul = id;
            }
        }
    }
}