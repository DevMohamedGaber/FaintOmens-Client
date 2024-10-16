namespace Game
{
    [System.Serializable]
    public struct PlayerClassData
    {
        public PlayerClass type;
        public byte rank;
        
        public ScriptableClass data => ScriptableClass.dict[type];
        public SubclassInfo sub => data.subs[rank];
        public int hp => sub.hp.Get(rank);
        public int mp => sub.mp.Get(rank);
        public int pAtk => sub.pAtk.Get(rank);
        public int pDef => sub.pDef.Get(rank);
        public int mAtk => sub.mAtk.Get(rank);
        public int mDef => sub.mDef.Get(rank);
        public float block => sub.block.Get(rank);
        public float untiBlock => sub.untiBlock.Get(rank);
        public float crit => sub.crit.Get(rank);
        public float critDmg => sub.critDmg.Get(rank);
        public float untiCrit => sub.untiCrit.Get(rank);
        public string Name => LanguageManger.GetClassName(((int)type * 3) + (type == PlayerClass.Warrior ? rank : rank - 1));
        public PlayerClassData(PlayerClass type, byte rank = 0)
        {
            this.type = type;
            this.rank = rank;
        }
        public bool Validate(PlayerClassData pClass)
        {
            return pClass.type == PlayerClass.Any || (pClass.type == type && pClass.rank >= rank);
        }
    }
}