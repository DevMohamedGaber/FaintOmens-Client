using UnityEngine;
namespace Game.UI
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] protected Stat hp;
        public void ClearAll()
        {
            hp.Clear();
        }
        
    }
}