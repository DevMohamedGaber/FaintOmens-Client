using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Skill/Normal/TargetProjectile", order=999)]
    public class TargetProjectileSkill : DamageSkill
    {
        public override bool CheckSelf(Entity caster, int skillLevel) => base.CheckSelf(caster, skillLevel);
        public override bool CheckTarget(Entity caster) => caster.target != null /*&& caster.CanAttack(caster.target)*/;
        public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
        {
            // target still around?
            if (caster.target != null)
            {
                destination = Utils.ClosestPoint(caster.target, caster.transform.position);
                return Utils.ClosestDistance(caster, caster.target) <= castRange.Get(skillLevel);
            }
            destination = caster.transform.position;
            return false;
        }

    }
}