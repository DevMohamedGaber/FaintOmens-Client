namespace Game
{
    [System.Serializable]
    public struct GuildPublicInfo
    {
        public static GuildPublicInfo Empty = new GuildPublicInfo();
        public uint id;
        public string name;
    }
}