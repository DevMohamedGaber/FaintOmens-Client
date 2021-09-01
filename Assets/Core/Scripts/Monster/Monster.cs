using System;
using UnityEngine;
using Mirror;
namespace Game
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NetworkNavMeshAgent))]
    public class Monster : Entity
    {
        [Header("Components")]
        [SerializeField] RTLTMPro.RTLTextMeshPro3D nameTxt;
        [Header("Movement")]
        [Range(0, 1)] public float moveProbability = 0.1f;
        public float moveDistance = 10;
        public float followDistance = 20;
        [Range(0.1f, 1)] public float attackToMoveRangeRatio = 0.8f; // move as close as 0.8 * attackRange to a target
        [Header("Experience Reward")]
        public uint rewardExperience = 10;
        public uint rewardSkillExperience = 2;
        [Header("Loot")]
        public uint lootGoldMin = 0;
        public uint lootGoldMax = 10;
        public ItemDropChance[] dropChances;
        [Header("Respawn")]
        public float deathTime = 30f; // enough for animation & looting
        double deathTimeEnd; // double for long term precision
        public bool respawn = true;
        public float respawnTime = 10f;
        double respawnTimeEnd; // double for long term precision
        Vector3 startPosition;

        // the last skill that was casted, to decide which one to cast next
        int lastSkill = -1;
        [Header("Synced Data")]
        [SyncVar(hook="OnStateChanged")] public EntityState state = EntityState.Idle;
        public SyncListItemSlot inventory = new SyncListItemSlot();
        public string Name => LanguageManger.GetWord(name.ToInt(), LanguageDictionaryCategories.Monster);
        protected override void Start()
        {
            base.Start();
            // remember start position in case we need to respawn later
            startPosition = transform.position;
            if(nameTxt != null)
            {
                nameTxt.text = Name;
            }
        }
        void OnStateChanged(EntityState oldState, EntityState newState) {}
        public override void ResetMovement() => agent.ResetMovement();
        protected override void UpdateClient()
        {
            animator.SetBool("MOVING", state == EntityState.Moving && agent.velocity != Vector3.zero);
            animator.SetBool("CASTING", state == EntityState.Casting);
            animator.SetBool("STUNNED", state == EntityState.Stunned);
            animator.SetBool("DEAD", state == EntityState.Dead);
            foreach (Skill skill in skills)
            {
                animator.SetBool(skill.name.ToString(), skill.CastTimeRemaining() > 0);
            }
            if(state == EntityState.Casting)
            {
                // keep looking at the target for server & clients (only Y rotation)
                if (target)
                {
                    LookAtY(target.transform.position);
                }
            }
        }
        public override bool CanAttack(Entity entity)
        {
            return base.CanAttack(entity) && (entity is Player || entity is Pet);
        }
        int NextSkill()
        {
            // find the next ready skill, starting at 'lastSkill+1' (= next one)
            // and looping at max once through them all (up to skill.Count)
            //  note: no skills.count == 0 check needed, this works with empty lists
            //  note: also works if lastSkill is still -1 from initialization
            for(int i = 0; i < skills.Count; ++i)
            {
                int index = (lastSkill + 1 + i) % skills.Count;
                // could we cast this skill right now? (enough mana, skill ready, etc.)
                if (CastCheckSelf(skills[index]))
                {
                    return index;
                }
            }
            return -1;
        }
        public int InventorySlotsOccupied()
        {
            // count manually. Linq is HEAVY(!) on GC and performance
            int occupied = 0;
            foreach (ItemSlot slot in inventory)
            {
                if(slot.amount > 0)
                {
                    occupied++;
                }
            }
            return occupied;
        }
    }
}