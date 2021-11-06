namespace Game
{
    [System.Serializable]
    public struct GuildMember
    {
        public uint id;
        public string Name;
        public byte avatar;
        public byte level;
        public uint br;
        public PlayerClassData classInfo;
        public GuildRank rank;
        public uint contribution;
        public double online;
        public bool isOnline => online == 0;
    }
}