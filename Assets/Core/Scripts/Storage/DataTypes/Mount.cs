namespace Game.StorageData
{
    [System.Serializable]
    public struct Mount
    {
        public byte lvlCap;
        public ExponentialUInt expMax;
        public byte starsCap;
        public ushort pointPerLvl;
        public int trainItemId;
        public int upgradeItemId;
        public int starsUpItemId;
        public uint[] upgradeReqCount;
        public uint[] starUpReqCount;
        public ushort[] trainingExpMax;
        public FeedItem[] feeds;
    }
}