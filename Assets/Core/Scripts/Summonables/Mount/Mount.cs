namespace Game
{
    [System.Serializable]
    public struct Mount
    {
        public static readonly Mount Empty = new Mount();
        public ushort id;
        public SummonableStatus status;
        public byte _level;
        public uint experience;
        public Tier tier;
        public byte _stars;
        public ushort vitality;
        public ushort intelligence;
        public ushort endurance;
        public ushort strength;
        public MountTraining training;
        public uint expMax => Storage.data.mount.expMax.Get(level);
        public ScriptableMount data => ScriptableMount.dict[id];
        public int level
        {
            set
            {
                _level = (byte)value;
            }
            get
            {
                return (int)_level;
            }
        }
        public int stars
        {
            set
            {
                _stars = (byte)value;
            }
            get
            {
                return (int)_stars;
            }
        }
        // unsummoned data
        public int healthMax
        {
            get
            {
                int result = data.health.Get(level);
                result += tier != Tier.F ? (result / 5) * (int)tier : 0;
                result += vitality * Storage.data.ratios.AP_Vitality;
                return result;
            }
        }
        public int manaMax
        {
            get
            {
                int result = data.mana.Get(level);
                result += tier != Tier.F ? (result / 5) * (int)tier : 0;
                result += intelligence * Storage.data.ratios.AP_Intelligence_MANA;
                return result;
            }
        }
        public int p_atk
        {
            get
            {
                int result = data.pAtk.Get(level);
                result += tier != Tier.F ? (result / 5) * (int)tier : 0;
                result += strength * Storage.data.ratios.AP_Strength_ATK;
                return result;
            }
        }
        public int m_atk
        {
            get
            {
                int result = data.mAtk.Get(level);
                result += tier != Tier.F ? (result / 5) * (int)tier : 0;
                result += intelligence * Storage.data.ratios.AP_Intelligence_ATK;
                return result;
            }
        }
        public int p_def
        {
            get
            {
                int result = data.pDef.Get(level);
                result += tier != Tier.F ? (result / 5) * (int)tier : 0;
                result += endurance * Storage.data.ratios.AP_Endurance + intelligence * Storage.data.ratios.AP_Strength_DEF;
                return result;
            }
        }
        public int m_def
        {
            get
            {
                int result = data.mDef.Get(level);
                result += tier != Tier.F ? (result / 5) * (int)tier : 0;
                result += endurance * Storage.data.ratios.AP_Endurance + intelligence * Storage.data.ratios.AP_Intelligence_DEF;
                return result;
            }
        }
        public float blockChance
        {
            get
            {
                float result = data.block.Get(level);
                result += tier != Tier.F ? (result / 5) * (int)tier : 0;
                return result;
            }
        }
        public float untiBlockChance
        {
            get
            {
                float result = data.untiBlock.Get(level);
                result += tier != Tier.F ? (result / 5) * (int)tier : 0;
                return result;
            }
        }
        public float criticalChance
        {
            get
            {
                float result = data.crit.Get(level);
                result += tier != Tier.F ? (result / 5) * (int)tier : 0;
                return result;
            }
        }
        public float criticalRate
        {
            get
            {
                float result = data.critDmg.Get(level);
                result += tier != Tier.F ? (result / 5) * (int)tier : 0;
                return result;
            }
        }
        public float untiCriticalChance
        {
            get
            {
                float result = data.untiCrit.Get(level);
                result += tier != Tier.F ? (result / 5) * (int)tier : 0;
                return result;
            }
        }
        public float speed
        {
            get
            {
                float result = data.speed.Get(level);
                result += tier != Tier.F ? (result / 5) * (int)tier : 0;
                return result;
            }
        }
        public uint battlepower
        {
            get
            {
                return System.Convert.ToUInt32(healthMax + manaMax + m_atk + p_atk + m_def + p_def + 
                        ((blockChance + untiBlockChance + criticalChance + criticalRate + untiCriticalChance) * 100));
            }
        }
    }
}