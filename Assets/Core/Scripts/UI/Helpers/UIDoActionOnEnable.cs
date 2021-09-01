using UnityEngine;
using UnityEngine.Events;
namespace Game.UI
{
    public class UIDoActionOnEnable : MonoBehaviour
    {
        [SerializeField] UnityEvent action;
        void OnEnable()
        {
            if(action != null)
            {
                action.Invoke();
            }
        }
    }
}