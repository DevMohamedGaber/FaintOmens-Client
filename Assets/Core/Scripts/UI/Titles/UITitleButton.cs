using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Game.UI
{
    public class UITitleButton : MonoBehaviour {
        public Button btn;
        public Image image;
        public int id;
        public void Set(int titleId, Sprite sprite, float width, float height) {
            id = titleId;
            image.sprite = sprite;
            Rect rect = (transform as RectTransform).rect;
            rect.width = width;
            rect.height = height;
        }
    }
}