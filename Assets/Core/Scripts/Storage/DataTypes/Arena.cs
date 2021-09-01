namespace Game.StorageData
{
    [System.Serializable]
    public struct Arena
    {
        public byte minLvl;
        public int cancelTime;
        public int matchDurationInMins;
        public byte pointsOnWin;
        public byte pointsOnLoss;
    }
}