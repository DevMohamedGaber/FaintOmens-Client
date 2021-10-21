using UnityEngine;
namespace Game.UI
{
    public class CharacterStats : Stats
    {
        public void Set(Player data)
        {
            hp.Set(data.healthMax);
        }
    }
}