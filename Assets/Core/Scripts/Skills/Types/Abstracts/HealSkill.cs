using UnityEngine;
using Mirror;
namespace Game
{
    public abstract class HealSkill : ScriptableSkill
    {
        public LinearInt healsHealth;
        public LinearInt healsMana;
        public GameObject effect;
    }
}