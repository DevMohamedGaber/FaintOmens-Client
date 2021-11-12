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
    }
}