using System;
using System.Collections.Generic;
namespace Game
{
    [Serializable]
    public struct Socket
    {
        public static Socket Empty = new Socket();
        public short id;
        public BonusType type => data.bonusType;
        public float bonus => data.bonus;
        public bool isOpen => id > -1;
        public new string Name => $"{LanguageManger.GetItem(id)} {LanguageManger.Decide("Lvl", "مستوي")}.{data.level}";
        public string bonusText => LanguageManger.UseSymbols($"(+{data.bonusText})", "(", ")");
        public Socket(short id = -1)
        {
            this.id = id;
        }
        public GemItem data
        {
            get
            {
                if (!ScriptableItem.dict.ContainsKey(id))
                    throw new KeyNotFoundException("There is no ScriptableItem with ID=" + id + ". Make sure that all ScriptableItems are in the Resources folder so they are loaded properly.");
                return (GemItem)ScriptableItem.dict[id];
            }
        }
    }
}