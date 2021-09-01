using UnityEngine;
namespace Game.Components
{
    public abstract class AnimationEventEffects : MonoBehaviour, IAnimationEventEffects
    {
        [SerializeField] protected bool reqTarget = true;
        Player _caster;
        public Player caster
        {
            get => _caster;
            set
            {
                _caster = value;
                OnSetCaster();
            }
        }
        protected Entity target;
        public abstract void InstantiateEffect(int EffectNumber);
        protected virtual void OnSetCaster()
        {
            _caster.animationEventEffects = this;
            if(reqTarget && caster.target != null)
            {
                target = caster.target;
            }
        }
        public bool CasterExist()
        {
            if(caster == null)
            {
                Debug.Log("There is no caster");
                return false;
            }
            return true;
        }
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}