using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UITitleCategoryCollapsable : MonoBehaviour {
        public Text header;
        public Button button;
        public Transform content;
        [SerializeField] private Image icon;
        [SerializeField] private Sprite plusIcon;
        [SerializeField] private Sprite minusIcon;
        bool opened;
        public bool isOpened => opened;
        public void ToggleOpened() {
            opened = !opened;
            icon.sprite = opened ? minusIcon : plusIcon;
            content.gameObject.SetActive(opened);
        }
        void OnEnable() {
            button.onClick.AddListener(() => ToggleOpened());
        }
        void OnDisable() {
            button.onClick.RemoveAllListeners();
        }
    }
}