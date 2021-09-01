using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIDailySignDaySlot : MonoBehaviour {
        public Button button;
        public Text text;
        [SerializeField] UIItemSlot slot;
        public GameObject signed;
        public GameObject missed;
        public void OnClickRecieve(int day) {

        }
    }
}