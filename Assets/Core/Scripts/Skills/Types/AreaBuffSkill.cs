using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Skill/Normal/AreaBuff", order=999)]
    public class AreaBuffSkill : BuffSkill
    {
        public override bool CheckTarget(Entity caster) => true;
        public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
        {
            destination = caster.transform.position;
            return true;
        }
    }
}