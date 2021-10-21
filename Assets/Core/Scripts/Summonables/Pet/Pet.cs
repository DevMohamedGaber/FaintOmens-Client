using UnityEngine;
using RTLTMPro;
using Mirror;
using System;
using Game.Components;
namespace Game
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NetworkNavMeshAgent))]
    public class Pet : Entity
    {
        [Header("Pet Components")]
        //public NetworkNavMeshAgent networkNavMeshAgent;
        [SerializeField] RTLTextMeshPro3D nameTxt;
        [SerializeField] Transform bodyHolder;
        [Header("Pet Synced")]
        [SyncVar(hook="OnStateChanged")] public EntityState state = EntityState.Idle;
        [SyncVar(hook="OnIdChanged"), SerializeField] ushort _id;
        [SyncVar(hook="OnTierChanged"), SerializeField] Tier tier;
        [SyncVar] GameObject _owner;
        //int dataIndex = -1;
        public Player owner => _owner != null ? _owner.GetComponent<Player>() : null;
        public ushort id => _id;
        //PetInfo data => dataIndex != -1 ? Player.localPlayer.own.pets[dataIndex] : PetInfo.Empty;
        public override float speed => owner != null ? owner.speed : base.speed;
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            if(id != 0 && owner != null)
            {
                UpdateModelInfo();
            }
        }
        void UpdateModelInfo()
        {
            if(id == 0)
            {
                Debug.Log("Pet summoned with id = 0");
                return;
            }
            if(bodyHolder != null && ScriptablePet.dict.TryGetValue(id, out ScriptablePet petData))
            {
                if(bodyHolder.childCount > 0)
                {
                    Destroy(bodyHolder.GetChild(0));
                }
                if(petData.prefab != null)
                {
                    PetModelInfo info = Instantiate(petData.prefab, bodyHolder, false).GetComponent<PetModelInfo>();
                    if(info != null)
                    {
                        if(animator != null)
                        {
                            if(info.animatorController != null)
                            {
                                animator.runtimeAnimatorController = info.animatorController;
                            }
                            if(info.avatar != null)
                            {
                                animator.avatar = info.avatar;
                            }
                            animator.Rebind();
                        }
                        if(info.collider != null)
                        {
                            collider = info.collider;
                        }
                        if(info.ownerNameTxt != null)
                        {
                            info.ownerNameTxt.text = owner.name;
                        }
                        if(info.nameTxt != null)
                        {
                            nameTxt = info.nameTxt;
                            nameTxt.text = petData.Name;
                        }
                        if(info.stunnedText != null)
                        {
                            stunnedOverlay = info.stunnedText;
                        }
                        info.Disappear();
                    }
                }
            }
        }
        void OnTierChanged(Tier oldTier, Tier newTier)
        {
            if(nameTxt != null)
            {
                nameTxt.color = UIManager.data.assets.tierColor[(int)tier];
            }
        }
        void OnStateChanged(EntityState oldState, EntityState newState)
        {
            foreach(Animator anim in GetComponentsInChildren<Animator>())
            {
                anim.SetBool("MOVING", IsMoving() && state != EntityState.Casting);
                anim.SetBool("CASTING", state == EntityState.Casting);
                anim.SetBool("STUNNED", state == EntityState.Stunned);
                anim.SetBool("DEAD", state == EntityState.Dead);
                foreach(Skill skill in skills)
                {
                    if(skill.level > 0 && !(skill.data is PassiveSkill))
                    {
                        anim.SetBool(skill.id.ToString(), skill.CastTimeRemaining() > 0);
                    }
                }
            }
        }
        void OnIdChanged(ushort oldId, ushort newId)
        {
            UpdateModelInfo();
        }
        public override bool IsWorthUpdating() => true;
        protected override void UpdateClient()
        {
            if(state == EntityState.Casting)
            { // keep looking at the target for server & clients (only Y rotation)
                if (target)
                {
                    LookAtY(target.transform.position);
                }
            }
        }
        public override bool CanAttack(Entity entity)
        {
            return base.CanAttack(entity) &&
                    (entity is Monster ||
                    (entity is Player && entity != owner) ||
                    (entity is Pet pet && pet.owner != owner));
        }
        public override void ResetMovement()
        {
            agent.ResetMovement();
        }
    }
}
