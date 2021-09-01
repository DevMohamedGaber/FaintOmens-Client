using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Skill/Normal/PassiveSkill", order=999)]
    public class PassiveSkill : BonusSkill
    {
        public override bool CheckTarget(Entity caster) => false;
        public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
        {
            destination = caster.transform.position;
            return false;
        }
    }
}