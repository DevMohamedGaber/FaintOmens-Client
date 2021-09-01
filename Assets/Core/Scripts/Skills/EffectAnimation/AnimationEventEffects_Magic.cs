using UnityEngine;
namespace Game.Components
{
    public class AnimationEventEffects_Magic : AnimationEventEffects
    {
        [SerializeField] GameObject shotEffect;
        [SerializeField] bool summonNotShot;
        [SerializeField] GameObject buffEffect;
        bool shotInited;
        MagicWeaponInfo weapon;
        public override void InstantiateEffect(int EffectNumber)
        {
            if(!CasterExist())
            {
                DestroySelf();
            }
            if(reqTarget && target == null)
            {
                Debug.Log("No target found");
                DestroySelf();
            }
            if(EffectNumber == 0)
            {
                Shot();
            }
            else if(EffectNumber == 1)
            {
                Buff();
            }
        }
        void Shot()
        {
            if(shotInited)
                return;
            if(summonNotShot)
            {
                if(shotEffect != null)
                {
                    Instantiate(shotEffect, target.transform, false);
                }
            }
            else
            {
                if(shotEffect != null && weapon != null)
                {
                    GameObject go = Instantiate(shotEffect, weapon.tip.position, weapon.tip.rotation);
                    ProjectileEffect shot = go.GetComponent<ProjectileEffect>();
                    if(shot != null)
                    {
                        shot.Set(target);
                        shotInited = true;
                    }
                }
                else
                {
                    Debug.Log("the shot effect or the bow are missing");
                }
            }
        }
        void Buff()
        {
            if(buffEffect != null)
            {
                Instantiate(buffEffect, caster.transform, false);
            }
        }
        protected override void OnSetCaster()
        {
            base.OnSetCaster();
            if(caster.HasWeapon())
            {
                weapon = caster.mainWeaponHolder.GetChild(0)?.GetComponent<MagicWeaponInfo>();
            }
        }
        
        [System.Serializable]
        public struct EffectInfo
        {
            public GameObject prefab;
            public bool summonNotShot;
        }
    }
}