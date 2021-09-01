using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIMiniLoading : MonoBehaviour {
        [SerializeField] UILanguageDefiner text;
        public void Show(int code = 1) {
            text.SetCode(0, code);
            gameObject.SetActive(true);
        }
        public void Hide() => gameObject.SetActive(false);
    }
}