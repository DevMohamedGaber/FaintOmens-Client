using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UINotifications : MonoBehaviour {
        public static UINotifications list;
        [SerializeField] UINotification prefab;
        // general
        public void Add(string en = "", string ar = "") => ShowNotification(LanguageManger.Decide(en, ar));
        public void Add(string info = "") => ShowNotification(info);
        void ShowNotification(string info) {
            GameObject go = Instantiate(prefab.gameObject, transform, false);
            go.GetComponent<UINotification>().Show(info);
    #if UNITY_EDITOR
            Debug.Log($"Notifiy:{info}");
    #endif
        }
        void Awake() {
            if(list == null) 
                list = this;
        }
        // custom
        public void SomethingWentWrong() => Add("Something went wrong, please try again", "حدث خطأ ما, برجاء المحاولة مرة اخري");
        public void AlreadyMarried() => Add("You are already married", "انت متزوح بالفعل");
        public void AlreadyFriends() => Add("You are already friends", "انت متصادقون بالفعل");
    }
}