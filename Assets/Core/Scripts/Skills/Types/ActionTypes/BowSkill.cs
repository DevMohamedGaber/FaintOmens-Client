using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Skill/Special/BowSkill", order=999)]
    public class BowSkill : TargetProjectileSkill {
        public override void OnCastStarted(Entity caster)
        {
            base.OnCastStarted(caster);
            if(effects != null && caster is Player player)
            {
                GameObject go = GameObject.Instantiate(effects, caster.transform, false);
                go.GetComponent<Components.AnimationEventEffects_Bow>().caster = player;
            }
        }
    }
}