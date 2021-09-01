namespace Game
{
    [System.Serializable]
    public struct ItemQualityData
    {
        public Quality current;
        public Quality max;
        public ushort progress;
        public bool isGrowth => current < max;
        public ushort expMax => isGrowth ? Storage.data.item.equipmentQualityExpMax[(int)current] : (ushort)0;
        public ScriptableQuality data => ScriptableQuality.dict[(int)current];
        public void Reset()
        {
            current = Quality.Normal;
            max = Quality.Normal;
            progress = 0;
        }
    }
}