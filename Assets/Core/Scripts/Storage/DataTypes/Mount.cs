namespace Game.StorageData
{
    [System.Serializable]
    public struct Mount
    {
        public byte lvlCap;
        public ExponentialUInt expMax;
        public byte starsCap;
        public ushort pointPerLvl;
        public int upgradeItemId;
        public uint[] upgradeReqCount;
        public int starsUpItemId;
        public uint[] starUpReqCount;
        public FeedItem[] feeds;
    }
}