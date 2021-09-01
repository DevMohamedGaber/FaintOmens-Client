using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Skill/Normal/TargetBuff", order=999)]
    public class TargetBuffSkill : BuffSkill
    {
        [SerializeField] bool attachToCaster = true;
        //public bool canBuffSelf = true;
        //public bool canBuffOthers = false; // so that players can buff other players
        //public bool canBuffEnemies = false; // so that players can buff monsters

        // helper function to determine the target that the skill will be cast on
        // (e.g. cast on self if targeting a monster that can't be buffed)
        /*Entity CorrectedTarget(Entity caster) {
            // targeting nothing? then try to cast on self
            if (caster.target == null)
                return canBuffSelf ? caster : null;

            // targeting self?
            if (caster.target == caster)
                return canBuffSelf ? caster : null;

            // targeting someone of same type? buff them or self
            if (caster.target.GetType() == caster.GetType()) {
                if (canBuffOthers)
                    return caster.target;
                else if (canBuffSelf)
                    return caster;
                else
                    return null;
            }
            // targeting an enemy? buff them or self
            if (caster.CanAttack(caster.target)) {
                if (canBuffEnemies)
                    return caster.target;
                else if (canBuffSelf)
                    return caster;
                else
                    return null;
            }

            // no valid target? try to cast on self or don't cast at all
            return canBuffSelf ? caster : null;
        }*/
        public override bool CheckTarget(Entity caster) => caster.target != null && caster.target.health > 0;
        public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
        {
            //Entity caster = CorrectedTarget(caster);
            if (caster != null)
            {
                destination = Utils.ClosestPoint(caster, caster.transform.position);
                return Utils.ClosestDistance(caster, caster) <= castRange.Get(skillLevel);
            }
            destination = caster.transform.position;
            return false;
        }
        public override void OnCastStarted(Entity caster)
        {
            base.OnCastStarted(caster);
            if(caster is Player player)
            {
                if(attachToCaster)
                {
                    GameObject.Instantiate(effects, player.transform, false);
                }
                else
                {
                    GameObject.Instantiate(effects, player.transform.position, player.transform.rotation);
                }
            }
        }
    }
}