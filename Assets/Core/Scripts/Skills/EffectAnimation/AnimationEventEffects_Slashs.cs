using UnityEngine;
namespace Game.Components
{
    public class AnimationEventEffects_Slashs : AnimationEventEffects
    {
        [SerializeField] EffectInfo[] effects;
        public override void InstantiateEffect(int num)
        {
            if(!CasterExist())
            {
                DestroySelf();
            }
            if(effects == null || effects.Length <= num)
            {
                Debug.Log("Incorrect effect number or effect is null");
                DestroySelf();
            }
            GameObject instance = Instantiate(effects[num].prefab, effects[num].position.position, effects[num].position.rotation);
            Destroy(instance, effects[num].DestroyAfter);
        }
        void Awake()
        {
            if(effects == null)
            {
                DestroySelf();
            }
        }
        [System.Serializable]
        public struct EffectInfo
        {
            public GameObject prefab;
            public Transform position;
            public float DestroyAfter;
        }
    }
}