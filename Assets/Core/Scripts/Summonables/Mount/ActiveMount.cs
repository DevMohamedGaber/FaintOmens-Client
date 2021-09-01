namespace Game
{
    [System.Serializable]
    public struct ActiveMount
    {
        public ushort id;
        public bool mounted;

        public bool canMount => id > 0;
        public UnityEngine.GameObject prefab
        {
            get
            {
                if(id > 0 && ScriptableMount.dict.ContainsKey(id))
                {
                    return ScriptableMount.dict[id].prefab;
                }
                return null;
            }
        }
    }
}