using System.Collections.Generic;
namespace Game
{
    [System.Serializable]
    public struct Achievement
    {
        public ushort id;
        public bool claimed;
        public ScriptableAchievement data
        {
            get
            {
                if (!ScriptableAchievement.dict.ContainsKey(id))
                {
                    throw new System.Collections.Generic.KeyNotFoundException("There is no ScriptableAchievement with ID=" + id + ". Make sure that all ScriptableAchievement are in the Resources folder so they are loaded properly.");
                }
                return ScriptableAchievement.dict[id];
            }
        }
    }
}