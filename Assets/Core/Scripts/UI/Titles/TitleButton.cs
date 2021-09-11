using UnityEngine;
namespace Game.UI
{
    public class TitleButton : MonoBehaviour
    {
        [SerializeField] UnityEngine.UI.Image image;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] GameObject notAquired;
        [SerializeField] BasicButton _button;
        public BasicButton button
        {
            get
            {
                return _button;
            }
        }
        public void Set(ScriptableTitle title)
        {
            image.sprite = title.GetImage();
            nameTxt.text = title.GetName();
            notAquired.SetActive(!Player.localPlayer.own.titles.Contains((ushort)title.name));
        }
    }
}