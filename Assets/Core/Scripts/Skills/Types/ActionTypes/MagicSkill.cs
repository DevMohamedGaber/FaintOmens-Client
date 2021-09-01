using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Skill/Special/MagicSkill", order=999)]
    public class MagicSkill : TargetProjectileSkill
    {
        public override void OnCastStarted(Entity caster)
        {
            base.OnCastStarted(caster);
            if(effects != null && caster is Player player)
            {
                GameObject go = GameObject.Instantiate(effects, caster.transform, false);
                go.GetComponent<Components.AnimationEventEffects_Magic>().caster = player;
            }
        }
    }
}