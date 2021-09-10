using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using TMPro;
namespace Game
{
    [RequireComponent(typeof(Rigidbody))] // kinematic, only needed for OnTrigger
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Entity : NetworkBehaviourNonAlloc
    {
        [Header("Components")]
        public NavMeshAgent agent;
        public Animator animator;
    #pragma warning disable CS0109 // member does not hide accessible member
        public new Collider collider;
    #pragma warning restore CS0109 // member does not hide accessible member
    #region Variables
        #region Static
        [Header("Static Variables")]
        public GameObject damagePopupPrefab;
        public GameObject stunnedOverlay;
        public Transform effectMount;
        public DamageType classType;
        [HideInInspector] public bool inSafeZone;
        #endregion
        #region Synced
        [Header("Synced Variables")]
        [SerializeField, SyncVar(hook ="OnLevelChanged")] byte _level;
        public int level => (int)_level;
        //[SyncVar(hook="OnStateChanged")] public EntityState state = EntityState.Idle;
        [Header("Target")]
        [SerializeField, SyncVar(hook="OnTargetChanged")] GameObject _target;
        public Entity target => _target != null  ? _target.GetComponent<Entity>() : null;
        [Header("Combat")]
        [SyncVar] public sbyte currentSkill = -1;
        [SyncVar] public double lastCombatTime;
        public ScriptableSkill[] skillTemplates;
        public SyncListSkill skills = new SyncListSkill();
        public SyncListBuff buffs = new SyncListBuff(); // active buffs
        #endregion
    #endregion //Variables
    #region Attributes
        #region Health
        [Header("Health")]
        [SerializeField] protected LinearInt _healthMax = new LinearInt{baseValue=100};
        public virtual int healthMax {
            get {
                int i, result = _healthMax.Get(level);
                for(i = 0; i < skills.Count; i++) {
                    if(skills[i].level > 0 && skills[i].data is PassiveSkill) {
                        result += ((PassiveSkill)skills[i].data).healthMaxBonus.Get(skills[i].level);
                    }
                }
                if(buffs.Count > 0) {
                    for(i = 0; i < buffs.Count; i++) {
                        result += buffs[i].healthMaxBonus;
                    }
                }
                return result;
            }
        }
        public bool invincible = false; // GMs, Npcs, ...
        [SyncVar(hook="OnHealthChanged"), SerializeField] int _health = 1;
        public int health => Mathf.Min(_health, healthMax); 
        public bool healthRecovery = true; // can be disabled in combat etc.
        [SerializeField] protected LinearInt _healthRecoveryRate = new LinearInt{baseValue=1};
        public virtual int healthRecoveryRate {
            get {
                float result = _healthRecoveryRate.Get(level);
                foreach(Skill skill in skills)
                    if(skill.level > 0 && skill.data is PassiveSkill passiveSkill)
                        result += passiveSkill.healthPercentPerSecondBonus.Get(skill.level);
                for(int i = 0; i < buffs.Count; ++i)
                    result += buffs[i].healthPercentPerSecondBonus;
                return Convert.ToInt32(result * healthMax);
            }
        }
        #endregion
        #region Mana
        [Header("Mana")]
        [SerializeField] protected LinearInt _manaMax = new LinearInt{baseValue=100};
        public virtual int manaMax {
            get {
                int i, result = _manaMax.Get(level);
                for(i = 0; i < skills.Count; i++) {
                    if(skills[i].level > 0 && skills[i].data is PassiveSkill) {
                        result += ((PassiveSkill)skills[i].data).manaMaxBonus.Get(skills[i].level);
                    }
                }
                if(buffs.Count > 0) {
                    for(i = 0; i < buffs.Count; i++) {
                        result += buffs[i].manaMaxBonus;
                    }
                }
                return result;
            }
        }
        [SyncVar(hook="OnManaChanged")] int _mana = 1;
        public int mana => Mathf.Min(_mana, manaMax);
        public bool manaRecovery = true; // can be disabled in combat etc.
        [SerializeField] protected LinearInt _manaRecoveryRate = new LinearInt{baseValue=1};
        public virtual int manaRecoveryRate {
            get {
                float result = _manaRecoveryRate.Get(level);
                foreach (Skill skill in skills)
                    if (skill.level > 0 && skill.data is PassiveSkill passiveSkill)
                        result += passiveSkill.manaPercentPerSecondBonus.Get(skill.level);
                foreach (Buff buff in buffs)
                    result += buff.manaPercentPerSecondBonus;
                return Convert.ToInt32(result * manaMax);
            }
        }
        #endregion
        #region Attack
        [Header("Attack")]
        [SerializeField] protected LinearInt _p_atk = new LinearInt{baseValue=1};
        public virtual int p_atk {
            get {
                // base + passives + buffs
                int i, result = _p_atk.Get(level);
                for(i = 0; i < skills.Count; i++) {
                    if(skills[i].level > 0 && skills[i].data is PassiveSkill) {
                        result += ((PassiveSkill)skills[i].data).damageBonus.Get(skills[i].level);
                    }
                }
                if(buffs.Count > 0) {
                    for(i = 0; i < buffs.Count; i++) {
                        result += buffs[i].damageBonus;
                    }
                }
                return result;
            }
        }
        [SerializeField] protected LinearInt _m_atk = new LinearInt{baseValue=1};
        public virtual int m_atk {
            get {
                int i, result = _m_atk.Get(level);
                for(i = 0; i < skills.Count; i++) {
                    if(skills[i].level > 0 && skills[i].data is PassiveSkill) {
                        result += ((PassiveSkill)skills[i].data).damageBonus.Get(skills[i].level);
                    }
                }
                if(buffs.Count > 0) {
                    for(i = 0; i < buffs.Count; i++) {
                        result += buffs[i].damageBonus;
                    }
                }
                return result;
            }
        }
        #endregion
        #region Defense
        [Header("Defense")]
        [SerializeField] protected LinearInt _p_def = new LinearInt{baseValue=1};
        public virtual int p_def {
            get {
                // base + passives + buffs
                int i, result = _p_def.Get(level);
                for(i = 0; i < skills.Count; i++) {
                    if(skills[i].level > 0 && skills[i].data is PassiveSkill) {
                        result += ((PassiveSkill)skills[i].data).defenseBonus.Get(skills[i].level);
                    }
                }
                if(buffs.Count > 0) {
                    for(i = 0; i < buffs.Count; i++) {
                        result += buffs[i].defenseBonus;
                    }
                }
                return result;
            }
        }
        [SerializeField] protected LinearInt _m_def = new LinearInt{baseValue=1};
        public virtual int m_def {
            get {
                // base + passives + buffs
                int i, result = _m_def.Get(level);
                for(i = 0; i < skills.Count; i++) {
                    if(skills[i].level > 0 && skills[i].data is PassiveSkill) {
                        result += ((PassiveSkill)skills[i].data).defenseBonus.Get(skills[i].level);
                    }
                }
                if(buffs.Count > 0) {
                    for(i = 0; i < buffs.Count; i++) {
                        result += buffs[i].defenseBonus;
                    }
                }
                return result;
            }
        }
        #endregion
        #region Block
        [Header("Block")]
        [SerializeField] protected LinearFloat _blockChance;
        public virtual float blockChance {
            get {
                int i;
                float result = _blockChance.Get(level);
                for(i = 0; i < skills.Count; i++) {
                    if(skills[i].level > 0 && skills[i].data is PassiveSkill) {
                        result += ((PassiveSkill)skills[i].data).blockChanceBonus.Get(skills[i].level);
                    }
                }
                if(buffs.Count > 0) {
                    for(i = 0; i < buffs.Count; i++) {
                        result += buffs[i].blockChanceBonus;
                    }
                }
                return result;
            }
        }
        [SerializeField] protected LinearFloat _untiBlockChance;
        public virtual float untiBlockChance {
            get {
                float result = _untiBlockChance.Get(level);
                /* base + passives + buffs
                float passiveBonus = (from skill in skills
                                    where skill.level > 0 && skill.data is PassiveSkill
                                    select ((PassiveSkill)skill.data).bonusBlockChance.Get(skill.level)).Sum();
                float buffBonus = buffs.Sum(buff => buff.bonusBlockChance);*/
                return result;
            }
        }
        #endregion
        #region Critical
        [Header("Critical")]
        [SerializeField] protected LinearFloat _critRate;
        public virtual float critRate {
            get {
                int i;
                float result = _critRate.Get(level);
                for(i = 0; i < skills.Count; i++) {
                    if(skills[i].level > 0 && skills[i].data is PassiveSkill) {
                        result += ((PassiveSkill)skills[i].data).criticalChanceBonus.Get(skills[i].level);
                    }
                }
                if(buffs.Count > 0) {
                    for(i = 0; i < buffs.Count; i++) {
                        result += buffs[i].criticalChanceBonus;
                    }
                }
                return result;
            }
        }
        [SerializeField] protected LinearFloat _critDmg;
        public virtual float critDmg {
            get {
                float result = _critDmg.Get(level);
                /* base + passives + buffs
                float passiveBonus = (from skill in skills
                                    where skill.level > 0 && skill.data is PassiveSkill
                                    select ((PassiveSkill)skill.data).bonusCriticalChance.Get(skill.level)).Sum();
                float buffBonus = buffs.Sum(buff => buff.bonusCriticalChance);*/
                return result;
            }
        }
        [SerializeField] protected LinearFloat _antiCrit;
        public virtual float antiCrit {
            get {
                float result = _antiCrit.Get(level);
                /* base + passives + buffs
                float passiveBonus = (from skill in skills
                                    where skill.level > 0 && skill.data is PassiveSkill
                                    select ((PassiveSkill)skill.data).bonusCriticalChance.Get(skill.level)).Sum();
                float buffBonus = buffs.Sum(buff => buff.bonusCriticalChance);*/
                return result;
            }
        }
        #endregion
        #region Others
        [SerializeField] protected LinearFloat _untiStunChance;
        public virtual float untiStunChance {
            get {
                return _untiStunChance.Get(level);
            }
        }
        #endregion
        #region Speed
        [Header("Speed")]
        [SerializeField] protected LinearFloat _speed = new LinearFloat{baseValue=5};
        public virtual float speed {
            get {
                float result = _speed.Get(level);
                foreach (Skill skill in skills)
                    if (skill.level > 0 && skill.data is PassiveSkill passiveSkill)
                        result += passiveSkill.speedBonus.Get(skill.level);
                foreach (Buff buff in buffs)
                    result += buff.speedBonus;
                return result;
            }
        }
        #endregion
        #region Helpers
        public int MyAtkType {
            get {
                if(classType == DamageType.Physical)
                    return p_atk;
                return m_atk;
            }
        }
        //Battle Power
        public int battlepower {
            get {
                return healthMax + manaMax + m_atk + p_atk + m_def + p_def + 
                (int)(blockChance + untiBlockChance + critRate + critDmg + antiCrit);
            }
        }
        public float HealthPercent() => (health != 0 && healthMax != 0) ? (float)health / (float)healthMax : 0;
        public float ManaPercent() => (mana != 0 && manaMax != 0) ? (float)mana / (float)manaMax : 0;
        #endregion
    #endregion //Attributes
        protected abstract void UpdateClient();
        protected virtual void Awake() {}
        protected virtual void Start() {}
        public virtual void OnAggro(Entity entity) {}
        protected virtual void OnTargetChanged(GameObject oldTarget, GameObject newTarget) {}
        protected virtual void OnLevelChanged(byte oldLevel, byte newLevel) {}
        protected virtual void OnHealthChanged(int oldHp, int newHp) {}
        protected virtual void OnManaChanged(int oldMp, int newMp) {}
        //public virtual void OnCurrentSkillChanged(sbyte oldSkill, sbyte newSkill) {}
        public abstract void ResetMovement();
        void Update() {
            if (IsWorthUpdating()) {
                agent.speed = speed;// always apply speed to agent
                UpdateClient();
            }
            // update overlays in any case, except on server-only mode (also update for character selection previews etc. then)
            if (!isServerOnly) UpdateOverlays();
        }
        protected virtual void UpdateOverlays() {}
        public virtual bool IsWorthUpdating() => netIdentity.observers == null || netIdentity.observers.Count > 0 || IsHidden();
        public bool IsHidden() => netIdentity.visible == Visibility.ForceHidden;
        public float VisRange() => ((SpatialHashingInterestManagement)NetworkServer.aoi).visRange;
        public void LookAtY(Vector3 position) => transform.LookAt(new Vector3(position.x, transform.position.y, position.z));
        public bool IsMoving() => agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity != Vector3.zero;
        public virtual void Navigate(Vector3 destination, float stoppingDistance = 0f) {
            agent.stoppingDistance = stoppingDistance;
            agent.destination = destination;
        }
        
        // skills and attack
        public int GetSkillIndexByName(int skillName) {
            // (avoid FindIndex to minimize allocations)
            for (int i = 0; i < skills.Count; ++i)
                if (skills[i].name == skillName)
                    return i;
            return -1;
        }
        public int GetBuffIndexByName(int buffName) {
            // (avoid FindIndex to minimize allocations)
            for (int i = 0; i < buffs.Count; ++i)
                if (buffs[i].name == buffName)
                    return i;
            return -1;
        }
        public virtual bool CanAttack(Entity entity) => health > 0 && entity.health > 0 && entity != this &&
                !inSafeZone && !entity.inSafeZone &&
                !NavMesh.Raycast(transform.position, entity.transform.position, out NavMeshHit hit, NavMesh.AllAreas);
        public bool CastCheckSelf(Skill skill, bool checkSkillReady = true) => skill.CheckSelf(this, checkSkillReady);
        public bool CastCheckTarget(Skill skill) => skill.CheckTarget(this);
        public bool CastCheckDistance(Skill skill, out Vector3 destination) => skill.CheckDistance(this, out destination);
        // client 
        [Client] void ShowDamagePopup(int amount, AttackType damageType) {
            if (damagePopupPrefab != null) { // spawn the damage popup (if any) and set the text
                // showing it above their head looks best, and we don't have to use
                // a custom shader to draw world space UI in front of the entity
                Bounds bounds = collider.bounds;
                Vector3 position = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);

                GameObject popup = Instantiate(damagePopupPrefab, position, Quaternion.identity);
                if (damageType == AttackType.Normal)
                    popup.GetComponentInChildren<TextMeshPro>().text = amount.ToString();
                else if (damageType == AttackType.Block)
                    popup.GetComponentInChildren<TextMeshPro>().text = "<i>Block!</i>";
                else if (damageType == AttackType.Crit)
                    popup.GetComponentInChildren<TextMeshPro>().text = amount + " Crit!";
            }
        }
        [ClientRpc] void RpcOnDamageReceived(int amount, AttackType damageType) {
            // show popup above receiver's head in all observers via ClientRpc
            ShowDamagePopup(amount, damageType);
        }
        [ClientRpc] public void RpcSkillCastStarted(Skill skill) {
            if(health > 0) // validate: still alive?
                skill.data.OnCastStarted(this);
        }
        [ClientRpc] public void RpcSkillCastFinished(Skill skill) {
            if (health > 0) { // validate: still alive?
                Debug.Log("RpcSkillCastFinished");
                skill.data.OnCastFinished(this);// call scriptableskill event
                // maybe some other component needs to know about it too
                SendMessage("OnSkillCastFinished", skill, SendMessageOptions.DontRequireReceiver);
            }
        }
        [ClientRpc] public void RpcSkillCastCanceled() {
            if(health > 0 && currentSkill != -1) {
                skills[currentSkill].data.OnCastCanceled(this);
            }
        }
        // Triggers
        protected virtual void OnTriggerEnter(Collider col) {
            // check if trigger first to avoid GetComponent tests for environment
            if (col.isTrigger && col.GetComponent<Components.SafeZone>())
                inSafeZone = true;
        }
        protected virtual void OnTriggerExit(Collider col) {
            // check if trigger first to avoid GetComponent tests for environment
            if (col.isTrigger && col.GetComponent<Components.SafeZone>())
                inSafeZone = false;
        }
    }
}