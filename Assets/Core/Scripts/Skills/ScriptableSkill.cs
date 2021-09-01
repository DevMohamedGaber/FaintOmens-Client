using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace Game
{
    public abstract class ScriptableSkill : ScriptableObjectNonAlloc
    {
        [Header("Info")]
        public bool followupDefaultAttack;
        [SerializeField, TextArea(1, 30)] protected string toolTip; // not public, use ToolTip()
        public Sprite image;
        public bool learnDefault; // normal attack etc.
        public bool showCastBar;
        public bool cancelCastIfTargetDied; // direct hit may want to cancel if target died. buffs doesn't care. etc.

        [Header("Requirements")]
        public ScriptableSkill predecessor; // this skill has to be learned first
        public int predecessorLevel = 1; // level of predecessor skill that is required
        public WeaponType reqWeapon = WeaponType.Any;
        public LinearInt requiredLevel; // required player level
        public LinearLong requiredSkillExperience;

        [Header("Properties")]
        public int maxLevel = 1;
        public LinearInt manaCosts;
        public LinearFloat castTime;
        public LinearFloat cooldown;
        public LinearFloat castRange;
        public GameObject effects;
        
        //[Header("Sound")]
        //public AudioClip castSound;
        // helpers
        public virtual bool CheckSelf(Entity caster, int skillLevel)
        {
            if(caster.health < 1 || caster.mana < manaCosts.Get(skillLevel))
                return false;
            if(caster is Player player)
            {
                if(reqWeapon == WeaponType.Any || player.IsEquipedWithWeapon(reqWeapon))
                    return false;
            }
            return true;
        }
        public abstract bool CheckTarget(Entity caster);
        public abstract bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination);
        // state
        public virtual void OnCastStarted(Entity caster)
        {
            if(caster is Player player)
            {
                player.onCastingStarted();
                /*if(weaponBuff != null && player.weaponHolder.HasWeapon()) {
                    GameObject go = Instantiate(weaponBuff);
                    AE_SetMeshToEffect eff = go.GetComponent<AE_SetMeshToEffect>();
                    eff.Mesh = player.weaponHolder.GetChild(0).gameObject;
                    eff.Initialize();
                }*/
            }
            //if (caster.audioSource != null && castSound != null)
            //{
            //    caster.audioSource.PlayOneShot(castSound);
            //}
        }
        public virtual void OnCastCanceled(Entity caster)
        {
            if(caster is Player player)
            {
                player.onCastingFinished();
            }
        }
        public virtual void OnCastFinished(Entity caster)
        {
            Debug.Log("OnCastFinished");
            if(caster is Player player)
            {
                player.onCastingFinished();
            }
        }
        static Dictionary<int, ScriptableSkill> cache;
        public static Dictionary<int, ScriptableSkill> dict
        {
            get
            {
                // not loaded yet?
                if(cache == null)
                {
                    // get all ScriptableSkills in resources
                    ScriptableSkill[] skills = Resources.LoadAll<ScriptableSkill>("");
                    // check for duplicates, then add to cache
                    List<int> duplicates = skills.ToList().FindDuplicates(skill => skill.name);
                    if (duplicates.Count == 0)
                    {
                        cache = skills.ToDictionary(skill => skill.name, skill => skill);
                    }
                    else
                    {
                        foreach (int duplicate in duplicates)
                            Debug.LogError("Resources folder contains multiple ScriptableSkills with the name " + duplicate + ". If you are using subfolders like 'Warrior/NormalAttack' and 'Archer/NormalAttack', then rename them to 'Warrior/(Warrior)NormalAttack' and 'Archer/(Archer)NormalAttack' instead.");
                    }
                }
                return cache;
            }
        }
    }
}
