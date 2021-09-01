namespace Game
{
    [System.Serializable]
    public struct Currencies
    {
        public uint gold;
        public uint diamonds;
        public uint b_diamonds;
        public bool recieved;
        public bool HasCurrency()
        {
            return gold > 0 || diamonds > 0 || b_diamonds > 0;
        }
    }
}