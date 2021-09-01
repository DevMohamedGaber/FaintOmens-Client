namespace Game.StorageData
{
    [System.Serializable]
    public struct Pet
    {
        public byte lvlCap;
        public ExponentialUInt expMax;
        public byte starsCap;
        public ushort pointPerLvl;
        public byte potentialToAP;
        public byte potentialMax;
        public int trainItemId;
        public int UpgradeItemId;
        public int starsUpItemId;
        public uint[] upgradeReqCount;
        public uint[] starUpReqCount;
        public FeedItem[] feeds;
    }
}