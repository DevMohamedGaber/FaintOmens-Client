using UnityEngine;
namespace Game.Components
{
    public class AnimationEventEffects_Bow : AnimationEventEffects
    {
        [SerializeField] EffectInfo shotEffect;
        [SerializeField] EffectInfo buffEffect;
        [SerializeField] EffectInfo weaponBuffEffect;
        BowInfo bow;
        GameObject arrow;

        protected override void OnSetCaster()
        {
            base.OnSetCaster();
            if(caster.HasWeapon())
            {
                bow = caster.mainWeaponHolder.GetChild(0)?.GetComponent<BowInfo>();
                if(caster.secondaryWeaponHolder.childCount > 0)
                {
                    arrow = caster.secondaryWeaponHolder.GetChild(0).gameObject;
                }
            }
            if(bow.bowString != null)
            {
                bow.bowString.InHand = true;
            }
        }

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
                Weapon();
            }
            else if(EffectNumber == 2)
            {
                Buff();
            }
        }
        void Shot()
        {
            if(shotEffect.prefab != null && bow != null)
            {
                GameObject go = Instantiate(shotEffect.prefab, bow.center.position, bow.center.rotation);
                ProjectileEffect shot = go.GetComponent<ProjectileEffect>();
                if(shot != null)
                {
                    shot.Set(target);
                }
                if(bow.bowString != null)
                {
                    bow.bowString.InHand = false;
                }
                Destroy(go, shotEffect.destroyAfter);
            }
            else
            {
                Debug.Log("the shot effect or the bow are missing");
            }
        }
        void Weapon()
        {
            if(weaponBuffEffect.prefab != null)
            {
                GameObject go = Instantiate(weaponBuffEffect.prefab);
                AE_SetMeshToEffect setMeshToEffect = go.GetComponent<AE_SetMeshToEffect>();
                if(setMeshToEffect != null)
                {
                    // bow
                    if(setMeshToEffect.MeshType == AE_SetMeshToEffect.EffectMeshType.Bow && bow != null)
                    {
                        setMeshToEffect.Mesh = bow.gameObject;
                        go.transform.SetParent(bow.transform, false);
                    }
                    // arrow
                    else if(setMeshToEffect.MeshType == AE_SetMeshToEffect.EffectMeshType.Arrow && arrow != null)
                    {
                        setMeshToEffect.Mesh = arrow;
                        go.transform.SetParent(arrow.transform, false);
                    }
                    else
                    {
                        Debug.Log("weapon is missing");
                    }
                }
                Destroy(go, weaponBuffEffect.destroyAfter);
            }
            else
            {
                Debug.Log("weaponBuffEffect is missing");
            }
        }
        void Buff() {
            if(buffEffect.prefab != null)
            {
                GameObject go = Instantiate(buffEffect.prefab, caster.transform, false);
                Destroy(go, buffEffect.destroyAfter);
            }
        }

        [System.Serializable]
        public struct EffectInfo
        {
            public GameObject prefab;
            public float destroyAfter;
        }
    }
}