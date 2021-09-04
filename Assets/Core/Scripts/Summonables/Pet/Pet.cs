using UnityEngine;
using Mirror;
using System;
using RTLTMPro;
namespace Game
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NetworkNavMeshAgent))]
    public class Pet : Entity
    {
        [Header("Components")]
        public NetworkNavMeshAgent networkNavMeshAgent;
        [SerializeField] int id;
        [SerializeField] RTLTextMeshPro3D ownerNameTxt;
        [SerializeField] RTLTextMeshPro3D nameTxt;
        [SerializeField] SpriteRenderer tierImage;
        [SyncVar] GameObject _owner;
        public Player owner => _owner != null ? _owner.GetComponent<Player>() : null;
        [SyncVar(hook="OnStateChanged")] public EntityState state = EntityState.Idle;
        [SyncVar] public byte level;
        [SyncVar, SerializeField] Tier tier;
        [SyncVar, SerializeField] byte stars;
        [Header("Movement")]
        public float returnDistance = 5;
        public float followDistance = 10;
        public float teleportDistance = 20;
        [Range(0.1f, 1)] public float attackToMoveRangeRatio = 0.8f; // move as close as 0.8 * attackRange to a target
        [Header("Death")]
        public float deathTime = 2; // enough for animation
        double deathTimeEnd; // double for long term precision

        // use owner's speed if found, so that the pet can still follow the
        // owner if he is riding a mount, etc.
        public override float speed => owner != null ? owner.speed : base.speed;
        public bool defendOwner = true; // attack what attacks the owner
        public bool autoAttack = true; // attack what the owner attacks
        int dataIndex = -1;
        int lastSkill = -1;
        PetInfo data => Player.localPlayer.own.pets[dataIndex];
        #region Attributes
        #region Basics(Health/Mana/Speed)
        public override int healthMax {
            get {
                if(dataIndex == -1)
                    return 0;
                int result = data.vitality * Storage.data.AP_Vitality;
                result += data.tier != Tier.F ? (_healthMax.Get(level) / 5) * (int)data.tier : 0;
                return base.healthMax + result;
            }
        }
        public override int manaMax {
            get {
                if(dataIndex == -1)
                    return 0;
                int result = data.intelligence * Storage.data.AP_Intelligence_MANA;
                result += data.tier != Tier.F ? (_manaMax.Get(level) / 5) * (int)data.tier : 0;
                return base.manaMax + result;
            }
        }
        #endregion
        #region Attack
        public override int p_atk {
            get {
                if(dataIndex == -1)
                    return 0;
                int result = data.strength * Storage.data.AP_Strength_ATK;
                result += data.tier != Tier.F ? (_p_atk.Get(level) / 5) * (int)data.tier : 0;
                return base.p_atk + result;
            }
        }
        public override int m_atk {
            get {
                if(dataIndex == -1)
                    return 0;
                int result = data.intelligence * Storage.data.AP_Intelligence_ATK;
                result += data.tier != Tier.F ? (_m_atk.Get(level) / 5) * (int)data.tier : 0;
                return base.m_atk + result;
            }
        }
        #endregion
        #region Defense
        public override int p_def {
            get {
                if(dataIndex == -1)
                    return 0;
                int result = data.endurance * Storage.data.AP_Endurance + data.intelligence * Storage.data.AP_Strength_DEF;
                result += data.tier != Tier.F ? (_p_def.Get(level) / 5) * (int)data.tier : 0;
                return base.p_def + result;
            }
        }
        public override int m_def {
            get {
                if(dataIndex == -1)
                    return 0;
                int result = data.endurance * Storage.data.AP_Endurance + data.intelligence * Storage.data.AP_Intelligence_DEF;
                result += data.tier != Tier.F ? (_m_def.Get(level) / 5) * (int)data.tier : 0;
                return base.m_def + result;
            }
        }
        #endregion
        public uint battlepower => dataIndex == -1 ? 0 : 
        Convert.ToUInt32(healthMax + manaMax + m_atk + p_atk + m_def + p_def + 
        (blockChance + untiBlockChance + critRate + critDmg + antiCrit + untiStunChance + speed) * 100);
    #endregion
        void OnStateChanged(EntityState oldState, EntityState newState) {
            foreach(Animator anim in GetComponentsInChildren<Animator>()) {
                anim.SetBool("MOVING", IsMoving() && state != EntityState.Casting);
                anim.SetBool("CASTING", state == EntityState.Casting);
                anim.SetBool("STUNNED", state == EntityState.Stunned);
                anim.SetBool("DEAD", state == EntityState.Dead);
                foreach(Skill skill in skills) {
                    if(skill.level > 0 && !(skill.data is PassiveSkill))
                        anim.SetBool(skill.name.ToString(), skill.CastTimeRemaining() > 0);
                }
            }
        }
        public override void OnStartClient() {
            base.OnStartClient();
            if(ScriptablePet.dict.TryGetValue(id, out ScriptablePet info)) {
                ownerNameTxt.text = owner != null ? LanguageManger.UseSymbols(owner.name, "<", ">") : "";
                nameTxt.text = $"{info.Name} {LanguageManger.UseSymbols(tier.ToString(), "(", ")")}";
                nameTxt.color = UIManager.data.assets.tierColor[(int)tier];
                // put stars
                if(owner.id == Player.localPlayer.id) {
                    for(int i = 0; i < Player.localPlayer.own.pets.Count; i++) {
                        if(Player.localPlayer.own.pets[i].id == id) {
                            dataIndex = i;
                            break;
                        }
                    }
                }
            }
        }
        //public override bool IsWorthUpdating() { return true; }
        protected override void UpdateClient() {
            if(state == EntityState.Casting) {
                // keep looking at the target for server & clients (only Y rotation)
                if (target) LookAtY(target.transform.position);
            }
        }
        public override bool CanAttack(Entity entity) => base.CanAttack(entity) && (entity is Monster ||
                                    (entity is Player && entity != owner) || (entity is Pet pet && pet.owner != owner));
        public override void ResetMovement() => agent.ResetMovement();
    }
}
