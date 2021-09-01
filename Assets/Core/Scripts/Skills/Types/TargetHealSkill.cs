using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Skill/Normal/TargetHeal", order=999)]
    public class TargetHealSkill : HealSkill
    {
        public bool canHealSelf = true;
        public bool canHealOthers = false;
        Entity CorrectedTarget(Entity caster)
        {
            // targeting nothing? then try to cast on self
            if (caster.target == null)
            {
                return canHealSelf ? caster : null;
            }
            // targeting self?
            if (caster.target == caster)
            {
                return canHealSelf ? caster : null;
            }
            // targeting someone of same type? buff them or self
            if (caster.target.GetType() == caster.GetType())
            {
                if (canHealOthers)
                    return caster.target;
                else if (canHealSelf)
                    return caster;
                else
                    return null;
            }

            // no valid target? try to cast on self or don't cast at all
            return canHealSelf ? caster : null;
        }

        public override bool CheckTarget(Entity caster) => caster.target != null && caster.target.health > 0;
        public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
        {
            Entity target = CorrectedTarget(caster);
            if (target != null)
            {
                destination = Utils.ClosestPoint(target, caster.transform.position);
                return Utils.ClosestDistance(caster, target) <= castRange.Get(skillLevel);
            }
            destination = caster.transform.position;
            return false;
        }
    }
}