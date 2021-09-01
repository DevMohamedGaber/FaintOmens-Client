namespace Game
{
    [System.Serializable]
    public partial struct HotEventObjective
    {
        public string type;
        public int amount;
        public HotEventReward[] rewards;
    }
}