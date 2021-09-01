using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UINotification : MonoBehaviour {
        [SerializeField] RTLTMPro.RTLTextMeshPro info;
        public void Show(string text) {
            info.text = text;
            Invoke("Remove", 5f);
        }
        public void Remove() => Destroy(gameObject);
    }
}