namespace Game
{
    [System.Serializable]
    public struct PetInfo
    {
        public static readonly PetInfo Empty = new PetInfo();
        public ushort id;
        public SummonableStatus status;
        public byte level;
        public uint experience;
        public Tier tier;
        public byte stars;
        public byte potential;
        public ushort vitality;
        public ushort intelligence;
        public ushort endurance;
        public ushort strength;
        public ScriptablePet data => ScriptablePet.dict[id];
        public int qualityBonus => (int)tier * 10 + stars;
        public uint experienceMax => Storage.data.pet.expMax.Get(level);
        // unsummoned data
        public int healthMax
        {
            get
            {
                int result = data.health.Get(level);
                result += tier != Tier.F ? (result / 5) * qualityBonus : 0;
                result += vitality * Storage.data.ratios.AP_Vitality;
                return result;
            }
        }
        public int manaMax
        {
            get
            {
                int result = data.mana.Get(level);
                result += tier != Tier.F ? (result / 5) * qualityBonus : 0;
                result += intelligence * Storage.data.ratios.AP_Intelligence_MANA;
                return result;
            }
        }
        public int p_atk
        {
            get
            {
                int result = data.pAtk.Get(level);
                result += tier != Tier.F ? (result / 5) * qualityBonus : 0;
                result += strength * Storage.data.ratios.AP_Strength_ATK;
                return result;
            }
        }
        public int m_atk
        {
            get
            {
                int result = data.mAtk.Get(level);
                result += tier != Tier.F ? (result / 5) * qualityBonus : 0;
                result += intelligence * Storage.data.ratios.AP_Intelligence_ATK;
                return result;
            }
        }
        public int p_def
        {
            get
            {
                int result = data.pDef.Get(level);
                result += tier != Tier.F ? (result / 5) * qualityBonus : 0;
                result += endurance * Storage.data.ratios.AP_Endurance + intelligence * Storage.data.ratios.AP_Strength_DEF;
                return result;
            }
        }
        public int m_def
        {
            get
            {
                int result = data.mDef.Get(level);
                result += tier != Tier.F ? (result / 5) * qualityBonus : 0;
                result += endurance * Storage.data.ratios.AP_Endurance + intelligence * Storage.data.ratios.AP_Intelligence_DEF;
                return result;
            }
        }
        public float block
        {
            get
            {
                float result = data.block.Get(level);
                result += tier != Tier.F ? (result / 5) * qualityBonus : 0;
                return result;
            }
        }
        public float antiBlock
        {
            get
            {
                float result = data.untiBlock.Get(level);
                result += tier != Tier.F ? (result / 5) * qualityBonus : 0;
                return result;
            }
        }
        public float critRate
        {
            get
            {
                float result = data.crit.Get(level);
                result += tier != Tier.F ? (result / 5) * qualityBonus : 0;
                return result;
            }
        }
        public float critDmg
        {
            get
            {
                float result = data.critDmg.Get(level);
                result += tier != Tier.F ? (result / 5) * qualityBonus : 0;
                return result;
            }
        }
        public float antiCrit
        {
            get
            {
                float result = data.untiCrit.Get(level);
                result += tier != Tier.F ? (result / 5) * qualityBonus : 0;
                return result;
            }
        }
        public uint battlepower
        {
            get
            {
                return System.Convert.ToUInt32(healthMax + manaMax + m_atk + p_atk + m_def + p_def + 
                        ((block + antiBlock + critRate + critDmg + antiCrit) * 100));
            }
        }
    }
}