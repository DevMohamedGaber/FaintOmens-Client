using UnityEngine;
using UnityEngine.AI;
namespace Game
{
    public class MountBody : MonoBehaviour {
        [SerializeField] Animator animator;
        [SerializeField] NavMeshAgent agent;
        [SerializeField] Transform seatPosition;
        public int dataIndex;
        [SerializeField] Transform owner;
        public Transform seat => seatPosition;
        public void Moving(bool isMoving)
        {
            animator.SetBool("MOVING", isMoving);
        }
        public void Set(Transform ownerTransform, float speed)
        {
            agent.speed = speed;
            owner = ownerTransform;
        }
        void Update()
        {
            if(owner != null)
            {
                agent.Warp(owner.position);
                transform.rotation = owner.rotation;
            }
        }
    }
}