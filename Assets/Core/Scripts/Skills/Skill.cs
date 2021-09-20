using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace Game
{
    [Serializable]
    public struct Skill
    {
        public ushort id;
        public byte level; // 0 if not learned, >0 if learned
        public uint experience;
        public double castTimeEnd; // server time. double for long term precision.
        public double cooldownEnd; // server time. double for long term precision.

        public ScriptableSkill data
        {
            get
            {
                if (!ScriptableSkill.dict.ContainsKey(id))
                    throw new KeyNotFoundException("There is no ScriptableSkill with id=" + id + ". Make sure that all ScriptableSkills are in the Resources folder so they are loaded properly.");
                return ScriptableSkill.dict[id];
            }
        }
        public int name => data.name;
        public float castTime => data.castTime.Get(level);
        public float cooldown => data.cooldown.Get(level);
        public float castRange => data.castRange.Get(level);
        public int manaCosts => data.manaCosts.Get(level);
        public bool followupDefaultAttack => data.followupDefaultAttack;
        public Sprite image => data.image;
        public bool learnDefault => data.learnDefault;
        public bool showCastBar => data.showCastBar;
        public bool cancelCastIfTargetDied => data.cancelCastIfTargetDied;
        public int maxLevel => data.maxLevel;
        public uint expMax => data.requiredSkillExperience[level - 1];
        public ScriptableSkill predecessor => data.predecessor;
        public int predecessorLevel => data.predecessorLevel;
        public int upgradeRequiredLevel => data.requiredLevel.Get(level+1);
        public string Name
        {
            get
            {
                return $"{LanguageManger.GetWord(id, LanguageDictionaryCategories.SkillName)} {(LanguageManger.Decide("Lvl.", "مستوي "))}{(int)level}";
            }
        }
        public string Description
        {
            get
            {
                return data.GetDescription((int)level);
            }
        }

        // events
        public bool CheckSelf(Entity caster, bool checkSkillReady=true)
        {
            return (!checkSkillReady || IsReady()) && data.CheckSelf(caster, level);
        }
        public bool CheckTarget(Entity caster)
        {
            return data.CheckTarget(caster);
        }
        public bool CheckDistance(Entity caster, out Vector3 destination)
        {
            return data.CheckDistance(caster, level, out destination);
        }
        public float CastTimeRemaining()
        {
            return NetworkTime.time >= castTimeEnd ? 0 : (float)(castTimeEnd - NetworkTime.time);
        }
        // we are casting a skill if the casttime remaining is > 0
        public bool IsCasting()
        {
            return CastTimeRemaining() > 0;
        }
        // how much time remaining until the cooldown ends? (using server time)
        public float CooldownRemaining()
        {
            return NetworkTime.time >= cooldownEnd ? 0 : (float)(cooldownEnd - NetworkTime.time);
        }

        public bool IsOnCooldown()
        {
            return CooldownRemaining() > 0;
        }

        public bool IsReady()
        {
            return !IsCasting() && !IsOnCooldown();
        }
    }
}