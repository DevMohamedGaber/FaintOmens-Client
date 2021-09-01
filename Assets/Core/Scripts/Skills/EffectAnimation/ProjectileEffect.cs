using UnityEngine;
namespace Game.Components
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class ProjectileEffect : MonoBehaviour
    {
        [SerializeField] GameObject collision;
        [SerializeField] float speed = 35;
        Entity target;
        public void Set(Entity target)
        {
            this.target = target;
        }
        void FixedUpdate()
        {
            if(target != null)
            {
                Vector3 goal = target.collider.bounds.center;
                transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.fixedDeltaTime);
                transform.LookAt(goal);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        protected void OnTriggerEnter(Collider col)
        {
            if(target != null)
            {
                Entity entity = col.GetComponentInParent<Entity>();
                if(entity != null && entity == target)
                {
                    if(collision != null)
                    {
                        Instantiate(collision, transform.position, transform.rotation);
                    }
                    Destroy(gameObject);
                }
            }
        }
    }
}