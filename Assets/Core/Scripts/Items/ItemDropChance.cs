namespace Game
{
    [System.Serializable]
    public class ItemDropChance
    {
        public ScriptableItem item;
        [UnityEngine.Range(0,1)]
        public float probability;
    }
}
