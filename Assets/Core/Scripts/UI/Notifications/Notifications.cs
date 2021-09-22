using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class Notifications : MonoBehaviour
    {
        public static Notifications list;
        [SerializeField] UINotification prefab;
        // general
        public void Add(string en = "", string ar = "")
        {
            ShowNotification(LanguageManger.Decide(en, ar));
        }
        public void Add(string info = "")
        {
            ShowNotification(info);
        }
        void ShowNotification(string info) 
        {
            GameObject go = Instantiate(prefab.gameObject, transform, false);
            go.GetComponent<UINotification>().Show(info);
    #if UNITY_EDITOR
            Debug.Log($"Notifiy:{info}");
    #endif
        }
        void Awake()
        {
            if(list == null) 
            {
                list = this;
            }
        }
    }
}