using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIConfirmMessage : MonoBehaviour
    {
        public static UIConfirmMessage singleton;

        public static void Add(string msg) {

        }
        void Awake() {
            if(singleton == null)
                singleton = this;
        }
    }
}